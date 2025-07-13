using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using static System.Windows.Forms.AxHost;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        JObject livingRoomObj;
        JObject kitchenObj;
        JObject drainObj;
        JObject foyerObj;
        JObject backyardObj;
        JObject basementObj;
        JObject bedroomObj;
        JObject jennyRoomObj;
        JObject shopObj;
        JObject globals;

        JObject messages;

        JObject apData;
        string apDataPath;
        int apDataIndex = 0;

        RootObject stageData;
        ItemPool itemPool;
        Random r;
        string newIsoPath;
        int newPassword;

        int CoinFlagID = 1;
        int HappyBoxFlagID = 1;
        int ItemFlagID = 1;

        int apSpawnFlag = 1;
        int apHappyBoxFlagID = 1;
        int apCoinFlagID = 1;


        public Form1()
        {
            InitializeComponent();

            Random r = new Random();
            for (int i = 0; i < 10; i++)
            {
                seed.Text += (char)r.Next(33, 126);
            }


        }

        private void openISO_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "ISO File (*.iso)|*.iso";
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    isoFilePath.Text = ofd.FileName;
                }
            }
        }

        private void openDestination_Click(object sender, EventArgs e)
        {

            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.ShowNewFolderButton = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    destinationPath.Text = dialog.SelectedPath;
                }
            }
        }

        private void openAPZip_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "(*.json)|*.json";
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    apZipPath.Text = ofd.FileName;

                    string fileName = Path.GetFileName(ofd.FileName);

                    // Get file name and remove .zip from the path
                    seed.Text = fileName.Remove(fileName.Length - 5, 5);

                    apData = Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText(apZipPath.Text)) as JObject;

                    string pjCheck = apData.SelectToken("free_pjs").ToString();

                    if (pjCheck == "1")
                    {
                        freePJ.Checked = true;
                    }
                    else
                    {
                        freePJ.Checked = false;
                    }

                    string chargedGigaBatteryCheck = apData.SelectToken("charged_giga_battery").ToString();

                    if (chargedGigaBatteryCheck == "1")
                    {
                        batteryCharge.Checked = true;
                    }
                    else
                    {
                        batteryCharge.Checked = false;
                    }

                    string openUpstairsCheck = apData.SelectToken("open_upstairs").ToString();

                    if (openUpstairsCheck == "1")
                    {
                        openUpstairs.Checked = true;
                    }
                    else
                    {
                        openUpstairs.Checked = false;
                    }

                    string openDownStairsCheck = apData.SelectToken("open_downstairs").ToString();

                    if (openDownStairsCheck == "1")
                    {
                        openDownstairs.Checked = true;
                    }
                    else
                    {
                        openDownstairs.Checked = false;
                    }

                    string chibiVisionString = apData.SelectToken("chibi_vision_off").ToString();

                    if (chibiVisionString == "1")
                    {
                        chibiVision.Checked = true;
                    }
                    else
                    {
                        chibiVision.Checked = false;
                    }

                    logicSettings.SelectedItem = "No Logic";

                }
            }


        }


        private void isoFilePath_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void settingsLabel_Click(object sender, EventArgs e)
        {

        }
        private void seed_Click(object sender, EventArgs e)
        {

        }

        //This is where the magic happens!
        private void randomizeButton_Click(object sender, EventArgs e)
        {

            if (validInput())
            {

                PBar.Value = 10;

                //Takes each character of the string and casts it to an int, then adds each together to create a integer representation of the seed
                int randoSeed = 0;

                foreach (char c in seed.Text)
                {
                    randoSeed += (int)c;
                }

                newPassword = 200667;

                newIsoPath = destinationPath.Text + "\\chibiRando_" + randoSeed + ".iso";

                File.Copy(isoFilePath.Text, newIsoPath, true);

                initializeStages();


                //Any code that handles stage editing / randomization goes here!


                Stream stageDataStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ChibiRoboRando.itemChecks.json");
                StreamReader readStageData = new StreamReader(stageDataStream);
                Stream itemPoolStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ChibiRoboRando.itemPool.json");
                StreamReader readItemPool = new StreamReader(itemPoolStream);

                stageData = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(readStageData.ReadToEnd());
                itemPool = Newtonsoft.Json.JsonConvert.DeserializeObject<ItemPool>(readItemPool.ReadToEnd());

                r = new Random(randoSeed);

                //Performs the randomization based on the settings
                Dictionary<string, string> newSpoilerLog = shuffleItemsGlitchless();

                //Settings involving edits to globals

                //Turns off Chibi Vision for all objects if enabled
                if (chibiVision.Checked)
                {
                    for (int i = 0; i < 159; i++)
                    {
                        globals.SelectToken("items[" + i + "].flags.chibiVision").Replace(false);
                    }
                }

                //Randomizes foot passcode, if enabled
                if (passwordRando.Checked)
                {
                    newPassword = r.Next(100000, 999999);
                    globals.SelectToken("stats[12].name").Replace("Password: " + newPassword);
                    globals.SelectToken("items[124].description").Replace("The number \"" + newPassword + "\" is engraved\non the inside.");
                }


                //Add PJs to Shop if enabled
                if (freePJ.Checked)
                {
                    JToken unusedShopItem = shopObj.SelectToken("items[16]");
                    unusedShopItem.SelectToken("item").Replace("pajamas");
                    unusedShopItem.SelectToken("price").Replace(10);
                    unusedShopItem.SelectToken("limit").Replace(1);
                }


                //Edits for Open Downstairs
                if (openDownstairs.Checked)
                {
                    JToken unusedShopItem = shopObj.SelectToken("items[17]");
                    unusedShopItem.SelectToken("item").Replace("drake_redcrest_suit");
                    unusedShopItem.SelectToken("price").Replace(10);
                    unusedShopItem.SelectToken("limit").Replace(1);
                }

                //Battery Drain Settings
                if (walkingBatteryDrain.Checked)
                {
                    JToken batteryGlobal = globals.SelectToken("batteryGlobals");
                    batteryGlobal.SelectToken("walk").Replace("0");
                }

                if (joggingBatteryDrain.Checked)
                {
                    JToken batteryGlobal = globals.SelectToken("batteryGlobals");
                    batteryGlobal.SelectToken("jog").Replace("0");
                }

                if (runningDecreasesBattery.Checked)
                {
                    JToken batteryGlobal = globals.SelectToken("batteryGlobals");
                    batteryGlobal.SelectToken("run").Replace("0");
                }

                //Edits for Open Upstairs setting
                if (openUpstairs.Checked)
                {
                    JToken latestToken = foyerObj.SelectToken("objects[512]");

                    Stream openUpstairsStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ChibiRoboRando.openUpstairs.json");
                    StreamReader readOpenUpstairs = new StreamReader(openUpstairsStream);
                    latestToken.AddAfterSelf(Newtonsoft.Json.JsonConvert.DeserializeObject(readOpenUpstairs.ReadToEnd()) as JObject);
                }



                //Spoiler log output
                using (StreamWriter logOutput = new StreamWriter(File.OpenWrite(destinationPath.Text + "\\Spoiler Log " + randoSeed + ".txt")))
                {
                    logOutput.WriteLine("******");
                    logOutput.WriteLine("Seed: " + seed.Text);
                    logOutput.WriteLine("Mode: " + logicSettings.Text);
                    logOutput.WriteLine("Password:  " + newPassword);
                    logOutput.WriteLine("Open Upstairs: " + openUpstairs.Checked);
                    logOutput.WriteLine("Charged Battery: " + batteryCharge.Checked);
                    logOutput.WriteLine("Free PJs: " + freePJ.Checked);
                    logOutput.WriteLine("Open Downstairs: " + openDownstairs.Checked);
                    logOutput.WriteLine("Randomize Password: " + passwordRando.Checked);
                    logOutput.WriteLine("Chibi-Vision Off: " + chibiVision.Checked);
                    logOutput.WriteLine("******\n");

                    foreach (string key in newSpoilerLog.Keys)
                    {
                        logOutput.WriteLine(key + ": " + newSpoilerLog[key]);
                    }

                }

                reimportStages();

                PBar.Value = 100;

                statusDialog.Text += "\nISO Rebuilding Complete :)";
            }
        }

        private void initializeStages()
        {
            List<string> oldFiles = new List<string>();
            foreach (string f in Directory.GetFiles(Directory.GetCurrentDirectory()))
            {
                if (f.Substring(0, 4) == "stage")
                {
                    oldFiles.Add(f);
                }
            }
            foreach (string f in oldFiles)
            {
                File.Delete(f);
            }

            runUnplugCommand("stage export --iso \"" + newIsoPath + "\" stage07 -o \"" + Directory.GetCurrentDirectory() + @"\stage07.json" + "\"");
            runUnplugCommand("stage export --iso \"" + newIsoPath + "\" stage01 -o \"" + Directory.GetCurrentDirectory() + @"\stage01.json" + "\"");
            runUnplugCommand("stage export --iso \"" + newIsoPath + "\" stage11 -o \"" + Directory.GetCurrentDirectory() + @"\stage11.json" + "\"");
            runUnplugCommand("stage export --iso \"" + newIsoPath + "\" stage09 -o \"" + Directory.GetCurrentDirectory() + @"\stage09.json" + "\"");
            runUnplugCommand("stage export --iso \"" + newIsoPath + "\" stage03 -o \"" + Directory.GetCurrentDirectory() + @"\stage03.json" + "\"");
            runUnplugCommand("stage export --iso \"" + newIsoPath + "\" stage06 -o \"" + Directory.GetCurrentDirectory() + @"\stage06.json" + "\"");
            runUnplugCommand("stage export --iso \"" + newIsoPath + "\" stage04 -o \"" + Directory.GetCurrentDirectory() + @"\stage04.json" + "\"");
            runUnplugCommand("stage export --iso \"" + newIsoPath + "\" stage02 -o \"" + Directory.GetCurrentDirectory() + @"\stage02.json" + "\"");

            runUnplugCommand("globals export --iso \"" + newIsoPath + "\" -o \"" + Directory.GetCurrentDirectory() + @"\globals.json");

            runUnplugCommand("shop export --iso \"" + newIsoPath + "\" -o \"" + Directory.GetCurrentDirectory() + @"\shop.json" + "\"");

            //runUnplugCommand("messages export --iso \"" + newIsoPath + "\" -o \"" + Directory.GetCurrentDirectory() + @"\messages.xml" + "\"");

            //WriteXML("stage14:15:26", "Welcome To The Randomizer <br>");


            XmlDocument doc = new XmlDocument();
            doc.Load(Directory.GetCurrentDirectory() + @"\Resources\messages.xml");
            doc.Save(Directory.GetCurrentDirectory() + @"\messages.xml");

            globals = Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText("globals.json")) as JObject;

            livingRoomObj = Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText("stage07.json")) as JObject;
            kitchenObj = Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText("stage01.json")) as JObject;
            drainObj = Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText("stage11.json")) as JObject;
            backyardObj = Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText("stage09.json")) as JObject;
            basementObj = Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText("stage03.json")) as JObject;
            bedroomObj = Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText("stage06.json")) as JObject;
            jennyRoomObj = Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText("stage04.json")) as JObject;
            foyerObj = Newtonsoft.Json.JsonConvert.DeserializeObject(File.ReadAllText("stage02.json")) as JObject;

            string shopInput = File.ReadAllText("shop.json");
            shopObj = Newtonsoft.Json.JsonConvert.DeserializeObject(@"{ 'items': " + shopInput + "}") as JObject;


        }
        private void runUnplugCommand(string command)
        {
            using (Process cmd = new Process())
            {
                ////CD to swap to 

                //ProcessStartInfo cdCommandInfo = new ProcessStartInfo();

                //cdCommandInfo.UseShellExecute = false;
                //cdCommandInfo.WorkingDirectory = @"C:\Windows\System32";

                //cdCommandInfo.FileName = "cmd.exe";
                //cdCommandInfo.Verb = "runas";
                //cdCommandInfo.Arguments = "/C cd \"" + Directory.GetCurrentDirectory() + "\""; ;
                //cdCommandInfo.WindowStyle = ProcessWindowStyle.Minimized;
                //cdCommandInfo.RedirectStandardOutput = true;
                //cmd.StartInfo = cdCommandInfo;
                //cmd.Start();


                ProcessStartInfo unplugCommandInfo = new ProcessStartInfo();

                //string fullCommand = Directory.GetCurrentDirectory() + "" + command;
                string fullCommand = "" + Directory.GetCurrentDirectory() + "\\unplug.exe " + command;

                unplugCommandInfo.UseShellExecute = false;
                unplugCommandInfo.WorkingDirectory = @"C:\Windows\System32";

                unplugCommandInfo.FileName = "cmd.exe";
                unplugCommandInfo.Verb = "runas";
                unplugCommandInfo.Arguments = "/C " + fullCommand;
                unplugCommandInfo.WindowStyle = ProcessWindowStyle.Minimized;
                unplugCommandInfo.RedirectStandardOutput = true;
                cmd.StartInfo = unplugCommandInfo;
                cmd.Start();

                //statusDialog.Text += "\nFull Command: " + fullCommand;
                //statusDialog.Text += "\nArguments put into CMD: " + unplugCommandInfo.Arguments;

                StreamReader sr = cmd.StandardOutput;
                string test = sr.ReadToEnd();
                //statusDialog.Text += "\n" + sr.ReadToEnd();
                cmd.WaitForExit();
            }
        }


        private bool validInput()
        {
            statusDialog.Text = "";
            //Input Validation
            string isoValidation = isoFilePath.Text;
            isoValidation = isoValidation.Substring(isoValidation.Length - 4);

            if (isoValidation.ToLower() == ".iso")
            {
                statusDialog.Text += "Validated ISO";

            }
            else
            {
                statusDialog.Text += "[ERROR] Invalid file path to Chibi-Robo ISO";
                return false;
            }

            if (destinationPath.Text != "<- Set destination path")
            {
                statusDialog.Text += "\nValidated file path";
            }
            else
            {
                statusDialog.Text += "\n[ERROR] Invalid destination file path";
                return false;
            }

            if (logicSettings.SelectedItem != null)
            {
                statusDialog.Text += "\nValidation complete";
            }
            else
            {
                statusDialog.Text += "\n[ERROR] Please select a game mode from the dropdown menu.";
                return false;
            }

            return true;
        }

        //Picks locations for the items, puts them into the appropriate locations, and then returns the spoiler log
        private Dictionary<string, string> shuffleItemsGlitchless()
        {
            //*** SETUP ***


            //List of all checks
            List<ItemLocation> allLocations = new List<ItemLocation>();

            //Tracks what checks are occupied
            List<bool> occupiedChecks = new List<bool>();

            //This will let us build the spoiler log later!
            Dictionary<string, string> spoilerLog = new Dictionary<string, string>();

            ItemLocation chargerLocation = null;
            ItemLocation batteryLocation = null;
            ItemLocation legLocation;

            //Builds the list of checks and occupiedChecks counter
            for (int i = 0; i < stageData.rooms.Count; i++)
            {
                for (int j = 0; j < stageData.rooms[i].locations.Count(); j++)
                {
                    allLocations.Add(stageData.rooms[i].locations[j]);
                    occupiedChecks.Add(false);
                }
            }

            //Clears all key item checks and replaces them with coin_c objects
            foreach (ItemLocation location in allLocations)
            {
                if (allLocations.IndexOf(location) > (allLocations.Count() - stageData.rooms[8].locations.Count()))
                {
                    break;
                }
                for (int i = 0; i < itemPool.Items.Count - 8; i++)
                {
                    if (location.ObjectName == itemPool.Items[i].objectName)
                    {
                        shuffleItem("coin_c", occupiedChecks, new string[] { }, allLocations);

                        //insertItem("item_kami_kuzu", allLocations.IndexOf(location));
                        break;
                    }
                }
            }



            for (int i = 0; i < occupiedChecks.Count; i++)
                occupiedChecks[i] = false;


            //Flattens out shop checks with null values
            shopObj.SelectToken("items[3].item").Replace(null);
            shopObj.SelectToken("items[4].item").Replace(null);
            shopObj.SelectToken("items[5].item").Replace(null);
            shopObj.SelectToken("items[6].item").Replace(null);
            shopObj.SelectToken("items[7].item").Replace(null);
            shopObj.SelectToken("items[9].item").Replace(null);
            shopObj.SelectToken("items[10].item").Replace(null);
            shopObj.SelectToken("items[11].item").Replace(null);
            shopObj.SelectToken("items[12].item").Replace(null);
            shopObj.SelectToken("items[13].item").Replace(null);
            shopObj.SelectToken("items[14].item").Replace(null);
            shopObj.SelectToken("items[15].item").Replace(null);

            if (apData != null)
            {

                // Get All Locations Of Json for AP
                JObject locations = apData.SelectToken("Locations").ToObject<JObject>();

                //int shopId = 0;

                // Loop through each location josin
                foreach (KeyValuePair<string, JToken> location in locations)
                {

                    string locationName = location.Key;
                    string name = location.Value.SelectToken("name").ToString();
                    string objectName = location.Value.SelectToken("object").ToString();

                    var roomID = get_room_id_by_name(location.Key);

                    var roomObject = get_room_object_id_by_name(location.Key);

                    // Add the item as the multiworld item. Hopefully there is a way to add a
                    // new item down the road for new ingame text?
                    if (objectName.Contains("archipelago_item"))
                    {
                        objectName = "item_timer_15";
                        //objectName = "energyb";
                    }

                    // shop items are not the same as normal items
                    if (roomID != 8)
                    {
                        // Add the object name into the location
                        roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].object").Replace(objectName);

                        switch (objectName)
                        {
                            case "coin_c":
                            case "coin_s":
                            case "coin_g":
                            case "item_junk_a":
                            case "item_junk_b":
                            case "item_junk_c":
                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].flags[0]").AddAfterSelf("spawn");
                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].flags[0]").AddAfterSelf("interact");

                                break;
                            case "living_happy_box":
                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].flags[0]").AddAfterSelf("spawn");
                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].flags[0]").AddAfterSelf("explode");
                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].flags[0]").AddAfterSelf("climb");
                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].flags[0]").AddAfterSelf("clamber");
                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].flags[0]").AddAfterSelf("fall");
                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].flags[0]").AddAfterSelf("grab");

                                break;

                            case "item_chip_53":
                            case "item_chip_54":
                            case "item_hocyouki":
                            case "item_receipt":
                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].flags[0]").AddAfterSelf("spawn");
                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].flags[0]").AddAfterSelf("flash");
                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].flags[0]").AddAfterSelf("cull");
                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].flags[0]").AddAfterSelf("lift");
                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].flags[0]").AddAfterSelf("interact");

                                var posY = roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].position.y");
                                posY = ((float)posY) + 2.0f;

                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].position.y").Replace(posY);


                                break;
                            default:
                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].flags[0]").AddAfterSelf("spawn");
                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].flags[0]").AddAfterSelf("flash");
                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].flags[0]").AddAfterSelf("cull");
                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].flags[0]").AddAfterSelf("lift");
                                roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].flags[0]").AddAfterSelf("interact");
                               
                                break;
                        }
                    }
                    else
                    {

                        // Disable shop for now since you can't have duplicate items in shop / doesn't work nicly in multiworld yet
                        // The object name will need to be updated to the in game item ID
                        // Example gunpowder = item_kayaku

                        //shopObj.SelectToken("items[" + shopId + "].item").Replace(objectName);
                        //shopObj.SelectToken("items["+ shopId +"].price").Replace(10);
                        //shopObj.SelectToken("items["+ shopId + "].limit").Replace(1);

                        //Console.WriteLine(objectName);

                        //roomObject.SelectToken("items[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].item").Replace(name);
                        //roomObject.SelectToken("items[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].price").Replace(10);
                        //roomObject.SelectToken("items[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].limit").Replace(1);
                        //roomObject.SelectToken("items[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].item").Replace(objectName);

                        //shopId++;

                    }


                    roomObject.SelectToken("objects[" + stageData.rooms[roomID].locations[apDataIndex].ID + "].spawnFlag").Replace(apSpawnFlag);

                    //Console.WriteLine("Index: " + apDataIndex + " Flag: " + apSpawnFlag );

                    //Console.WriteLine("Room ID: " + roomID);

                    //Console.WriteLine("Stage Data Desc: " + stageData.rooms[roomID].locations[apDataIndex].Description);

                    //spoilerLog.Add(name + " " + apDataIndex + " " + roomID, stageData.rooms[roomID].locations[apDataIndex].Description);

                    apSpawnFlag++;
                    apDataIndex++;

                    if (apSpawnFlag == 135)
                    {
                        apSpawnFlag = 1;
                    }


                }

                JToken unusedShopItem = shopObj.SelectToken("items[17]");
                unusedShopItem.SelectToken("item").Replace("drake_redcrest_suit");
                unusedShopItem.SelectToken("price").Replace(10);
                unusedShopItem.SelectToken("limit").Replace(1);

                //livingRoomObj.SelectToken("objects[447].object").Replace("item_mag_cup");

            }
            else
            {

                spoilerLog.Add("Locations: ", "");

                #region Junk Items

                for (int i = 0; i < 28; i++)
                {
                    spoilerLog.Add("10M Coin " + (i + 1), allLocations[shuffleItem("coin_c", occupiedChecks, new string[] { "shop" }, allLocations)].Description);
                }

                for (int i = 0; i < 7; i++)
                {
                    spoilerLog.Add("50M Coin " + (i + 1), allLocations[shuffleItem("coin_s", occupiedChecks, new string[] { "shop" }, allLocations)].Description);
                }

                for (int i = 0; i < 4; i++)
                {
                    spoilerLog.Add("100M Coin " + (i + 1), allLocations[shuffleItem("coin_g", occupiedChecks, new string[] { "shop" }, allLocations)].Description);
                }

                for (int i = 0; i < 15; i++)
                {
                    spoilerLog.Add("Happy Block " + (i + 1), allLocations[shuffleItem("living_happy_box", occupiedChecks, new string[] { "shop" }, allLocations)].Description);
                }
                #endregion

                #region Key Item Important Checks
                //Shuffles Charger
                int chargerCheck;
                if (openUpstairs.Checked)
                    chargerCheck = shuffleItem("item_chibi_house_denti_2", occupiedChecks, new string[] { "divorce" }, allLocations);
                else
                    chargerCheck = shuffleItem("item_chibi_house_denti_2", occupiedChecks, new string[] { "ladder", "bridge", "divorce" }, allLocations);

                spoilerLog.Add("Giga-Charger", allLocations[chargerCheck].Description);
                chargerLocation = allLocations[chargerCheck];


                //Shuffles Uncharged Battery

                int uBatteryCheck;
                if (openUpstairs.Checked)
                    uBatteryCheck = shuffleItem("item_deka_denchi", occupiedChecks, new string[] { "divorce" }, allLocations);
                else
                    uBatteryCheck = shuffleItem("item_deka_denchi", occupiedChecks, new string[] { "ladder", "bridge", "divorce" }, allLocations);

                spoilerLog.Add("Giga-Battery", allLocations[uBatteryCheck].Description);
                batteryLocation = allLocations[uBatteryCheck];

                if (batteryLocation.Prereqs.Contains("toothbrush") || chargerLocation.Prereqs.Contains("toothbrush"))
                {
                    spoilerLog.Add("Toothbrush", allLocations[shuffleItem("item_brush", occupiedChecks, new string[] { "ladder", "bridge", "toothbrush" }, allLocations)].Description);
                }
                else
                {
                    spoilerLog.Add("Toothbrush", allLocations[shuffleItem("item_brush", occupiedChecks, new string[] { "toothbrush" }, allLocations)].Description);
                }

                if (batteryLocation.Prereqs.Contains("squirter") || batteryLocation.Prereqs.Contains("frog suit") || chargerLocation.Prereqs.Contains("squirter") || chargerLocation.Prereqs.Contains("frog suit"))
                {
                    spoilerLog.Add("Squirter", allLocations[shuffleItem("item_tyuusyaki", occupiedChecks, new string[] { "squirter", "frog suit", "ladder", "bridge" }, allLocations)].Description);
                }
                else
                {
                    spoilerLog.Add("Squirter", allLocations[shuffleItem("item_tyuusyaki", occupiedChecks, new string[] { "squirter", "frog suit" }, allLocations)].Description);
                }

                if (batteryLocation.Prereqs.Contains("blaster") || chargerLocation.Prereqs.Contains("blaster"))
                {
                    spoilerLog.Add("Chibi-Blaster", allLocations[shuffleItem("cb_cannon_lv_2", occupiedChecks, new string[] { "ladder", "bridge", "blaster" }, allLocations)].Description);
                }
                else
                {
                    spoilerLog.Add("Chibi-Blaster", allLocations[shuffleItem("cb_cannon_lv_2", occupiedChecks, new string[] { "blaster" }, allLocations)].Description);
                }

                if (batteryLocation.Prereqs.Contains("radar") || chargerLocation.Prereqs.Contains("radar"))
                {
                    spoilerLog.Add("Chibi-Radar", allLocations[shuffleItem("cb_radar", occupiedChecks, new string[] { "ladder", "bridge", "radar" }, allLocations)].Description);
                }
                else
                {
                    spoilerLog.Add("Chibi-Radar", allLocations[shuffleItem("cb_radar", occupiedChecks, new string[] { "radar" }, allLocations)].Description);
                }


                if (batteryLocation.Prereqs.Contains("mug") || chargerLocation.Prereqs.Contains("mug"))
                {
                    spoilerLog.Add("Mug", allLocations[shuffleItem("item_mag_cup", occupiedChecks, new string[] { "ladder", "bridge", "mug" }, allLocations)].Description);
                }
                else
                {
                    spoilerLog.Add("Mug", allLocations[shuffleItem("item_mag_cup", occupiedChecks, new string[] { "mug" }, allLocations)].Description);
                }

                if (batteryLocation.Prereqs.Contains("spoon") || chargerLocation.Prereqs.Contains("spoon"))
                {
                    spoilerLog.Add("Spoon", allLocations[shuffleItem("item_spoon", occupiedChecks, new string[] { "spoon", "ladder", "bridge" }, allLocations)].Description);
                }
                else
                {
                    spoilerLog.Add("Spoon", allLocations[shuffleItem("item_spoon", occupiedChecks, new string[] { "spoon" }, allLocations)].Description);
                }

                if (batteryLocation.Prereqs.Contains("charge chip") || chargerLocation.Prereqs.Contains("charge chip"))
                {
                    spoilerLog.Add("Charge Chip", allLocations[shuffleItem("item_chip_53", occupiedChecks, new string[] { "charge chip", "ladder", "bridge" }, allLocations)].Description);
                }
                else
                {
                    spoilerLog.Add("Charge Chip", allLocations[shuffleItem("item_chip_53", occupiedChecks, new string[] { "charge chip" }, allLocations)].Description);
                }

                spoilerLog.Add("Red Shoe", allLocations[shuffleItem("item_peets_kutu", occupiedChecks, new string[] { "red shoe", "ladder", "bridge" }, allLocations)].Description);


                if (batteryCharge.Checked)
                {
                    spoilerLog.Add("Charged Giga-Battery", allLocations[shuffleItem("item_deka_denchi_full", occupiedChecks, new string[] { }, allLocations)].Description);
                }

                #endregion

                //Checks that are Key Items but don't lock progression or check access
                #region Key Item (Misc.) Checks

                spoilerLog.Add("Toy Receipt", allLocations[shuffleItem("item_receipt", occupiedChecks, new string[] { "divorce" }, allLocations)].Description);

                spoilerLog.Add("Range Chip", allLocations[shuffleItem("item_chip_54", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Alien Ear Chip", allLocations[shuffleItem("item_hocyouki", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Chibi-Battery", allLocations[shuffleItem("item_c_denchi", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Free Rangers Photo", allLocations[shuffleItem("item_army_photo", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Red Block", allLocations[shuffleItem("item_t_block_6", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Green Block", allLocations[shuffleItem("item_t_block_4", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("White Block", allLocations[shuffleItem("item_t_block_3", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Red Crayon", allLocations[shuffleItem("item_kure_1", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Yellow Crayon", allLocations[shuffleItem("item_kure_3", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Green Crayon", allLocations[shuffleItem("item_kure_4", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Purple Crayon", allLocations[shuffleItem("item_kure_5", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Dog Tags", allLocations[shuffleItem("item_tug", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Bandage", allLocations[shuffleItem("item_houtai", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Ticket Stub", allLocations[shuffleItem("item_ticket", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Gunpowder", allLocations[shuffleItem("item_kayaku", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Hot Rod", allLocations[shuffleItem("item_car_item", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Space Scrambler", allLocations[shuffleItem("item_nwing_item", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Scurvy Splinter", allLocations[shuffleItem("npc_hock_ship_114", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Passed-Out Frog", allLocations[shuffleItem("item_frog", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Dinahs Teeth", allLocations[shuffleItem("item_rex_tooth", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Snorkel", allLocations[shuffleItem("item_goggle", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("AA Battery", allLocations[shuffleItem("item_denchi_3", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("C Battery", allLocations[shuffleItem("item_denchi_2", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("D Battery", allLocations[shuffleItem("item_denchi_1", occupiedChecks, new string[] { }, allLocations)].Description);

                spoilerLog.Add("Wedding Ring", allLocations[shuffleItem("item_papa_yubiwa", occupiedChecks, new string[] { }, allLocations)].Description);

                // 10 Frog Rings

                for (int i = 0; i < 10; i++)
                {
                    spoilerLog.Add("Frog Ring " + (i + 1), allLocations[shuffleItem("item_frog_ring", occupiedChecks, new string[] { "shop" }, allLocations)].Description);
                }
                #endregion

                int legCheck;
                legCheck = shuffleItem("item_left_foot", occupiedChecks, new string[] { }, allLocations);

                spoilerLog.Add("Giga-Robo's Left Leg", allLocations[legCheck].Description);
                legLocation = allLocations[legCheck];

            }


            return spoilerLog;
        }


        //Randomizes the location of the given item
        private int shuffleItem(string objectName, List<bool> occupiedLocations, string[] prerequisites, List<ItemLocation> allChecks)
        {
            while (true)
            {
                int nextCheck = r.Next(0, allChecks.Count() - 1);

                if (occupiedLocations[nextCheck] == true || !validLocation(nextCheck, prerequisites, allChecks))
                {

                }
                else
                {
                    occupiedLocations[nextCheck] = true;

                    //Determine the room we are editing
                    int relativeCheck = 0;
                    JToken roomToEdit;
                    int roomIndex;

                    switch (nextCheck)
                    {
                        case int n when n < stageData.rooms[0].locations.Count():
                            relativeCheck = nextCheck;
                            roomToEdit = livingRoomObj;
                            roomIndex = 0;
                            break;
                        case int n when n < stageData.rooms[0].locations.Count() + stageData.rooms[1].locations.Count():
                            relativeCheck = nextCheck - stageData.rooms[0].locations.Count();
                            roomToEdit = kitchenObj;
                            roomIndex = 1;
                            break;
                        case int n when n < stageData.rooms[0].locations.Count() + stageData.rooms[1].locations.Count + stageData.rooms[2].locations.Count():
                            relativeCheck = nextCheck - (stageData.rooms[0].locations.Count() + stageData.rooms[1].locations.Count());
                            roomToEdit = drainObj;
                            roomIndex = 2;
                            break;
                        case int n when n < stageData.rooms[0].locations.Count() + stageData.rooms[1].locations.Count() + stageData.rooms[2].locations.Count() + stageData.rooms[3].locations.Count():
                            relativeCheck = nextCheck - (stageData.rooms[0].locations.Count() + stageData.rooms[1].locations.Count + stageData.rooms[2].locations.Count());
                            roomToEdit = foyerObj;
                            roomIndex = 3;
                            break;
                        case int n when n < stageData.rooms[0].locations.Count() + stageData.rooms[1].locations.Count() + stageData.rooms[2].locations.Count() + stageData.rooms[3].locations.Count() + stageData.rooms[4].locations.Count():
                            relativeCheck = nextCheck - (stageData.rooms[0].locations.Count() + stageData.rooms[1].locations.Count + stageData.rooms[2].locations.Count() + stageData.rooms[3].locations.Count());
                            roomToEdit = basementObj;
                            roomIndex = 4;
                            break;
                        case int n when n < stageData.rooms[0].locations.Count() + stageData.rooms[1].locations.Count() + stageData.rooms[2].locations.Count() + stageData.rooms[3].locations.Count() + stageData.rooms[4].locations.Count() + stageData.rooms[5].locations.Count():
                            relativeCheck = nextCheck - (stageData.rooms[0].locations.Count() + stageData.rooms[1].locations.Count + stageData.rooms[2].locations.Count() + stageData.rooms[3].locations.Count() + stageData.rooms[4].locations.Count());
                            roomToEdit = backyardObj;
                            roomIndex = 5;
                            break;
                        case int n when n < stageData.rooms[0].locations.Count() + stageData.rooms[1].locations.Count() + stageData.rooms[2].locations.Count() + stageData.rooms[3].locations.Count() + stageData.rooms[4].locations.Count() + stageData.rooms[5].locations.Count() + stageData.rooms[6].locations.Count():
                            relativeCheck = nextCheck - (stageData.rooms[0].locations.Count() + stageData.rooms[1].locations.Count + stageData.rooms[2].locations.Count() + stageData.rooms[3].locations.Count() + stageData.rooms[4].locations.Count() + stageData.rooms[5].locations.Count());
                            roomToEdit = jennyRoomObj;
                            roomIndex = 6;
                            break;
                        case int n when n < stageData.rooms[0].locations.Count() + stageData.rooms[1].locations.Count() + stageData.rooms[2].locations.Count() + stageData.rooms[3].locations.Count() + stageData.rooms[4].locations.Count() + stageData.rooms[5].locations.Count() + stageData.rooms[6].locations.Count() + stageData.rooms[7].locations.Count():
                            relativeCheck = nextCheck - (stageData.rooms[0].locations.Count() + stageData.rooms[1].locations.Count + stageData.rooms[2].locations.Count() + stageData.rooms[3].locations.Count() + stageData.rooms[4].locations.Count() + stageData.rooms[5].locations.Count() + stageData.rooms[6].locations.Count());
                            roomToEdit = bedroomObj;
                            roomIndex = 7;
                            break;
                        case int n when n < stageData.rooms[0].locations.Count() + stageData.rooms[1].locations.Count() + stageData.rooms[2].locations.Count() + stageData.rooms[3].locations.Count() + stageData.rooms[4].locations.Count() + stageData.rooms[5].locations.Count() + stageData.rooms[6].locations.Count() + stageData.rooms[7].locations.Count() + stageData.rooms[8].locations.Count():
                            relativeCheck = nextCheck - (stageData.rooms[0].locations.Count() + stageData.rooms[1].locations.Count + stageData.rooms[2].locations.Count() + stageData.rooms[3].locations.Count() + stageData.rooms[4].locations.Count() + stageData.rooms[5].locations.Count() + stageData.rooms[6].locations.Count() + stageData.rooms[7].locations.Count());
                            roomToEdit = shopObj;
                            roomIndex = 8;
                            break;
                        default:
                            statusDialog.Text += "\nSomething has TERRIBLY fucked up in shuffleItem() trying to determine a item check's room location";
                            roomToEdit = null;
                            roomIndex = -1;
                            break;
                    }

                    insertItem(objectName, relativeCheck, roomToEdit, roomIndex);
                    return nextCheck;
                }
            }

        }

        //Determines if a location is a valid position for an object given the prerequisites
        private bool validLocation(int location, string[] prerequisites, List<ItemLocation> allChecks)
        {
            foreach (string p in prerequisites)
            {
                if (allChecks[location].Prereqs.Contains(p) || allChecks[location].Prereqs.Contains("suitcase"))
                    return false;
            }
            return true;
        }


        //Inserts objectName at given location, assuming location is pulled from allLocations
        private void insertItem(string objectName, int relativeLocation, JToken roomObject, int roomIndex)
        {

            //Standard rooms
            if (roomIndex == 8)
            {
                //Shop checks take an item name rather than an object name. Using the supplied object name, we can get the item name of the matching object in itemPool.json
                string itemName = "";
                foreach (Item i in itemPool.Items)
                {
                    if (i.objectName == objectName)
                    {
                        itemName = i.itemName;
                    }
                }
                roomObject.SelectToken("items[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].item").Replace(itemName);

                if (itemName != "item_frog_ring" || itemName != "frog_ring")
                {
                    roomObject.SelectToken("items[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].limit").Replace(1);
                }
            }
            else
            {
                roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].object").Replace(objectName);


                //Setting the correct flags for the new object
                int finalFlagIndex = roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags").Children().Count() - 1;

                List<JToken> oldFlags = new List<JToken>();

                foreach (JToken flag in roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags").Children())
                {
                    oldFlags.Add(flag);
                }

                for (int i = 1; i < oldFlags.Count; i++)
                {
                    oldFlags[i].Remove();
                }

                switch (objectName)
                {
                    case "coin_c":
                    case "coin_s":
                    case "coin_g":
                    case "item_junk_a":
                    case "item_junk_b":
                    case "item_junk_c":
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags[0]").AddAfterSelf("spawn");
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags[0]").AddAfterSelf("interact");
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].spawnFlag").Replace(CoinFlagID);
                        CoinFlagID++;

                        if (CoinFlagID == 165)
                        {
                            CoinFlagID = 1;
                        }
                        break;

                    case "living_happy_box":
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags[0]").AddAfterSelf("spawn");
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags[0]").AddAfterSelf("explode");
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags[0]").AddAfterSelf("climb");
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags[0]").AddAfterSelf("clamber");
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags[0]").AddAfterSelf("fall");
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags[0]").AddAfterSelf("grab");
                        //roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].spawnFlag").Replace(HappyBoxFlagID);
                        //HappyBoxFlagID++;
                        break;
                    case "item_chip_53":
                    case "item_chip_54":
                    case "item_hocyouki":
                    case "item_receipt":
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags[0]").AddAfterSelf("spawn");
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags[0]").AddAfterSelf("flash");
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags[0]").AddAfterSelf("cull");
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags[0]").AddAfterSelf("lift");
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags[0]").AddAfterSelf("interact");
                        
                        var posY = roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].position.y");

                        posY = ((float)posY) + 2.0f;

                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].position.y").Replace(posY);
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].spawnFlag").Replace(ItemFlagID);

                        Console.WriteLine(objectName);
                        Console.WriteLine(ItemFlagID);

                        ItemFlagID++;

                        if (ItemFlagID == 165)
                        {
                            ItemFlagID = 1;
                        }

                        break;
                    default:
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags[0]").AddAfterSelf("spawn");
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags[0]").AddAfterSelf("flash");
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags[0]").AddAfterSelf("cull");
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags[0]").AddAfterSelf("lift");
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].flags[0]").AddAfterSelf("interact");
                        roomObject.SelectToken("objects[" + stageData.rooms[roomIndex].locations[relativeLocation].ID + "].spawnFlag").Replace(ItemFlagID);

                        Console.WriteLine(objectName);
                        Console.WriteLine(ItemFlagID);

                        ItemFlagID++;

                        if (ItemFlagID == 165)
                        {
                            ItemFlagID = 1;
                        }

                        break;
                }

            }
        }

        //Reimports the JSON stage and shop data into the ISO
        private void reimportStages()
        {
            File.WriteAllText("stage01.json", kitchenObj.ToString());
            File.WriteAllText("stage02.json", foyerObj.ToString());
            File.WriteAllText("stage03.json", basementObj.ToString());
            File.WriteAllText("stage04.json", jennyRoomObj.ToString());
            File.WriteAllText("stage06.json", bedroomObj.ToString());
            File.WriteAllText("stage07.json", livingRoomObj.ToString());
            File.WriteAllText("stage09.json", backyardObj.ToString());
            File.WriteAllText("stage11.json", drainObj.ToString());
            File.WriteAllText("globals.json", globals.ToString());

            PBar.Value = 45;

            // Import custom scritping First

            // Birthday Party Into
            runUnplugCommand("script assemble --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\Resources\stage14.us" + "\"");

            // Living Room
            //runUnplugCommand("script assemble --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\Resources\stage07.us" + "\"");

            // Update Messages
            //runUnplugCommand("messages import --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\Resources\messages.xml" + "\"");

            // Globals
            runUnplugCommand("script assemble --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\Resources\globals.us" + "\"");
            runUnplugCommand("globals import --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\globals.json" + "\"");

            //System.Diagnostics.Debug.WriteLine(Directory.GetCurrentDirectory() + @"\messages.xml" + "\"");

            string test = shopObj.ToString().Substring(14, shopObj.ToString().Length - 15);
            //JSON formatting for the shop is borked so this is the reconversion into the form that Unplug is looking for
            File.WriteAllText("shop.json", shopObj.ToString().Substring(14, shopObj.ToString().Length - 15));

            // Running unplug commands
            // https://github.com/adierking/unplug/blob/main/docs/tour.md#editing-cutscene-messages
            runUnplugCommand("stage import --iso \"" + newIsoPath + "\" stage01 \"" + Directory.GetCurrentDirectory() + @"\stage01.json" + "\"");
            runUnplugCommand("stage import --iso \"" + newIsoPath + "\" stage02 \"" + Directory.GetCurrentDirectory() + @"\stage02.json" + "\"");
            runUnplugCommand("stage import --iso \"" + newIsoPath + "\" stage03 \"" + Directory.GetCurrentDirectory() + @"\stage03.json" + "\"");
            runUnplugCommand("stage import --iso \"" + newIsoPath + "\" stage04 \"" + Directory.GetCurrentDirectory() + @"\stage04.json" + "\"");
            runUnplugCommand("stage import --iso \"" + newIsoPath + "\" stage06 \"" + Directory.GetCurrentDirectory() + @"\stage06.json" + "\"");

            PBar.Value = 50;

            runUnplugCommand("stage import --iso \"" + newIsoPath + "\" stage07 \"" + Directory.GetCurrentDirectory() + @"\stage07.json" + "\"");
            runUnplugCommand("stage import --iso \"" + newIsoPath + "\" stage09 \"" + Directory.GetCurrentDirectory() + @"\stage09.json" + "\"");
            runUnplugCommand("stage import --iso \"" + newIsoPath + "\" stage11 \"" + Directory.GetCurrentDirectory() + @"\stage11.json" + "\"");

            runUnplugCommand("shop import --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\shop.json" + "\"");



            PBar.Value = 60;

            List<string> oldFiles = new List<string>();
            foreach (string f in Directory.GetFiles(Directory.GetCurrentDirectory()))
            {
                if (f.Substring(0, 4) == "stage")
                {
                    oldFiles.Add(f);
                }
            }
            foreach (string f in oldFiles)
            {
                File.Delete(f);
            }
        }

        private JObject get_room_object_id_by_name(string location)
        {
            string roomTempName = location.Substring(0, location.IndexOf("-") + 1).Trim(new Char[] { ' ', '-' });

            if (roomTempName == "Living Room")
            {
                return livingRoomObj;
            }
            else if (roomTempName == "Kitchen")
            {
                return kitchenObj;
            }
            else if (roomTempName == "Sink Drain")
            {
                return drainObj;
            }
            else if (roomTempName == "Foyer")
            {
                return foyerObj;
            }
            else if (roomTempName == "Basement")
            {
                return basementObj;
            }
            else if (roomTempName == "Backyard")
            {
                return backyardObj;
            }
            else if (roomTempName == "Jenny's Room")
            {
                return jennyRoomObj;
            }
            else if (roomTempName == "Bedroom")
            {
                return bedroomObj;
            }
            else if (roomTempName == "Chibi House")
            {
                return shopObj;
            }
            else
            {
                return null;
            }

        }

        string lastRoom = "Living Room";
        private int get_room_id_by_name(string location)
        {

            //Console.WriteLine("Checked Location: " + location);

            string roomTempName = location.Substring(0, location.IndexOf("-") + 1).Trim(new Char[] { ' ', '-' });

            //Console.WriteLine("apDataIndex: " + apDataIndex);

            //Console.WriteLine("apSpawnFlag: " + apSpawnFlag);

            if (roomTempName == "Living Room")
            {
                return 0;
            }
            else if (roomTempName == "Kitchen")
            {
                if (lastRoom != "Kitchen")
                {
                    lastRoom = roomTempName;
                    apDataIndex = 0;
                    apSpawnFlag = 1;

                }

                return 1;
            }
            else if (roomTempName == "Sink Drain")
            {
                if (lastRoom != "Sink Drain")
                {
                    lastRoom = roomTempName;
                    apDataIndex = 0;
                    apSpawnFlag = 1;

                }

                return 2;
            }
            else if (roomTempName == "Foyer")
            {
                if (lastRoom != "Foyer")
                {
                    lastRoom = roomTempName;
                    apDataIndex = 0;
                    apSpawnFlag = 1;

                }

                return 3;
            }
            else if (roomTempName == "Basement")
            {
                if (lastRoom != "Basement")
                {
                    lastRoom = roomTempName;
                    apDataIndex = 0;
                    apSpawnFlag = 1;

                }

                return 4;
            }
            else if (roomTempName == "Backyard")
            {
                if (lastRoom != "Backyard")
                {
                    lastRoom = roomTempName;
                    apDataIndex = 0;
                    apSpawnFlag = 1;

                }

                return 5;
            }
            else if (roomTempName == "Jenny's Room")
            {
                if (lastRoom != "Jenny's Room")
                {
                    lastRoom = roomTempName;
                    apDataIndex = 0;
                    apSpawnFlag = 1;

                }

                return 6;
            }
            else if (roomTempName == "Bedroom")
            {
                if (lastRoom != "Bedroom")
                {
                    lastRoom = roomTempName;
                    apDataIndex = 0;
                    apSpawnFlag = 1;

                }

                return 7;
            }
            else if (roomTempName == "Chibi House")
            {
                if (lastRoom != "Chibi House")
                {
                    lastRoom = roomTempName;
                    apDataIndex = 0;
                    apSpawnFlag = 1;

                }

                return 8;
            }
            else
            {
                return -1;
            }


        }

        private void title_Click(object sender, EventArgs e)
        {

        }

        private void openUpstairs_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void freePJ_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void passwordRando_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void openDownstairs_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (PBar.Value == 100)
            {
                timer1.Stop();
            }
        }

        private void walkingBatteryDrain_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void batteryCharge_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }


    }
}
