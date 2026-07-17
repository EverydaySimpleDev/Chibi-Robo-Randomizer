using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
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

        RootObject stageData;
        ItemPool itemPool;
        Random r;
        string newIsoPath;
        int newLeftFootPassword;
        int newSuitCasePassword;

        int apSpawnFlag = 1;

        int apItemVoice = 0;
        string supportedAPVersion = "1.2.5";

        bool optOpenUpstairs;
        bool optChibiVisionOff;
        bool optRandomizePasswords;

        //List of all checks
        List<ItemLocation> allLocations = new List<ItemLocation>();

        //Tracks what checks are occupied
        List<bool> occupiedChecks = new List<bool>();

        //This will let us build the spoiler log later!
        Dictionary<string, string> spoilerLog = new Dictionary<string, string>();

        // Tracks (apCode, objectID) per stage file for anti-respawn subroutine generation
        Dictionary<string, List<(int code, int objID)>> stageAntiRespawnLocs = new Dictionary<string, List<(int code, int objID)>>();

        // Maps each AP location name to its code (0-220) for flag(2100+code) anti-respawn tracking
        static readonly Dictionary<string, int> apLocationCodes = new Dictionary<string, int>
        {
            { "Living Room - Frog Ring (Behind Window)", 0 },
            { "Living Room - Frog Ring (Corkboard)", 1 },
            { "Living Room - Frog Ring (Shelf)", 2 },
            { "Living Room - Wastepaper by Trashbin B", 17 },
            { "Living Room - Candy Wrapper above Trashbin A", 18 },
            { "Living Room - Wastepaper by Trashbin A", 19 },
            { "Living Room - Cupholder Wastepaper", 20 },
            { "Living Room - Cookie Crumbs under Table", 21 },
            { "Living Room - Cookie Crumbs by Record Player", 22 },
            { "Living Room - Toothbrush", 23 },
            { "Living Room - Wastepaper by Door to Kitchen", 34 },
            { "Living Room - Fireplace Wastepaper A", 35 },
            { "Living Room - Fireplace Wastepaper B", 36 },
            { "Living Room - Wastepaper on Stack of Books", 37 },
            { "Living Room - Couch Wastepaper B", 38 },
            { "Living Room - Wastepaper by Toothbrush Spawn", 41 },
            { "Living Room - Wastepaper below Cupholder", 42 },
            { "Living Room - Couch Wastepaper A", 43 },
            { "Living Room - Cookie Crumbs under Couch", 44 },
            { "Living Room - Cookie Crumbs on Couch", 45 },
            { "Living Room - Twig A", 46 },
            { "Living Room - Twig B", 47 },
            { "Living Room - Twig C", 48 },
            { "Living Room - Wastepaper above Trashbin A", 54 },
            { "Living Room - Wastepaper above Trashbin B", 55 },
            { "Living Room - Candy Wrapper above Trashbin B", 56 },
            { "Living Room - Candy Wrapper by Jenny A", 57 },
            { "Living Room - Couch Candy Wrapper", 58 },
            { "Living Room - Candy Wrapper by Jenny B", 59 },
            { "Living Room - Candy Wrapper on Book Stack", 60 },
            { "Living Room - Armchair Candy Wrapper B", 61 },
            { "Living Room - Armchair Candy Wrapper A", 62 },
            { "Living Room - Cupholder Candy Wrapper", 63 },
            { "Living Room - Couch Candy Bag", 64 },
            { "Living Room - Table Cookie Box A", 65 },
            { "Living Room - Table Cookie Box B", 66 },
            { "Living Room - Toy receipt on coach", 221 },
            { "Kitchen - Mug Location", 67 },
            { "Kitchen - Spoon Location", 68 },
            { "Kitchen - Wastepaper by Foyer Door", 69 },
            { "Kitchen - Wastepaper under Counter", 70 },
            { "Kitchen - Cookie Crumbs by Tao's Bowl", 71 },
            { "Kitchen - Cookie Crumbs by Spoon", 72 },
            { "Kitchen - Cookie Crumbs on Kitchen Table", 73 },
            { "Kitchen - Cookie Crumbs next to Fridge on Counter", 74 },
            { "Kitchen - Twig A", 79 },
            { "Kitchen - Twig B", 80 },
            { "Kitchen - Twig C", 81 },
            { "Kitchen - Dog Tags Location", 82 },
            { "Kitchen - Bandage Location", 83 },
            { "Kitchen - Frog Ring (Table)", 84 },
            { "Kitchen - Pink Soda Can", 85 },
            { "Kitchen - Purple Soda Can", 86 },
            { "Kitchen - Table Candy Wrapper A", 87 },
            { "Kitchen - Table Candy Wrapper B", 88 },
            { "Kitchen - Table Candy Bag", 89 },
            { "Kitchen - Cookie Box by Spoon A", 90 },
            { "Kitchen - Cookie Box by Spoon B", 91 },
            { "Kitchen - Wastepaper on Shelf by Toaster", 222 },
            { "Kitchen - Cookie Crumbs under Counter", 245 },
            { "Sink Drain - Frog Ring", 105 },
            { "Foyer - Free Rangers Photo", 107 },
            { "Foyer - Waterfall Frog Ring", 108 },
            { "Foyer - Red Block", 109 },
            { "Foyer - Wastepaper On Stairs", 223 },
            { "Foyer - Candy Wrapper by Entrance", 224 },
            { "Foyer - Candy Wrapper by Jenny Photo A", 225 },
            { "Foyer - Candy Wrapper by Jenny Photo B", 226 },
            { "Foyer - Soda Can on Shelf", 227 },
            { "Foyer - Candy Wrapper by Spaceship Shelf A", 228 },
            { "Foyer - Candy Wrapper by Spaceship Shelf B", 229 },
            { "Foyer - Candy Wrapper on Sofa A", 230 },
            { "Foyer - Candy Wrapper on Sofa B", 231 },
            { "Foyer - Candy Wrapper on Floor by Door", 232 },
            { "Foyer - Upstairs Wastepaper by Parents' Door", 233 },
            { "Foyer - Upstairs Wastepaper by stairs", 234 },
            { "Foyer - Upstairs Cookie Crumbs by stairs", 235 },
            { "Foyer - Upstairs Candy Wrapper by Parents' Door", 236 },
            { "Foyer - Upstairs Candy Wrapper by Jenny's Door A", 237 },
            { "Foyer - Upstairs Candy Wrapper by Jenny's Door B", 238 },
            { "Foyer - Candy Wrapper on Sofa C", 240 },
            { "Foyer - Candy Wrapper on Lower Shelf by Entrance", 241 },
            { "Foyer - Candy Wrapper by Spaceship Shelf C", 242 },
            { "Foyer - Candy Wrapper on Stairs", 243 },
            { "Foyer - Upstairs Soda Can", 244 },
            { "Basement - Giga Battery", 110 },
            { "Basement - Giga Charger", 111 },
            { "Basement - Wine Bottle A", 112 },
            { "Basement - Wine Bottle B", 113 },
            { "Basement - Wastepaper below Dresser", 114 },
            { "Basement - Wastepaper below Stairs", 115 },
            { "Basement - Wastepaper on Stairs", 116 },
            { "Basement - Wastepaper on Shelf", 117 },
            { "Basement - Broken Bottle Bottom", 118 },
            { "Basement - Broken Bottle Top", 119 },
            { "Basement - Gunpowder", 120 },
            { "Basement - Frog Ring", 121 },
            { "Basement - Purple Can", 122 },
            { "Basement - Cabinet Trash A", 123 },
            { "Basement - Trash On Stairs", 124 },
            { "Backyard - Twig by Glass Door", 131 },
            { "Backyard - Twig by Fence", 132 },
            { "Backyard - Twig under Tree", 133 },
            { "Backyard - Twig under Awning", 134 },
            { "Backyard - Frog Ring", 139 },
            { "Backyard - White Block", 145 },
            { "Jenny's Room - AA Battery", 146 },
            { "Jenny's Room - D Battery", 152 },
            { "Jenny's Room - C Battery", 153 },
            { "Jenny's Room - Wastepaper by Trashcan", 154 },
            { "Jenny's Room - Wastepaper by Piano", 155 },
            { "Jenny's Room - Wastepaper under Dresser", 156 },
            { "Jenny's Room - Wastepaper under Bed A", 157 },
            { "Jenny's Room - Wastepaper under Bed B", 158 },
            { "Jenny's Room - Wastepaper under Bed C", 159 },
            { "Jenny's Room - Wastepaper under Bed D", 160 },
            { "Jenny's Room - Wastepaper by Crayon Box", 161 },
            { "Jenny's Room - Red Shoe", 162 },
            { "Jenny's Room - Frog Ring", 163 },
            { "Jenny's Room - Squirter", 164 },
            { "Jenny's Room - Snorkel", 165 },
            { "Jenny's Room - Cookie Crumbs under Bed A", 166 },
            { "Jenny's Room - Cookie Crumbs under Bed B", 167 },
            { "Jenny's Room - Cookie Crumbs under Bed C", 168 },
            { "Jenny's Room - Cookie Crumbs under Bed D", 169 },
            { "Jenny's Room - Cookie Crumbs by Chair", 170 },
            { "Jenny's Room - Cookie Crumbs on Desk A", 171 },
            { "Jenny's Room - Cookie Crumbs B", 172 },
            { "Jenny's Room - Candy Wrapper below Bed A", 173 },
            { "Jenny's Room - Candy Wrapper below Bed B", 174 },
            { "Jenny's Room - Candy Wrapper below Bed C", 175 },
            { "Jenny's Room - Candy Wrapper on Bed A", 176 },
            { "Jenny's Room - Candy Wrapper on Bed B", 177 },
            { "Jenny's Room - Candy Wrapper by TV", 178 },
            { "Jenny's Room - Candy Wrapper by Crayon Box A", 179 },
            { "Jenny's Room - Candy Wrapper by Crayon Box B", 180 },
            { "Jenny's Room - Candy Bag under Bed", 181 },
            { "Jenny's Room - Candy Bag on Bed", 182 },
            { "Jenny's Room - Cookie Box under Bed A", 183 },
            { "Jenny's Room - Cookie Box under Bed B", 184 },
            { "Jenny's Room - Cookie Box on Desk", 185 },
            { "Jenny's Room - Orange Can", 186 },
            { "Jenny's Room - Purple Can", 187 },
            { "Jenny's Room - Red Crayon", 188 },
            { "Jenny's Room - Yellow Crayon", 189 },
            { "Jenny's Room - Green Crayon", 190 },
            { "Jenny's Room - Purple Crayon", 191 },
            { "Jenny's Room - Green Block", 192 },
            { "Jenny's Room - Wastepaper under Bed E", 239 },
            { "Bedroom - Dinahs Teeth", 198 },
            { "Bedroom - Ticket Stub", 200 },
            { "Bedroom - Passed Out Frog", 201 },
            { "Bedroom - Wastepaper by Bills B", 202 },
            { "Bedroom - Wastepaper by Bills A", 203 },
            { "Bedroom - Wastepaper under Bed", 204 },
            { "Bedroom - Wastepaper under Vanity", 205 },
            { "Bedroom - Wastepaper on Vanity", 206 },
            { "Bedroom - Wastepaper on Bed", 207 },
            { "Bedroom - Wastepaper by Dinahs Place A", 208 },
            { "Bedroom - Wastepaper by Dinahs Place B", 209 },
            { "Bedroom - Cookie Crumbs on Toybox", 210 },
            { "Bedroom - Vanity Candy Wrapper A", 211 },
            { "Bedroom - Vanity Candy Wrapper B", 212 },
            { "Bedroom - Shelf Candy Wrapper", 213 },
            { "Bedroom - Vanity Candy Bag", 214 },
        };

        public Form1()
        {
            InitializeComponent();

            Theme.Apply(this);       // base bg/font + auto-styles standard controls

            // Wrap the log console in a themed card so it matches the other sections.
            var consoleCard = new SectionPanel { Header = "Output" };
            consoleCard.Bounds = statusDialog.Bounds;   // take over the console's current spot
            statusDialog.Parent = consoleCard;          // move the log into the card
            statusDialog.Dock = DockStyle.Fill;         // fill the card (respects its padding)
            this.Controls.Add(consoleCard);

            PBar.WalkerImage = Image.FromFile(Path.Combine(Directory.GetCurrentDirectory() + @"\Resources\chibiWalk.png"));
            PBar.Height = 76;

            PBar.WalkerFrames = 4;
            PBar.WalkerTicksPerFrame = 4;   // step speed — lower = faster walk

            Theme.StylePrimaryButton(randomizeButton);

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
                if (ofd.ShowDialog(this) == DialogResult.OK)
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

                if (dialog.ShowDialog(this) == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    destinationPath.Text = dialog.SelectedPath;
                }
            }
        }

        // Opens the modal Options dialog, seeding it with the current option
        // state and reading the user's choices back out if they click OK.
        private void optionsButton_Click(object sender, EventArgs e)
        {
            using (var dlg = new OptionsForm
            {
                OpenUpstairs = optOpenUpstairs,
                ChibiVisionOff = optChibiVisionOff,
                RandomizePasswords = optRandomizePasswords
            })
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    optOpenUpstairs = dlg.OpenUpstairs;
                    optChibiVisionOff = dlg.ChibiVisionOff;
                    optRandomizePasswords = dlg.RandomizePasswords;
                }
            }
        }

        private void openAPZip_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "(*.apcr)|*.apcr";
                ofd.RestoreDirectory = true;
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    apZipPath.Text = ofd.FileName;

                    string fileName = Path.GetFileName(ofd.FileName);

                    string fileExtention = Path.GetExtension(ofd.FileName);

                    apData = JsonConvert.DeserializeObject(File.ReadAllText(apZipPath.Text)) as JObject;

                    //Get file name and remove file extention from the path
                    seed.Text = fileName.Remove(fileName.Length - 5, 5);

                    apItemVoice = ((int)apData.SelectToken("favorite_character_voice"));

                    string openUpstairsCheck = apData.SelectToken("open_upstairs").ToString();
                    optOpenUpstairs = (openUpstairsCheck == "1");

                    string chibiVisionString = apData.SelectToken("chibi_vision_off").ToString();
                    optChibiVisionOff = (chibiVisionString == "1");

                    string passwordRandoString = apData.SelectToken("password_rando").ToString();
                    optRandomizePasswords = (passwordRandoString == "1");

                    logicSettings.SelectedItem = "AP Logic";

                    JToken apVersion = apData.SelectToken("Version");

                    string apFileVersion = apVersion[0].ToString() + "." + apVersion[1].ToString() + "." + apVersion[2].ToString();

                    if (apVersion == null)
                    {
                        AppendStatus("[ERROR] Can't Validate AP Version");
                    }

                    if (supportedAPVersion != apFileVersion)
                    {
                        AppendStatus("[ERROR] Supplied AP File Version(" + apFileVersion + ") Is Not Supported In Current Patcher Version(" + supportedAPVersion + ")");
                    }


                }
            }


        }


        private void isoFilePath_TextChanged(object sender, EventArgs e)
        {

        }
        private void settingsLabel_Click(object sender, EventArgs e)
        {

        }
        private void seed_Click(object sender, EventArgs e)
        {

        }

        //This is where the magic happens!
        private async void randomizeButton_Click(object sender, EventArgs e)
        {
            if (!validInput())
                return;

            // --- Read every UI value on the UI thread, BEFORE going to the background. ---
            string seedText = seed.Text;
            string destPath = destinationPath.Text;
            string isoPath = isoFilePath.Text;
            string logicText = logicSettings.Text;
            bool chibiVisionChecked = optChibiVisionOff;
            bool passwordRandoChecked = optRandomizePasswords;
            bool openUpstairsChecked = optOpenUpstairs;

            randomizeButton.Enabled = false;   // prevent re-entry while running
            PBar.Value = 0;

            try
            {
                await Task.Run(() =>
                {
                    // Build an integer seed from the string.
                    int randoSeed = 0;
                    foreach (char c in seedText)
                        randoSeed += (int)c;

                    newLeftFootPassword = 200667;
                    newSuitCasePassword = 2455;

                    newIsoPath = destPath + "\\chibiRando_" + randoSeed + ".iso";

                    File.Copy(isoPath, newIsoPath, true);
                    PBar.Value = 20;

                    initializeStages();
                    PBar.Value = 35;

                    // Any code that handles stage editing / randomization goes here!

                    Stream stageDataStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ChibiRoboRando.itemChecks.json");
                    StreamReader readStageData = new StreamReader(stageDataStream);
                    Stream itemPoolStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ChibiRoboRando.itemPool.json");
                    StreamReader readItemPool = new StreamReader(itemPoolStream);

                    stageData = Newtonsoft.Json.JsonConvert.DeserializeObject<RootObject>(readStageData.ReadToEnd());
                    itemPool = Newtonsoft.Json.JsonConvert.DeserializeObject<ItemPool>(readItemPool.ReadToEnd());
                    PBar.Value = 40;

                    r = new Random(randoSeed);

                    // Performs the randomization based on the settings
                    Dictionary<string, string> newSpoilerLog = shuffleItemsGlitchless();
                    PBar.Value = 50;

                    // --- Settings involving edits to globals ---

                    // Waste Paper Item Updates
                    globals.SelectToken("items[18].name").Replace("Filler AP Item");
                    globals.SelectToken("items[18].description").Replace("Filler AP Item");
                    globals.SelectToken("items[18].flags.chibiVision").Replace(true);

                    // Cookie Crumb Item Updates
                    globals.SelectToken("items[19].name").Replace("AP Item");
                    globals.SelectToken("items[19].description").Replace("AP Item");
                    globals.SelectToken("items[19].flags.chibiVision").Replace(true);

                    // Turns off Chibi Vision for all objects if enabled
                    if (chibiVisionChecked)
                    {
                        for (int i = 0; i < 159; i++)
                        {
                            globals.SelectToken("items[" + i + "].flags.chibiVision").Replace(false);
                        }
                    } else
                    {
                        for (int i = 0; i < 159; i++)
                        {
                            globals.SelectToken("items[" + i + "].flags.chibiVision").Replace(true);
                        }
                    }

                    // Randomizes left foot and suit case passcode, if enabled
                    if (passwordRandoChecked)
                    {
                        newLeftFootPassword = r.Next(100000, 999999);
                        newSuitCasePassword = r.Next(1000, 9999);
                        globals.SelectToken("stats[12].name").Replace("Password: " + newLeftFootPassword);
                        globals.SelectToken("items[124].description").Replace("The number \"" + newLeftFootPassword + "\" is engraved\non the inside.");
                    }

                    // Disable Starting Copter
                    globals.SelectToken("defaultAtcs.copter").Replace(false);

                    // Battery Drain Settings
                    JToken batteryGlobal = globals.SelectToken("batteryGlobals");

                    batteryGlobal.SelectToken("idle").Replace(apData.SelectToken("battery_drain_idle"));
                    batteryGlobal.SelectToken("walk").Replace(apData.SelectToken("battery_drain_walk"));
                    batteryGlobal.SelectToken("jog").Replace(apData.SelectToken("battery_drain_jog"));
                    batteryGlobal.SelectToken("run").Replace(apData.SelectToken("battery_drain_run"));
                    batteryGlobal.SelectToken("slide").Replace(apData.SelectToken("battery_drain_slide"));
                    batteryGlobal.SelectToken("equip").Replace(apData.SelectToken("battery_drain_equip"));
                    batteryGlobal.SelectToken("lift").Replace(apData.SelectToken("battery_drain_lift"));
                    batteryGlobal.SelectToken("drop").Replace(apData.SelectToken("battery_drain_drop"));
                    //batteryGlobal.SelectToken("leticker").Replace(apData.SelectToken("battery_drain_idle"));
                    batteryGlobal.SelectToken("ledgeClimb").Replace(apData.SelectToken("battery_drain_ledge_grab"));
                    batteryGlobal.SelectToken("ledgeSlide").Replace(apData.SelectToken("battery_drain_ledge_slide"));
                    batteryGlobal.SelectToken("ledgeDrop").Replace(apData.SelectToken("battery_drain_ledge_drop"));
                    batteryGlobal.SelectToken("ledgeTeeter").Replace(apData.SelectToken("battery_drain_ledge_teeter"));
                    batteryGlobal.SelectToken("jump").Replace(apData.SelectToken("battery_drain_jump"));
                    batteryGlobal.SelectToken("fall").Replace(apData.SelectToken("battery_drain_fall"));
                    batteryGlobal.SelectToken("ladderGrab").Replace(apData.SelectToken("battery_drain_ladder_grab"));
                    batteryGlobal.SelectToken("ladderAscend").Replace(apData.SelectToken("battery_drain_ladder_ascend"));
                    batteryGlobal.SelectToken("ladderDescend").Replace(apData.SelectToken("battery_drain_ladder_descend"));
                    batteryGlobal.SelectToken("ladderTop").Replace(apData.SelectToken("battery_drain_ladder_top"));
                    batteryGlobal.SelectToken("ladderBottom").Replace(apData.SelectToken("battery_drain_ladder_bottom"));
                    batteryGlobal.SelectToken("ropeGrab").Replace(apData.SelectToken("battery_drain_rope_grab"));
                    batteryGlobal.SelectToken("ropeAscend").Replace(apData.SelectToken("battery_drain_rope_ascend"));
                    batteryGlobal.SelectToken("ropeDescend").Replace(apData.SelectToken("battery_drain_rope_descend"));
                    batteryGlobal.SelectToken("ropeTop").Replace(apData.SelectToken("battery_drain_rope_top"));
                    batteryGlobal.SelectToken("ropeBottom").Replace(apData.SelectToken("battery_drain_rope_bottom"));
                    batteryGlobal.SelectToken("push").Replace(apData.SelectToken("battery_drain_push"));
                    batteryGlobal.SelectToken("copterHover").Replace(apData.SelectToken("battery_drain_copter_hover"));
                    batteryGlobal.SelectToken("copterDescend").Replace(apData.SelectToken("battery_drain_copter_descend"));
                    batteryGlobal.SelectToken("popperShoot").Replace(apData.SelectToken("battery_drain_popper_shoot"));
                    batteryGlobal.SelectToken("popperShootCharged").Replace(apData.SelectToken("battery_drain_pooper_shoot_charge"));
                    batteryGlobal.SelectToken("radarScan").Replace(apData.SelectToken("battery_drain_radar_scan"));
                    batteryGlobal.SelectToken("radarFollow").Replace(apData.SelectToken("battery_drain_radar_follow"));
                    batteryGlobal.SelectToken("brush").Replace(apData.SelectToken("battery_drain_brush"));
                    batteryGlobal.SelectToken("spoon").Replace(apData.SelectToken("battery_drain_spoon"));
                    batteryGlobal.SelectToken("mug").Replace(apData.SelectToken("battery_drain_mug"));
                    batteryGlobal.SelectToken("squirterSuck").Replace(apData.SelectToken("battery_drain_squirter_suck"));
                    batteryGlobal.SelectToken("squirterSpray").Replace(apData.SelectToken("battery_drain_squirter_spray"));



                    // Edits for Open Upstairs setting
                    if (openUpstairsChecked)
                    {
                        JToken latestToken = foyerObj.SelectToken("objects[512]");

                        Stream openUpstairsStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ChibiRoboRando.openUpstairs.json");
                        StreamReader readOpenUpstairs = new StreamReader(openUpstairsStream);
                        latestToken.AddAfterSelf(Newtonsoft.Json.JsonConvert.DeserializeObject(readOpenUpstairs.ReadToEnd()) as JObject);
                    }

                    // Living Room door (rouka_door_l, id 24) is permanently open with no closed pose.
                    // Swap to living_door — the same model used on the Living Room side (stage07 obj 77),
                    // which has correct open/closed animation frames and faces the right direction.
                    foyerObj.SelectToken("objects[?(@.id == 24)].object").Replace("living_door");

                    // Kitchen door (rouka_door_k, id 3) has no working open animation frame (anim 1 is a no-op).
                    // Swap to kitchen_door — same model used in stage01 (obj 49) which has proper anim 0=closed / anim 1=open.
                    //foyerObj.SelectToken("objects[?(@.id == 3)].object").Replace("kitchen_door");

                    // RANDOMIZER: Foyer<->Basement door (rouka_door_e, id 76) - this doorway had no door
                    // of its own in vanilla. rouka_door_e is confirmed cosmetic-only with no warp behind
                    // it (see door_flags.txt), so repurpose it here instead of adding a new object -
                    // adding a brand-new object index (beyond this stage's original 0-512 range) was
                    // tried first and produced a stray empty destination-label box in the overhead
                    // camera view near the Basement map_jump_box that followed the object wherever it
                    // moved; reusing an existing in-range object avoids that entirely.
                    foyerObj.SelectToken("objects[?(@.id == 76)].object").Replace("living_door");
                    foyerObj.SelectToken("objects[?(@.id == 76)].position.x").Replace(-182.56);
                    foyerObj.SelectToken("objects[?(@.id == 76)].position.y").Replace(0.0);
                    foyerObj.SelectToken("objects[?(@.id == 76)].position.z").Replace(-392.99);
                    foyerObj.SelectToken("objects[?(@.id == 76)].rotation.x").Replace(0);
                    foyerObj.SelectToken("objects[?(@.id == 76)].rotation.y").Replace(0);
                    foyerObj.SelectToken("objects[?(@.id == 76)].rotation.z").Replace(0);

                    PBar.Value = 65;

                    // Spoiler log output (note: uses the captured locals, not the controls)
                    using (StreamWriter logOutput = new StreamWriter(File.OpenWrite(destPath + "\\Spoiler Log " + randoSeed + ".txt")))
                    {
                        logOutput.WriteLine("******");
                        logOutput.WriteLine("Seed: " + seedText);
                        logOutput.WriteLine("Mode: " + logicText);
                        logOutput.WriteLine("Left Foot Password:  " + newLeftFootPassword);
                        logOutput.WriteLine("Suit Case Password:  " + newSuitCasePassword);
                        logOutput.WriteLine("Open Upstairs: " + openUpstairsChecked);
                        //logOutput.WriteLine("Charged Battery: " + batteryCharge.Checked);
                        logOutput.WriteLine("Randomize Password: " + passwordRandoChecked);
                        logOutput.WriteLine("Chibi-Vision Off: " + chibiVisionChecked);
                        logOutput.WriteLine("******\n");

                        foreach (string key in newSpoilerLog.Keys)
                        {
                            logOutput.WriteLine(key + ": " + newSpoilerLog[key]);
                        }
                    }

                    generateAntiRespawnSubroutines();
                    reimportStages();
                    PBar.Value = 100;

                    AppendStatus("\nISO Rebuilding Complete :)");
                });
            }
            catch (Exception ex)
            {
                AppendStatus("\nError: " + ex.Message);
                PBar.Value = 0;
            }
            finally
            {
                randomizeButton.Enabled = true;
            }
        }

        // Add this helper to the same class. It lets background-thread code append to
        // the status console safely by marshaling the update back to the UI thread.
        private void AppendStatus(string text)
        {
            if (statusDialog.InvokeRequired)
                statusDialog.Invoke((Action)(() => statusDialog.Text += text));
            else
                statusDialog.Text += text;
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

            //XmlDocument doc = new XmlDocument();
            //doc.Load(Directory.GetCurrentDirectory() + @"\Resources\messages.xml");
            //doc.Save(Directory.GetCurrentDirectory() + @"\messages.xml");

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

                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                cmd.StartInfo.UseShellExecute = false;

                ProcessStartInfo unplugCommandInfo = new ProcessStartInfo();
                unplugCommandInfo.CreateNoWindow = true;
                unplugCommandInfo.WindowStyle = ProcessWindowStyle.Hidden;
                unplugCommandInfo.UseShellExecute = false;

                // Quote the exe path so cmd.exe treats it as a single token even with spaces.
                string exePath = Path.Combine(Directory.GetCurrentDirectory(), "unplug.exe");
                string fullCommand = "\"" + exePath + "\" " + command;

                unplugCommandInfo.WorkingDirectory = @"C:\Windows\System32";
                unplugCommandInfo.FileName = "cmd.exe";
                unplugCommandInfo.Verb = "runas";
                // Outer quotes wrap the whole command; cmd /C strips this outer pair,
                // leaving the inner-quoted path intact.
                unplugCommandInfo.Arguments = "/C \"" + fullCommand + "\"";
                unplugCommandInfo.WindowStyle = ProcessWindowStyle.Minimized;
                unplugCommandInfo.RedirectStandardOutput = true;

                cmd.StartInfo = unplugCommandInfo;
                cmd.Start();

                StreamReader sr = cmd.StandardOutput;
                string test = sr.ReadToEnd();   // read fully BEFORE WaitForExit to avoid deadlock
                cmd.WaitForExit();
            }
        }


        private bool validInput()
        {
            //Input Validation
            string isoValidation = isoFilePath.Text;
            isoValidation = isoValidation.Substring(isoValidation.Length - 4);

            if (isoValidation.ToLower() == ".iso")
            {
                AppendStatus("Validated ISO");

            }
            else
            {
                AppendStatus("[ERROR] Invalid file path to Chibi-Robo ISO");
                return false;
            }

            if (destinationPath.Text != "<- Set destination path")
            {
                AppendStatus("\nValidated file path");
            }
            else
            {
                AppendStatus("\n[ERROR] Invalid destination file path");
                return false;
            }

            if (apZipPath.Text != "<- Set Archipelago Data To Enable Integration")
            {
                AppendStatus("\nValidated AP Path");
            }
            else
            {
                AppendStatus("\n[ERROR] Invalid AP file path");
                return false;
            }

            JToken apVersion = apData.SelectToken("Version");

            string apFileVersion = apVersion[0].ToString() + "." + apVersion[1].ToString() + "." + apVersion[2].ToString();

            if (apVersion == null)
            {
                AppendStatus("[ERROR] Can't Validate AP Version");
            }

            if (supportedAPVersion != apFileVersion)
            {
                AppendStatus("[ERROR] Supplied AP File Version(" + apFileVersion + ") Is Not Supported In Current Patcher Version(" + supportedAPVersion + ")");
            }

            if (logicSettings.SelectedItem != null)
            {
                AppendStatus("\nValidation complete");
            }
            else
            {
                AppendStatus("\n[ERROR] Please select a game mode from the dropdown menu.");
                return false;
            }

            return true;
        }

        //Picks locations for the items, puts them into the appropriate locations, and then returns the spoiler log
        private Dictionary<string, string> shuffleItemsGlitchless()
        {
            //*** SETUP ***
            stageAntiRespawnLocs = new Dictionary<string, List<(int code, int objID)>>();

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

            for (int i = 0; i < occupiedChecks.Count; i++)
                occupiedChecks[i] = false;


            //Flattens out shop checks with null values
            //shopObj.SelectToken("items[3].item").Replace(null);
            shopObj.SelectToken("items[4].item").Replace(null);
            shopObj.SelectToken("items[5].item").Replace(null);
            //shopObj.SelectToken("items[6].item").Replace(null);
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

                // Chibi House
                File.Copy(Directory.GetCurrentDirectory() + @"\Resources\stage05.us", Directory.GetCurrentDirectory() + @"\stage05_Edited.us", true);

                // Living Room
                File.Copy(Directory.GetCurrentDirectory() + @"\Resources\stage07.us", Directory.GetCurrentDirectory() + @"\stage07_Edited.us", true);

                // Kitchen
                File.Copy(Directory.GetCurrentDirectory() + @"\Resources\stage01.us", Directory.GetCurrentDirectory() + @"\stage01_Edited.us", true);

                // Sink Drain
                //File.Copy(Directory.GetCurrentDirectory() + @"\Resources\stage11.us", Directory.GetCurrentDirectory() + @"\stage11_Edited.us", true);

                // Foyer
                File.Copy(Directory.GetCurrentDirectory() + @"\Resources\stage02.us", Directory.GetCurrentDirectory() + @"\stage02_Edited.us", true);

                // Basement
                File.Copy(Directory.GetCurrentDirectory() + @"\Resources\stage03.us", Directory.GetCurrentDirectory() + @"\stage03_Edited.us", true);

                // Backyard
                File.Copy(Directory.GetCurrentDirectory() + @"\Resources\stage09.us", Directory.GetCurrentDirectory() + @"\stage09_Edited.us", true);

                // Jenny's Room
                File.Copy(Directory.GetCurrentDirectory() + @"\Resources\stage04.us", Directory.GetCurrentDirectory() + @"\stage04_Edited.us", true);

                // Bedroom
                File.Copy(Directory.GetCurrentDirectory() + @"\Resources\stage06.us", Directory.GetCurrentDirectory() + @"\stage06_Edited.us", true);

                // Bedroom (Old)
                File.Copy(Directory.GetCurrentDirectory() + @"\Resources\stage18.us", Directory.GetCurrentDirectory() + @"\stage18_Edited.us", true);

                //int shopId = 0;

                // "Name" is this ROM's own player/slot name - every location's "player" field is
                // compared against it below to tell apart a self-found item from one placed here
                // for a different player in the multiworld.
                string myPlayerName = apData.SelectToken("Name")?.ToString();

                // Loop through each location josin
                foreach (KeyValuePair<string, JToken> location in locations)
                {

                    string locationName = location.Key;
                    string name = location.Value.SelectToken("name").ToString();
                    string objectName = location.Value.SelectToken("object").ToString();
                    int locationID = location.Value.SelectToken("location_id")?.ToObject<int?>() ?? 0;

                    string playerID = location.Value.SelectToken("player").ToString();

                    // RANDOMIZER: trigger the Pan Drop Trap animation immediately on pickup when
                    // it's specifically this player's own trap (not one placed here for someone
                    // else in the multiworld, which is delivered to them separately over the
                    // network instead - see project_pan_drop_trap memory).
                    bool isSelfPanDropTrap = name == "Pan Drop Trap" && playerID == myPlayerName;

                    var roomID = get_room_id_by_name(location.Key);

                    var roomObject = get_room_object_id_by_name(location.Key);

                    // Add the item as the multiworld item.
                    // Hopefully there is a way to add a
                    // new item down the road 
                    if (objectName.Contains("archipelago_item"))
                    {

                        string classification = location.Value.SelectToken("classification").ToString();

                        if (classification == "progression" || classification == "usefull" || classification == "trap" || name == "Pan Drop Trap")
                        {
                            objectName = "item_cookie_kakera";
                        }
                        else
                        {
                            objectName = "item_kami_kuzu";
                        }

                    }

                    if (locationName == "Living Room - Drake Redcrest Suit")
                    {
                        updateDrakeSuitLoc(Directory.GetCurrentDirectory() + @"\stage07_Edited.us", objectName, playerID, name);
                        spoilerLog.Add(locationName, name);
                        continue;

                    }
                    else if (locationName == "Backyard - Frog Suit")
                    {
                        updateFrogSuitLoc(Directory.GetCurrentDirectory() + @"\stage09_Edited.us", objectName, playerID, name);
                        spoilerLog.Add(locationName, name);
                        continue;

                    }
                    else if (locationName == "Chibi House - Trauma Suit")
                    {
                        updateTraumaSuitLoc(Directory.GetCurrentDirectory() + @"\stage05_Edited.us", objectName, playerID, name);
                        spoilerLog.Add(locationName, name);
                        continue;

                    }
                    else if (locationName == "Chibi House - Ghost Suit")
                    {
                        updateGhostSuitLoc(Directory.GetCurrentDirectory() + @"\stage05_Edited.us", objectName, playerID, name);
                        spoilerLog.Add(locationName, name);
                        continue;
                    }
                    else if (locationName == "Bedroom - Pajama Suit")
                    {
                        updatePajamaSuitLoc(Directory.GetCurrentDirectory() + @"\stage06_Edited.us", objectName, playerID, name);
                        updatePajamaSuitKitchenLoc(Directory.GetCurrentDirectory() + @"\stage01_Edited.us", objectName, playerID, name);
                        spoilerLog.Add(locationName, name);
                        continue;
                    }
                    else if (locationName == "Foyer - Tao Suit")
                    {
                        updateToaSuitLoc(Directory.GetCurrentDirectory() + @"\stage02_Edited.us", objectName, playerID, name);
                        spoilerLog.Add(locationName, name);
                        continue;
                    }

                    // Resolve the AP location code for this location regardless of what item is placed here
                    int apCode = apLocationCodes.ContainsKey(locationName) ? apLocationCodes[locationName] : -1;

                    // Collect ALL AP locations for anti-respawn (skip Shop roomID=8 and Sink Drain roomID=2)
                    if (apCode >= 0 && roomID != 8 && roomID != 2)
                    {
                        string stageFile = getStageFileNameForRoom(roomID);
                        if (stageFile != null)
                        {
                            if (!stageAntiRespawnLocs.ContainsKey(stageFile))
                                stageAntiRespawnLocs[stageFile] = new List<(int code, int objID)>();
                            stageAntiRespawnLocs[stageFile].Add((apCode, locationID));
                        }
                    }

                    // if location has an AP placeholder item, add an in-game message of what the player picked up
                    if (objectName.Contains("item_kami_kuzu") || objectName.Contains("item_cookie_kakera") || objectName.Contains("item_cos_obake") || objectName.Contains("item_capsule_"))
                    {
                        roomCheckForInGameMessages(roomID, locationID, playerID, name, false, 0, apCode, isSelfPanDropTrap);
                    }
                    else if (apCode >= 0 && roomID != 8 && roomID != 2)
                    {
                        // Native Chibi-Robo item placed here: add a flag-only interact handler so anti-respawn works
                        string stageFile = getStageFileNameForRoom(roomID);
                        if (stageFile != null)
                            addFlagOnlyHandler(Directory.GetCurrentDirectory() + @"\" + stageFile, locationID, apCode);
                    }

                    // shop items are not the same as normal items
                    if (roomID != 8)
                    {

                        // Add the object name into the location
                        roomObject.SelectToken("objects[" + locationID + "].object").Replace(objectName);


                        switch (objectName)
                        {
                            case "item_chip_53":
                            case "item_ticket":
                            case "item_chip_54":
                            case "item_hocyouki":
                            case "item_receipt":
                            case "item_army_photo":
                            case "item_spoon":
                            case "item_kure_1":
                            case "item_kure_3":
                            case "item_kure_4":
                            case "item_kure_5":
                            case "cb_radar":
                            case "chibi_battery":
                            case "cb_cannon_lv_2":
                            case "cb_propeller_lv_2":
                            case "item_tug":
                            case "item_pajama_kiji":
                            case "item_pajama_kiji_2":
                            case "item_pajama_kiji_3":
                                roomObject.SelectToken("objects[" + locationID + "].flags[0]").AddAfterSelf("flash");
                                roomObject.SelectToken("objects[" + locationID + "].flags[0]").AddAfterSelf("cull");
                                roomObject.SelectToken("objects[" + locationID + "].flags[0]").AddAfterSelf("lift");
                                roomObject.SelectToken("objects[" + locationID + "].flags[0]").AddAfterSelf("interact");

                                var posY = roomObject.SelectToken("objects[" + locationID + "].position.y");
                                posY = ((float)posY) + 2.0f;

                                roomObject.SelectToken("objects[" + locationID + "].position.y").Replace(posY);

                                roomObject.SelectToken("objects[" + locationID + "].rotation.x").Replace(0);
                                roomObject.SelectToken("objects[" + locationID + "].rotation.y").Replace(0);
                                roomObject.SelectToken("objects[" + locationID + "].rotation.z").Replace(0);

                                break;
                            default:
                                roomObject.SelectToken("objects[" + locationID + "].flags[0]").AddAfterSelf("flash");
                                roomObject.SelectToken("objects[" + locationID + "].flags[0]").AddAfterSelf("cull");
                                roomObject.SelectToken("objects[" + locationID + "].flags[0]").AddAfterSelf("lift");
                                roomObject.SelectToken("objects[" + locationID + "].flags[0]").AddAfterSelf("interact");

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

                        //shopId++;

                    }

                    // Fix some locations items positions

                    // Bedroom
                    if (roomID == 7)
                    {
                        // Bedroom - Wastepaper on Bed
                        if (locationID == 286)
                        {
                            var posY = roomObject.SelectToken("objects[" + locationID + "].position.y");
                            posY = ((float)posY) + 2.0f;

                            roomObject.SelectToken("objects[" + locationID + "].position.y").Replace(posY);
                        }

                    }

                    // Living Room
                    if (roomID == 0)
                    {
                        // Living Room - Frog Ring (Shelf)
                        if (locationID == 21)
                        {
                            var posY = roomObject.SelectToken("objects[" + locationID + "].position.y");
                            posY = ((float)posY) + 0.5f;

                            roomObject.SelectToken("objects[" + locationID + "].position.y").Replace(posY);

                            roomObject.SelectToken("objects[" + locationID + "].rotation.y").Replace(0);
                        }

                    }

                    // Kitchen
                    if (roomID == 1)
                    {
                        // Kitchen - Wastepaper on Shelf by Toaster
                        if (locationID == 154)
                        {
                            var posZ = roomObject.SelectToken("objects[" + locationID + "].position.z");
                            posZ = ((float)posZ) + 3.0f;

                            roomObject.SelectToken("objects[" + locationID + "].position.z").Replace(posZ);

                        }

                    }

                    // Foyer
                    if (roomID == 3)
                    {
                        // Foyer - Wastepaper On Stairs
                        if (locationID == 268)
                        {
                            var posY = roomObject.SelectToken("objects[" + locationID + "].position.y");
                            posY = ((float)posY) + 1.0f;

                            roomObject.SelectToken("objects[" + locationID + "].position.y").Replace(posY);

                        }

                    }


                    roomObject.SelectToken("objects[" + locationID + "].spawnFlag").Replace(apSpawnFlag);

                    spoilerLog.Add(locationName, name);

                    apSpawnFlag++;

                    if (apSpawnFlag == 134)
                    {
                        apSpawnFlag = 1;
                    }

                }

            }

            return spoilerLog;
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

            PBar.Value = 70;

            // Import custom scritping First
            AppendStatus("\nPatching Rooms");

            // Birthday Party Into
            runUnplugCommand("script assemble --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\Resources\stage14.us" + "\"");

            // Living Room
            runUnplugCommand("script assemble --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\stage07_Edited.us" + "\"");

            // Kitchen
            runUnplugCommand("script assemble --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\stage01_Edited.us" + "\"");

            // Sink Drain
            //runUnplugCommand("script assemble --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\stage11_Edited.us" + "\"");

            // Foyer
            runUnplugCommand("script assemble --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\stage02_Edited.us" + "\"");

            // Basement
            updateLeftFootPassword(Directory.GetCurrentDirectory() + @"\stage03_Edited.us", newLeftFootPassword);
            runUnplugCommand("script assemble --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\stage03_Edited.us" + "\"");

            // Backyard
            runUnplugCommand("script assemble --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\stage09_Edited.us" + "\"");

            // Jenny's Room
            runUnplugCommand("script assemble --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\stage04_Edited.us" + "\"");

            // Bedroom
            updateSuitCasePassword(Directory.GetCurrentDirectory() + @"\stage06_Edited.us", newSuitCasePassword);
            runUnplugCommand("script assemble --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\stage06_Edited.us" + "\"");

            // Bedroom (Old)
            updateSuitCasePasswordText(Directory.GetCurrentDirectory() + @"\stage18_Edited.us", newSuitCasePassword);
            runUnplugCommand("script assemble --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\stage18_Edited.us" + "\"");

            //Chibi House
            runUnplugCommand("script assemble --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\stage05_Edited.us" + "\"");

            // Update Messages
            //runUnplugCommand("messages import --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\Resources\messages.xml" + "\"");

            // Globals
            runUnplugCommand("script assemble --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\Resources\globals.us" + "\"");
            runUnplugCommand("globals import --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\globals.json" + "\"");
            AppendStatus("\nUpdating Globals");

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

            PBar.Value = 80;

            runUnplugCommand("stage import --iso \"" + newIsoPath + "\" stage07 \"" + Directory.GetCurrentDirectory() + @"\stage07.json" + "\"");
            runUnplugCommand("stage import --iso \"" + newIsoPath + "\" stage09 \"" + Directory.GetCurrentDirectory() + @"\stage09.json" + "\"");
            runUnplugCommand("stage import --iso \"" + newIsoPath + "\" stage11 \"" + Directory.GetCurrentDirectory() + @"\stage11.json" + "\"");

            runUnplugCommand("shop import --iso \"" + newIsoPath + "\" \"" + Directory.GetCurrentDirectory() + @"\shop.json" + "\"");


            runUnplugCommand(" --iso \"" + newIsoPath + "\" iso set maker EveryDaySimpleDev-" + seed.Text);
            runUnplugCommand(" --iso \"" + newIsoPath + "\" iso set name \"Chibi-Robo!" + seed.Text + " \"");

            AppendStatus("\nImporting JSON data");

            if (apData != null)
            {
                runUnplugCommand(" --iso \"" + newIsoPath + "\" qp replace tpl\\title_en.tpl \"" + Directory.GetCurrentDirectory() + @"\Resources\title_en_ap.tpl" + "\"");

                runUnplugCommand(" --iso \"" + newIsoPath + "\" qp replace tpl\\misc.tpl \"" + Directory.GetCurrentDirectory() + @"\Resources\misc_edited.tpl" + "\"");

                runUnplugCommand(" --iso \"" + newIsoPath + "\" qp replace Item\\kami_kuzu.dat \"" + Directory.GetCurrentDirectory() + @"\Resources\kami_kuzu_edited.dat" + "\"");

                runUnplugCommand(" --iso \"" + newIsoPath + "\" qp replace Item\\cookie_kakera.dat \"" + Directory.GetCurrentDirectory() + @"\Resources\cookie_kakera_edited.dat" + "\"");
            }
            else
            {
                runUnplugCommand(" --iso \"" + newIsoPath + "\" qp replace tpl\\title_en.tpl \"" + Directory.GetCurrentDirectory() + @"\Resources\title_en_rando.tpl" + "\"");
            }

            PBar.Value = 85;

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

            string roomTempName = location.Substring(0, location.IndexOf("-") + 1).Trim(new Char[] { ' ', '-' });

            if (roomTempName == "Living Room")
            {
                return 0;
            }
            else if (roomTempName == "Kitchen")
            {
                if (lastRoom != "Kitchen")
                {
                    lastRoom = roomTempName;
                    apSpawnFlag = 1;

                }

                return 1;
            }
            else if (roomTempName == "Sink Drain")
            {
                if (lastRoom != "Sink Drain")
                {
                    lastRoom = roomTempName;
                    apSpawnFlag = 1;

                }

                return 2;
            }
            else if (roomTempName == "Foyer")
            {
                if (lastRoom != "Foyer")
                {
                    lastRoom = roomTempName;
                    apSpawnFlag = 1;

                }

                return 3;
            }
            else if (roomTempName == "Basement")
            {
                if (lastRoom != "Basement")
                {
                    lastRoom = roomTempName;
                    apSpawnFlag = 1;

                }

                return 4;
            }
            else if (roomTempName == "Backyard")
            {
                if (lastRoom != "Backyard")
                {
                    lastRoom = roomTempName;
                    apSpawnFlag = 1;

                }

                return 5;
            }
            else if (roomTempName == "Jenny's Room")
            {
                if (lastRoom != "Jenny's Room")
                {
                    lastRoom = roomTempName;
                    apSpawnFlag = 1;

                }

                return 6;
            }
            else if (roomTempName == "Bedroom")
            {
                if (lastRoom != "Bedroom")
                {
                    lastRoom = roomTempName;
                    apSpawnFlag = 1;

                }

                return 7;
            }
            else if (roomTempName == "Chibi House")
            {
                if (lastRoom != "Chibi House")
                {
                    lastRoom = roomTempName;
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void walkingBatteryDrain_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void batteryCharge_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void itemLocationsButton_Click(object sender, EventArgs e)
        {

        }

        private void roomCheckForInGameMessages(int roomID, int objectID, string player, string newObjectName, bool atc = false, int atcID = 0, int locationCode = -1, bool triggerPanDropAnim = false)
        {
            if (roomID == 0) // Living Room
            {

                int[] skipLocations = { };

                if (!skipLocations.Contains(objectID))
                {
                    if (atc == false)
                    {
                        addInGameMessages(Directory.GetCurrentDirectory() + @"\stage07_Edited.us", objectID, player, newObjectName, locationCode, triggerPanDropAnim);

                    }
                    else
                    {
                        enableATCToolPickup(Directory.GetCurrentDirectory() + @"\stage07_Edited.us", objectID, player, newObjectName, atcID);
                    }
                }

            }
            else if (roomID == 1) // Kitchen
            {

                int[] skipLocations = { 36, 37 };

                if (!skipLocations.Contains(objectID))
                {
                    if (atc == false)
                    {
                        addInGameMessages(Directory.GetCurrentDirectory() + @"\stage01_Edited.us", objectID, player, newObjectName, locationCode, triggerPanDropAnim);
                    }
                    else
                    {
                        enableATCToolPickup(Directory.GetCurrentDirectory() + @"\stage01_Edited.us", objectID, player, newObjectName, atcID);
                    }
                }
            }
            else if (roomID == 2) // Sink Drain
            {

                int[] skipLocations = { };

                if (!skipLocations.Contains(objectID))
                {
                    if (atc == false)
                    {
                        //addInGameMessages(Directory.GetCurrentDirectory() + @"\stage11_Edited.us", objectID, player, newObjectName, locationCode);
                    }
                    else
                    {
                        //enableATCToolPickup(Directory.GetCurrentDirectory() + @"\stage11_Edited.us", objectID, player, newObjectName, atcID);
                    }
                }
            }
            else if (roomID == 3) // Foyer
            {

                int[] skipLocations = { };

                if (!skipLocations.Contains(objectID))
                {
                    if (atc == false)
                    {
                        addInGameMessages(Directory.GetCurrentDirectory() + @"\stage02_Edited.us", objectID, player, newObjectName, locationCode, triggerPanDropAnim);
                    }
                    else
                    {
                        enableATCToolPickup(Directory.GetCurrentDirectory() + @"\stage02_Edited.us", objectID, player, newObjectName, atcID);
                    }
                }
            }
            else if (roomID == 4) // Basement
            {
                int[] skipLocations = { 6, 121 };

                if (!skipLocations.Contains(objectID))
                {
                    if (atc == false)
                    {
                        addInGameMessages(Directory.GetCurrentDirectory() + @"\stage03_Edited.us", objectID, player, newObjectName, locationCode, triggerPanDropAnim);
                    }
                    else
                    {
                        enableATCToolPickup(Directory.GetCurrentDirectory() + @"\stage03_Edited.us", objectID, player, newObjectName, atcID);
                    }
                }
            }
            else if (roomID == 5) // Backyard
            {
                int[] skipLocations = { };

                if (!skipLocations.Contains(objectID))
                {
                    if (atc == false)
                    {
                        addInGameMessages(Directory.GetCurrentDirectory() + @"\stage09_Edited.us", objectID, player, newObjectName, locationCode, triggerPanDropAnim);
                    }
                    else
                    {
                        enableATCToolPickup(Directory.GetCurrentDirectory() + @"\stage09_Edited.us", objectID, player, newObjectName, atcID);
                    }
                }
            }
            else if (roomID == 6) // Jenny's Room
            {
                int[] skipLocations = { };

                if (!skipLocations.Contains(objectID))
                {
                    if (atc == false)
                    {
                        addInGameMessages(Directory.GetCurrentDirectory() + @"\stage04_Edited.us", objectID, player, newObjectName, locationCode, triggerPanDropAnim);
                    }
                    else
                    {
                        enableATCToolPickup(Directory.GetCurrentDirectory() + @"\stage04_Edited.us", objectID, player, newObjectName, atcID);
                    }
                }
            }
            else if (roomID == 7) // Bedroom
            {
                int[] skipLocations = { };

                if (!skipLocations.Contains(objectID))
                {
                    if (atc == false)
                    {
                        addInGameMessages(Directory.GetCurrentDirectory() + @"\stage06_Edited.us", objectID, player, newObjectName, locationCode, triggerPanDropAnim);
                    }
                    else
                    {
                        enableATCToolPickup(Directory.GetCurrentDirectory() + @"\stage06_Edited.us", objectID, player, newObjectName, atcID);
                    }
                }
            }
        }

        private string getStageFileNameForRoom(int roomID)
        {
            switch (roomID)
            {
                case 0: return "stage07_Edited.us";
                case 1: return "stage01_Edited.us";
                case 3: return "stage02_Edited.us";
                case 4: return "stage03_Edited.us";
                case 5: return "stage09_Edited.us";
                case 6: return "stage04_Edited.us";
                case 7: return "stage06_Edited.us";
                default: return null;
            }
        }
        private string objectNameToGameID(string objectName)
        {
            int objectID;

            switch (objectName)
            {
                case "item_frog_ring":
                    objectID = 0;
                    break;
                case "item_deka_denchi":
                    objectID = 8;
                    break;
                case "item_snack_bone":
                    objectID = 14;
                    break;
                case "item_brush":
                    objectID = 16;
                    break;
                case "capsule_17":
                    objectID = 17;
                    break;
                case "item_kami_kuzu":
                    objectID = 18;
                    break;
                case "item_cookie_kakera":
                    objectID = 19;
                    break;
                case "item_spoon":
                    objectID = 21;
                    break;
                case "item_mag_cup":
                    objectID = 22;
                    break;
                case "item_capsule_24":
                    objectID = 24;
                    break;
                case "item_capsule_25":
                    objectID = 25;
                    break;
                case "item_capsule_26":
                    objectID = 26;
                    break;
                case "capsule_27":
                    objectID = 27;
                    break;
                case "capsule_28":
                    objectID = 28;
                    break;
                case "capsule_29":
                    objectID = 29;
                    break;
                case "item_capsule_30":
                    objectID = 30;
                    break;
                case "super_chibi_robo_suit":
                    objectID = 31;
                    break;
                case "item_capsule_32":
                    objectID = 32;
                    break;
                case "item_cos_obake":
                    objectID = 34;
                    break;
                case "cookie":
                    objectID = 45;
                    break;
                case "item_chibi_house_denti_2":
                    objectID = 48;
                    break;
                case "popper_trash":
                    objectID = 49;
                    break;
                case "empty_bottle":
                    objectID = 50;
                    break;
                case "broken_bottle_a":
                    objectID = 51;
                    break;
                case "broken_bottle_b":
                    objectID = 52;
                    break;
                case "item_chip_53":
                    objectID = 53;
                    break;
                case "item_chip_54":
                    objectID = 54;
                    break;
                case "item_receipt":
                    objectID = 55;
                    break;
                case "item_tyuusyaki":
                    objectID = 56;
                    break;
                case "moms_letter":
                    objectID = 57;
                    break;
                case "item_denchi_1":
                    objectID = 58;
                    break;
                case "item_denchi_2":
                    objectID = 59;
                    break;
                case "item_denchi_3":
                    objectID = 60;
                    break;
                case "item_peets_kutu":
                    objectID = 61;
                    break;
                case "item_hocyouki":
                    objectID = 62;
                    break;
                case "item_rex_tooth":
                    objectID = 88;
                    break;
                case "twig":
                    objectID = 91;
                    break;
                case "item_kayaku":
                    objectID = 98;
                    break;
                case "item_goggle":
                    objectID = 99;
                    break;
                case "love_letter":
                    objectID = 100;
                    break;
                case "item_tug":
                    objectID = 101;
                    break;
                case "item_ticket":
                    objectID = 102;
                    break;
                case "item_houtai":
                    objectID = 103;
                    break;
                case "drake_redcrest_album":
                    objectID = 105;
                    break;
                case "item_nwing_item":
                    objectID = 107;
                    break;
                case "item_army_photo":
                    objectID = 108;
                    break;
                case "giga_robos_left_leg":
                    objectID = 109;
                    break;
                case "circuit_schematic":
                    objectID = 110;
                    break;
                case "giga_battery_full":
                    objectID = 111;
                    break;
                case "small_handkerchief":
                    objectID = 112;
                    break;
                case "pink_flower":
                    objectID = 113;
                    break;
                case "the_scurvy_splinter":
                    objectID = 114;
                    break;
                case "old_boxers":
                    objectID = 115;
                    break;
                case "outdated_scarf":
                    objectID = 116;
                    break;
                case "item_t_block_1":
                    objectID = 117;
                    break;
                case "item_t_block_2":
                    objectID = 118;
                    break;
                case "item_t_block_3":
                    objectID = 119;
                    break;
                case "item_t_block_4":
                    objectID = 120;
                    break;
                case "item_t_block_5":
                    objectID = 121;
                    break;
                case "item_t_block_6":
                    objectID = 122;
                    break;
                case "item_car_item":
                    objectID = 123;
                    break;
                case "item_papa_yubiwa":
                    objectID = 124;
                    break;
                case "weeds":
                    objectID = 125;
                    break;
                case "block_layout":
                    objectID = 126;
                    break;
                case "item_frog":
                    objectID = 127;
                    break;
                case "rouka_tao_bag":
                    objectID = 130;
                    break;
                case "cb_cannon_lv_2":
                    objectID = 131;
                    break;
                case "cb_radar":
                    objectID = 132;
                    break;
                case "tamagotchi":
                    objectID = 133;
                    break;
                case "primopuel":
                    objectID = 134;
                    break;
                case "empty_can":
                    objectID = 135;
                    break;
                case "item_candy_gomi":
                    objectID = 136;
                    break;
                case "item_okasi_gomi_1":
                    objectID = 137;
                    break;
                case "item_okasi_gomi_2":
                    objectID = 138;
                    break;
                case "super_eggplant":
                    objectID = 139;
                    break;
                case "item_kure_1":
                    objectID = 147;
                    break;
                case "blue_crayon":
                    objectID = 148;
                    break;
                case "item_kure_3":
                    objectID = 149;
                    break;
                case "item_kure_4":
                    objectID = 150;
                    break;
                case "item_kure_5":
                    objectID = 151;
                    break;
                case "item_c_denchi":
                    objectID = 158;
                    break;

                default:
                    objectID = 139;
                    break;
            }

            return objectID.ToString();

        }

        private void updateDrakeSuitLoc(string stagefile, string newObjectName, string player, string itemName)
        {

            string objectID = objectNameToGameID(newObjectName);

            File.AppendAllText(
            stagefile,
            "sub_2560:" +
            "\r\n\tlib\t247.w" +
            "\r\n\tlib\t12.w" +
            "\r\n\tanim\t20008.d, 4.d" +
            "\r\n\twait\t@time, 16.w" +
            "\r\n\tsfx\t851978.d, 1.d" +
            "\r\n\twait\t@time, 61.w" +
            "\r\n\tsfx\t325.d, 1.d" +
            "\r\n\twait\t@anim, 20008.d, -1.d" +
            "\r\n\tanim\t20008.d, 5.d" +
            "\r\n\tpushbp" +
            "\r\n\tsetsp\t3.w" +
            "\r\n\tlib\t259.w" +
            "\r\n\tpopbp\r\n\tsfx\t295.d, 1.d" +
            "\r\n\tset\tvar(0.d), sub(obj(@dir, 20008.d), 180.w)" +
            "\r\n\tset\tvar(1.d), add(obj(@pos_x, 20008.d), mul(sin(mul(var(0.d), 100.w)), 20.w))" +
            "\r\n\tset\tvar(2.d), obj(@pos_y, 20008.d)" +
            "\r\n\tset\tvar(3.d), add(obj(@pos_z, 20008.d), mul(cos(mul(var(0.d), 100.w)), 20.w))" +
            "\r\n\tptcl\t35.d, @pos, var(1.d), var(2.d), var(3.d), 150.w, 150.w, 150.w, add(var(0.d), 180.w)" +
            "\r\n\tlib\t96.w" +
            //"\r\n\tanim\t20008.d, 6.d" +
            //"\r\n\twait\t@anim, 20008.d, -1.d" +
            "\r\n\tanim\t20008.d, 1020.d" +
            "\r\n\tmsg\tvoice(7.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\tanim(0.b, 20008.w, 1019.d)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\t\"Take this special gift! It's just for you!\"," +
            "\r\n\t\twait(10.b)" +
            "\r\n\tmsg\tvoice(7.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\tanim(0.b, 20008.w, 1021.d)," +
            "\r\n\t\t\"HA HA HA HAAAAAAAAAA!!!\"," +
            "\r\n\t\tanim(0.b, 20008.w, -1.d)," +
            "\r\n\t\tanim(0.b, 20008.w, 1.d)" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\tpushbp" +
            "\r\n\tsetsp\t" + objectID + ".d" +
            "\r\n\tset\tvar(673.d), 1.w" +
            //"\r\n\tlib\t79.w ; Give Set Item" +

            "\r\n\tset\tvar(62.d), sp(0.b)" +
            "\r\n\tcall\t20000.d, 503.d, var(62.d)" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\twait\t@anim, 20000.d, 20.w" +
            "\r\n\tsfx\t419.d, 1.d" +
            "\r\n\tlib\t305.w" +
            "\r\n\tset\tvar(62.d), sp(0.b)" +
            "\r\n\tpushbp" +
            "\r\n\tmsg\trgba(2164228351.d)," +
            "\r\n\t\t\"You found " + player + "\'s \"," +
            "\r\n\t\t\"" + itemName + "\"," +
            "\r\n\t\tcolor(0.b)," +
            "\r\n\t\twait(254.b)" +
            "\r\n\tpopbp" +
            "\r\n\tset\tvar(90.d), 0.w" +
            "\r\n\tset\tvar(96.d), 0.w" +

            "\r\n\tpopbp" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\tlib\t67.w " +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\tlib\t105.w ; @anim" +
            "\r\n\tmsg\tvoice(7.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\tanim(0.b, 20008.w, 1019.d)," +
            "\r\n\t\t\"Shall I show you how to don it, friend?\"," +
            "\r\n\t\tanim(0.b, 20008.w, 1020.d)," +
            "\r\n\t\twait(255.b)," +
            "\r\n\t\tstay" +
            "\r\n\trun\t*sub_2564" +
            "\r\n\tif\tne(cur_suit, 1.d), else *loc_2563" +
            "\r\n\tmsg\tvoice(7.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\tanim(0.b, 20008.w, 1019.d)," +
            "\r\n\t\t\"Put it on! Do not keep Drake Redcrest waiting!\\n\"," +
            "\r\n\t\t\"Show me how it looks on you!\"," +
            "\r\n\t\tanim(0.b, 20008.w, 1020.d)," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tendif\t*loc_2562\r\n"
            );

        }

        private void updateFrogSuitLoc(string stagefile, string newObjectName, string player, string itemName)
        {
            string objectID = objectNameToGameID(newObjectName);

            File.AppendAllText(
            stagefile,
            "sub_2560:" +
            "loc_1857:" +
            "\r\n\tcase\teq(var(31.d), 2.w), else *loc_1853" +
            "\r\n\tpushbp" +
            "\r\n\tsetsp\t1.w" +
            "\r\n\tlib\t259.w" +
            "\r\n\tpopbp" +
            "\r\n\tanim\t20057.d, 8.d" +
            "\r\n\twait\t@anim, 20057.d, -1.d" +
            "\r\n\tanim\t20057.d, 3.d" +
            "\r\n\tpos\t20000.d, 49993.d, 0.w, -21088.d" +
            "\r\n\tdir\t20000.d, 5.w" +
            "\r\n\tdir\t20057.d, 180.w" +
            "\r\n\tpushbp" +
            "\r\n\tsetsp\t1.w" +
            "\r\n\tlib\t259.w" +
            "\r\n\tpushbp" +
            "\r\n\tsetsp\t20057.d" +
            "\r\n\tsetsp\t0.d" +
            "\r\n\tsetsp\t0.w" +
            "\r\n\tsetsp\t13.d" +
            "\r\n\tsetsp\t0.w" +
            "\r\n\tlib\t210.w" +
            "\r\n\tpopbp" +
            "\r\n\tlib\t67.w" +
            "\r\n\tanim\t20057.d, 16.d" +
            "\r\n\tpushbp" +
            "\r\n\tsetsp\t" + objectID +
            "\r\n\tset\tvar(674.d), 1.w" +
            //"\r\n\tlib\t79.w ; Give Set Item" +

            "\r\n\tset\tvar(62.d), sp(0.b)" +
            "\r\n\tcall\t20000.d, 503.d, var(62.d)" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\twait\t@anim, 20000.d, 20.w" +
            "\r\n\tsfx\t419.d, 1.d" +
            "\r\n\tlib\t305.w" +
            "\r\n\tset\tvar(62.d), sp(0.b)" +
            "\r\n\tpushbp" +
            "\r\n\tmsg\trgba(2164228351.d)," +
            "\r\n\t\t\"You found " + player + "\'s \"," +
            "\r\n\t\t\"" + itemName + "\"," +
            "\r\n\t\tcolor(0.b)," +
            "\r\n\t\twait(254.b)" +
            "\r\n\tpopbp" +
            "\r\n\tset\tvar(90.d), 0.w" +
            "\r\n\tset\tvar(96.d), 0.w" +

            "\r\n\tpopbp" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\tanim\t20057.d, 1.d" +
            "\r\n\tlib\t67.w" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\tcamera\t@pos, 47359.d, 919.w, -25067.d, 3.d, 0.w" +
            "\r\n\tcamera\t@unk227, 51412.d, 127.w, -17758.d, 3.d, 0.w" +
            "\r\n\tcamera\t@unk229, 26.w, 3.d, 0.w" +
            "\r\n\tcamera\t@distance, 83.w, 3.d, 0.w" +
            "\r\n\twait\t@time, 200.w" +
            "\r\n\twait\t@cam" +
            "\r\n\tmsg\tvoice(26.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\tanim(0.b, 20057.w, 2.d)," +
            "\r\n\t\t\"Could you please find my lost boyfriend, ribbit?!\"," +
            "\r\n\t\tanim(0.b, 20057.w, 1.d)," +
            "\r\n\t\twait(254.b)" +
            "\r\n\tmdir\t20057.d, @dir, 0.w, 50.w, -1.d" +
            "\r\n\twait\t@dir, 20057.d" +
            "\r\n\tmoveto\t20057.d, 50500.d, 0.w, -15060.d, 200.w, 7.d, -2.d" +
            "\r\n\twait\t@move, 20057.d" +
            "\r\n\tanim\t20057.d, 5.d" +
            "\r\n\twait\t@anim, 20057.d, -1.d" +
            "\r\n\tsfx\t393226.d, 1.d" +
            "\r\n\tanim\t251.d, 1.w, 0.w" +
            "\r\n\tdisp\t20057.d, 0.d" +
            "\r\n\twait\t@time, 100.w" +
            "\r\n\tcamera\t@unk230" +
            "\r\n\tlib\t88.w" +
            "\r\n\tset\tvar(31.d), add(var(31.d), 1.w)" +
            "\r\n\tbreak\t*loc_1853\r\n"
            );

        }

        private void updateTraumaSuitLoc(string stagefile, string newObjectName, string player, string itemName)
        {
            string objectID = objectNameToGameID(newObjectName);

            File.AppendAllText(
            stagefile,
            "sub_27:" +
            "\r\n\tset\tflag(963.d), 0.d" +
            "\r\n\tcall\t20000.d, -1.d" +
            "\r\n\trun\t*sub_509" +
            "\r\n\tif\tand(eq(flag(32.d), 1.w), eq(flag(301.d), 0.d)), else *loc_504" +
            "\r\n\tset\tflag(add(1700.d, 32.d)), 1.w" +
            "\r\n\tendif\t*loc_29\r\n"
            );

            File.AppendAllText(
            stagefile,
            "sub_497:" +
            "\r\n\tmsg\tvoice(0.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\t\"Oh, and you should know, if you ever pass\\n\"," +
            "\r\n\t\t\"out like that again, don't get too worried.\"," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tanim\t20005.d, 1007.d" +
            "\r\n\tpushbp" +
            "\r\n\tsetsp\tactor_name(20005.d)" +
            "\r\n\tmsg\tvoice(0.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\t\"Your faithful friend, Telly Vision (that's me!),\\n\"," +
            "\r\n\t\t\"will carry you back to the Chibi-House!\"," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tpopbp" +
            "\r\n\tanim\t20005.d, 1.d" +
            "\r\n\twait\t@time, 30.w" +
            "\r\n\tanim\t20005.d, 1002.d" +
            "\r\n\tmsg\tvoice(0.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\t\"Oh, and another thing!\"," +
            "\r\n\t\tsize(255.b)," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tmenu\t1001.d, -1.d, 5.w" +
            "\r\n\tmenu\t1000.d, 1.w" +
            "\r\n\tmsg\tvoice(0.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\t\"If you press \"," +
            "\r\n\t\ticon(8.b)," +
            "\r\n\t\t\" and select $Remove,$\\n\"," +
            "\r\n\t\t\"you'll change out of your Trauma Suit.\"," +
            "\r\n\t\twait(254.b)" +
            "\r\n\tanim\t20005.d, 1.d" +
            "\r\n\tmenu\t1000.d, -1.d" +
            "\r\n\tset\tvar(676.d), 1.w" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\tpushbp" +
            "\r\n\tsetsp\t" + objectID + ".d" +
            //"\r\n\tlib\t79.w ; Give Set Item" +


            "\r\n\tset\tvar(62.d), sp(0.b)" +
            "\r\n\tcall\t20000.d, 503.d, var(62.d)" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\twait\t@anim, 20000.d, 20.w" +
            "\r\n\tsfx\t419.d, 1.d" +
            "\r\n\tlib\t305.w" +
            "\r\n\tset\tvar(62.d), sp(0.b)" +
            "\r\n\tpushbp" +
            "\r\n\tmsg\trgba(2164228351.d)," +
            "\r\n\t\t\"You found " + player + "\'s \"," +
            "\r\n\t\t\"" + itemName + "\"," +
            "\r\n\t\tcolor(0.b)," +
            "\r\n\t\twait(254.b)" +
            "\r\n\tpopbp" +
            "\r\n\tset\tvar(90.d), 0.w" +
            "\r\n\tset\tvar(96.d), 0.w" +

            "\r\n\tpopbp" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\tlib\t67.w " +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\tlib\t105.w ; @anim" +
            "\r\n\treturn\r\n"
            );

        }

        private void updateGhostSuitLoc(string stagefile, string newObjectName, string player, string itemName)
        {
            string objectID = objectNameToGameID(newObjectName);

            File.AppendAllText(
            stagefile,
            "loc_504:" +
            // RANDOMIZER: gate on our own AP-tracking vars instead of the vanilla item(34.d)
            // check - requires the player to already have the Trauma Suit (var(676)=1, set by
            // updateTraumaSuitLoc) and not yet have this AP item (var(677)=0), rather than relying
            // on vanilla's own trauma/ghost unlock flags. See project_chibi_house_suits memory.
            "\r\n\telif\tand(eq(var(676.d), 1.w), eq(var(677.d), 0.w)), else *loc_506" +
            "\r\n\tpushbp" +
            "\r\n\tsetsp\t" + objectID +
            //"\r\n\tlib\t79.w ; Give Set Item" +

            "\r\n\tset\tvar(62.d), sp(0.b)" +
            "\r\n\tcall\t20000.d, 503.d, var(62.d)" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\twait\t@anim, 20000.d, 20.w" +
            "\r\n\tsfx\t419.d, 1.d" +
            "\r\n\tlib\t305.w" +
            "\r\n\tset\tvar(62.d), sp(0.b)" +
            "\r\n\tpushbp" +
            "\r\n\tmsg\trgba(2164228351.d)," +
            "\r\n\t\t\"You found " + player + "\'s \"," +
            "\r\n\t\t\"" + itemName + "\"," +
            "\r\n\t\tcolor(0.b)," +
            "\r\n\t\twait(254.b)" +
            "\r\n\tpopbp" +
            "\r\n\tset\tvar(90.d), 0.w" +
            "\r\n\tset\tvar(96.d), 0.w" +

            "\r\n\tset\tvar(677.d), 1.w" +
            "\r\n\tset\tflag(add(1700.d, 34.d)), 1.w" +

            "\r\n\tpopbp" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\tlib\t67.w" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\tlib\t105.w ; @anim" +
            "\r\n\tendif\t*loc_29\r\n"
            );

        }

        private void updatePajamaSuitLoc(string stagefile, string newObjectName, string player, string itemName)
        {
            string objectID = objectNameToGameID(newObjectName);

            File.AppendAllText(
            stagefile,
            "loc_1223:" +
            "\r\n\telif\tge(var(76.d), 44.d), else *loc_1228" +
            "\r\n\tif\tlt(var(685.d), var(107.d)), else *loc_1227" +
            "\r\n\tset\tflag(1657.d), 1.w" +
            "\r\n\tpushbp" +
            "\r\n\tsetsp\tactor_name(20063.d)" +
            "\r\n\tmsg\tvoice(4.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\tanim(0.b, 20002.w, 1016.d)," +
            "\r\n\t\t\"Oh, Cheebo! I finished making your pj's!\"," +
            "\r\n\t\tanim(0.b, 20002.w, 1015.d)," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tpopbp" +
            "\r\n\tmsg\tvoice(4.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\tanim(0.b, 20002.w, 1016.d)," +
            "\r\n\t\t\"Try them on for me. I want to see how they look!\"," +
            "\r\n\t\tanim(0.b, 20002.w, 1015.d)," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tpushbp" +

            "\r\n\tsetsp\t" + objectID +
            //"\r\n\tlib\t79.w ; Give Set Item" +

            "\r\n\tset\tvar(62.d), sp(0.b)" +
            "\r\n\tcall\t20000.d, 503.d, var(62.d)" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\twait\t@anim, 20000.d, 20.w" +
            "\r\n\tsfx\t419.d, 1.d" +
            "\r\n\tlib\t305.w" +
            "\r\n\tset\tvar(62.d), sp(0.b)" +
            "\r\n\tpushbp" +
            "\r\n\tmsg\trgba(2164228351.d)," +
            "\r\n\t\t\"You found " + player + "\'s \"," +
            "\r\n\t\t\"" + itemName + "\"," +
            "\r\n\t\tcolor(0.b)," +
            "\r\n\t\twait(254.b)" +
            "\r\n\tpopbp" +
            "\r\n\tset\tvar(90.d), 0.w" +
            "\r\n\tset\tvar(96.d), 0.w" +
            "\r\n\tset\tvar(675.d), 1.w" +

            "\r\n\tpopbp" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\tlib\t67.w" +
            "\r\n\twait\t@time, 1.w" +
            //"\r\n\tset\tcur_suit, 7.d" +
            "\r\n\twait\t@time, 210.w" +
            "\r\n\tmsg\tvoice(4.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\tanim(0.b, 20002.w, 1016.d)," +
            "\r\n\t\t\"Awwww...you are just TOO CUTE!\"," +
            "\r\n\t\tanim(0.b, 20002.w, 1015.d)," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tmsg\tvoice(4.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\tanim(0.b, 20002.w, 1016.d)," +
            "\r\n\t\t\"When you're tired, make sure to get some sleep!\"," +
            "\r\n\t\tanim(0.b, 20002.w, 1015.d)," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tendif\t*loc_1226\r\n"

            );

        }

        private void updatePajamaSuitKitchenLoc(string stagefile, string newObjectName, string player, string itemName)
        {
            string objectID = objectNameToGameID(newObjectName);

            File.AppendAllText(
            stagefile,
            "loc_1631:" +
            "\r\n\tset\tflag(1644.d), 1.w" +
            "\r\n\tanim\t20002.d, var(280.d)" +
            "\r\n\tpushbp" +
            "\r\n\tsetsp\tactor_name(20063.d)" +
            "\r\n\tmsg\tvoice(4.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\t\"You were outstanding, Cheebo!\"," +
            "\r\n\t\tanim(1.b, 20002.w, 281.d)," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tpopbp" +
            "\r\n\tanim\t20002.d, var(280.d)" +
            "\r\n\tpushbp\r\n\tsetsp\tactor_name(20063.d)" +
            "\r\n\tsetsp\tactor_name(20002.d)" +
            "\r\n\tmsg\tvoice(4.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\t\"Since you've been working so hard,\\n\"," +
            "\r\n\t\t\"I have a present for you.\"," +
            "\r\n\t\tanim(1.b, 20002.w, 281.d)," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tpopbp" +
            "\r\n\tpushbp" +

            "\r\n\tsetsp\t" + objectID +
            //"\r\n\tlib\t79.w ; Give Set Item" +

            "\r\n\tset\tvar(62.d), sp(0.b)" +
            "\r\n\tcall\t20000.d, 503.d, var(62.d)" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\twait\t@anim, 20000.d, 20.w" +
            "\r\n\tsfx\t419.d, 1.d" +
            "\r\n\tlib\t305.w" +
            "\r\n\tset\tvar(62.d), sp(0.b)" +
            "\r\n\tpushbp" +
            "\r\n\tmsg\trgba(2164228351.d)," +
            "\r\n\t\t\"You found " + player + "\'s \"," +
            "\r\n\t\t\"" + itemName + "\"," +
            "\r\n\t\tcolor(0.b)," +
            "\r\n\t\twait(254.b)" +
            "\r\n\tpopbp" +
            "\r\n\tset\tvar(90.d), 0.w" +
            "\r\n\tset\tvar(96.d), 0.w" +
            "\r\n\tset\tvar(675.d), 1.w" +

            "\r\n\tpopbp" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\tlib\t67.w" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\tanim\t20002.d, var(280.d)" +
            "\r\n\tmsg\tvoice(4.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\t\"Put it on and let me see how it looks.\"," +
            "\r\n\t\tanim(1.b, 20002.w, 281.d)," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tset\tcur_suit, 7.d" +
            "\r\n\twait\t@time, 300.w" +
            "\r\n\tanim\t20002.d, var(280.d)" +
            "\r\n\tmsg\tvoice(4.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\t\"Oh, that is CUTE! I was worried about the size.\"," +
            "\r\n\t\tanim(1.b, 20002.w, 281.d)," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tanim\t20002.d, var(280.d)" +
            "\r\n\tmsg\tvoice(4.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\t\"If you get tired, make sure to get some sleep!\"," +
            "\r\n\t\tanim(1.b, 20002.w, 281.d)," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tendif\t*loc_1592\r\n"


            );

        }

        private void updateToaSuitLoc(string stagefile, string newObjectName, string player, string itemName)
        {
            string objectID = objectNameToGameID(newObjectName);

            File.AppendAllText(
            stagefile,
            "sub_1480:" +
            "\r\n\tif\teq(item(25.d), 0.w), else *loc_1483" +
            "\r\n\tmsg\tvoice(13.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\tanim(0.b, 20017.w, 20.d)," +
            "\r\n\t\t\"With you on our side, we'll have some serious\\n\"," +
            "\r\n\t\t\"manpower! Errr...I mean robopower!\"," +
            "\r\n\t\tanim(0.b, 20017.w, 1.d)," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tmsg\tvoice(13.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\tanim(0.b, 20017.w, 20.d)," +
            "\r\n\t\t\"Here...put this on!\"," +
            "\r\n\t\tanim(0.b, 20017.w, 1.d)," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tpushbp" +

            "\r\n\tsetsp\t" + objectID +
            //"\r\n\tlib\t79.w ; Give Set Item" +

            "\r\n\tset\tvar(62.d), sp(0.b)" +
            "\r\n\tcall\t20000.d, 503.d, var(62.d)" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\twait\t@anim, 20000.d, 20.w" +
            "\r\n\tsfx\t419.d, 1.d" +
            "\r\n\tlib\t305.w" +
            "\r\n\tset\tvar(62.d), sp(0.b)" +
            "\r\n\tpushbp" +
            "\r\n\tmsg\trgba(2164228351.d)," +
            "\r\n\t\t\"You found " + player + "\'s \"," +
            "\r\n\t\t\"" + itemName + "\"," +
            "\r\n\t\tcolor(0.b)," +
            "\r\n\t\twait(254.b)" +
            "\r\n\tpopbp" +
            "\r\n\tset\tvar(90.d), 0.w" +
            "\r\n\tset\tvar(96.d), 0.w" +
            "\r\n\tset\tvar(678.d), 1.w" +

            "\r\n\tpopbp" +
            "\r\n\twait\t@time, 1.w" +
            "\r\n\tlib\t67.w" +
            "\r\n\twait\t@time, 1.w" +
            //"\r\n\tset\tcur_suit, 3.d" +
            "\r\n\twait\t@time, 120.w" +
            "\r\n\tsfx\t1114148.d, 1.d" +
            "\r\n\tanim\t20018.d, 49.d, 50.d" +
            "\r\n\tanim\t20019.d, 49.d, 50.d" +
            "\r\n\tmdir\t20018.d, @obj, 20000.d, 30.w, -1.d" +
            "\r\n\tmdir\t20019.d, @obj, 20000.d, 30.w, -1.d" +
            "\r\n\twait\t@dir, 20018.d" +
            "\r\n\tmsg\tvoice(13.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\tanim(0.b, 20017.w, 20.d)," +
            "\r\n\t\t\"Hmmmm...\"," +
            "\r\n\t\tanim(0.b, 20017.w, 1.d)," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tmsg\tvoice(13.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\tanim(0.b, 20017.w, 20.d)," +
            "\r\n\t\t\"WELL, SLAP ME WITH A BATTLESHIP!\"," +
            "\r\n\t\tanim(0.b, 20017.w, 1.d)," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tanim\t20018.d, 56.d" +
            "\r\n\tanim\t20019.d, 56.d" +
            "\r\n\tsfx\t1114147.d, 1.d" +
            "\r\n\twait\t@sfx, 1114147.d" +
            "\r\n\tanim\t20018.d, 1.d" +
            "\r\n\tanim\t20019.d, 1.d" +
            "\r\n\tanim2\t20018.d, 21.d, 0.w, 200.w, 1.d, 0.w, 100.w" +
            "\r\n\twait\t@time, 10.w" +
            "\r\n\tmsg\tvoice(9.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\t\"You're even more hideous than the real Tao!\"," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tanim2\t20019.d, 21.d, 0.w, 200.w, 1.d, 0.w, 100.w" +
            "\r\n\twait\t@time, 10.w" +
            "\r\n\tmsg\tvoice(9.b)," +
            "\r\n\t\tspeed(0.b)," +
            "\r\n\t\t\"The other boys will crack when they see you!\"," +
            "\r\n\t\twait(255.b)" +
            "\r\n\tendif\t*loc_1482\r\n"


            );

        }

        private void updateSuitCasePassword(string stagefile, int newPass)
        {
            File.AppendAllText(
            stagefile,
            "loc_848:" +
            "\r\n\telif\teq(flag(9.d), 1.w), else *loc_856" +
            "\r\n\tsfx\t-65531.d, 4.d, 300.w, 0.w" +
            "\r\n\tmsg\t\"Enter password\\n\"," +
            "\r\n\t\tsize(24.b)," +
            "\r\n\t\t\" \"," +
            "\r\n\t\tinput(4.b, 4.b, 4.b)" +
            "\r\n\tif\teq(result, " + newPass + "), else *loc_855" +
            "\r\n\trun\t*sub_852" +
            "\r\n\tendif\t*loc_851"
            );

        }

        private void updateSuitCasePasswordText(string stagefile, int newPass)
        {
            File.AppendAllText(
            stagefile,
            "sub_49:" +
            "\r\n\tlib\t38.w" +
            "\r\n\tmsg\t\"Password: " + newPass + "\"," +
            "\r\n\t\twait(254.b)" +
            "\r\n\tset\tflag(9.d), 1.w" +
            "\r\n\tlib\t39.w" +
            "\r\n\treturn" +
            "\r\n\r\n"
            );

        }

        private void updateLeftFootPassword(string stagefile, int newPass)
        {
            File.AppendAllText(
            stagefile,
            "loc_617:\r\n" +
            "\tlib\t38.w" +
            "\r\n\tcall\t20007.d, 3.d, 1.d" +
            "\r\n\twait\t@time, 40.w" +
            "\r\n\tcamera\t@pos, 7478.w, 94.w, -4769.d, 3.d, 0.w" +
            "\r\n\tcamera\t@unk227, 11745.w, 1260.w, -11974.d, 3.d, 0.w" +
            "\r\n\tcamera\t@unk229, 39.w, 3.d, 0.w" +
            "\r\n\tcamera\t@distance, 84.w, 3.d, 0.w" +
            "\r\n\twait\t@time, 80.w" +
            "\r\n\tif\teq(flag(27.d), 1.d), else *loc_616" +
            "\r\n\tcall\t20000.d, -1.d" +
            "\r\n\tset\tflag(1080.d), 1.d" +
            "\r\n\trun\t*sub_610" +
            "\r\n\tset\tvar(39.d), result" +
            "\r\n\twait\t@time, 50.w" +
            "\r\n\tcall\t20007.d, 3.d, 0.d" +
            "\r\n\twait\t@time, 100.w" +
            "\r\n\tif\teq(var(39.d), " + newPass + "), else *loc_604" +
            "\r\n\tsfx\t327720.d, 1.d" +
            "\r\n\tset\tvar(672.d), 1.w" +
            "\r\n\twait\t@sfx, 327720.d" +
            "\r\n\tif\teq(flag(28.d), 1.d), else *loc_603" +
            "\r\n\trun\t*sub_539" +
            "\r\n\tlib\t106.w" +
            "\r\n\tendif\t*loc_536"
            );
        }

        // Returns the existing handler label for objectID in stagefile, or null if none.
        private string findExistingInteractLabel(string content, int objectID)
        {
            string marker = "\t.interact  " + objectID + ".d, *";
            int idx = content.IndexOf(marker);
            if (idx < 0) return null;
            int labelStart = idx + marker.Length;
            int labelEnd = content.IndexOfAny(new char[] { '\r', '\n' }, labelStart);
            return content.Substring(labelStart, labelEnd - labelStart).Trim();
        }

        // Replaces an existing .interact registration to point to a new wrapper label.
        private string redirectInteract(string content, int objectID, string newLabel)
        {
            string marker = "\t.interact  " + objectID + ".d, *";
            int idx = content.IndexOf(marker);
            if (idx < 0) return content;
            int lineEnd = content.IndexOfAny(new char[] { '\r', '\n' }, idx + marker.Length);
            return content.Substring(0, idx) + "\t.interact  " + objectID + ".d, *" + newLabel + content.Substring(lineEnd);
        }

        private void addFlagOnlyHandler(string stagefile, int objectID, int locationCode)
        {
            int flagNum = 2100 + locationCode;
            string content = File.ReadAllText(stagefile);

            string originalLabel = findExistingInteractLabel(content, objectID);
            if (originalLabel != null)
            {
                // Redirect: replace the .interact registration to our wrapper, then append
                // a wrapper that sets the flag and delegates to the original handler via run.
                // This preserves ALL original logic (item grants, cutscenes, etc.).
                string wrapperLabel = "ap_flag_" + objectID;
                content = redirectInteract(content, objectID, wrapperLabel);
                content += "\r\n" + wrapperLabel + ":" + Environment.NewLine +
                           "\trun\t*" + originalLabel + Environment.NewLine +
                           "\tset\tflag(" + flagNum + ".d), 1.d" + Environment.NewLine +
                           "\treturn\n" + Environment.NewLine;
                File.WriteAllText(stagefile, content);
            }
            else
            {
                File.AppendAllText(
                    stagefile,
                    "\t.interact  " + objectID + ".d, *ap_flag_" + objectID + Environment.NewLine + Environment.NewLine +
                    "ap_flag_" + objectID + ":" + Environment.NewLine +
                    "\tset\tflag(" + flagNum + ".d), 1.d" + Environment.NewLine +
                    "\treturn\n" + Environment.NewLine);
            }
        }

        // RANDOMIZER: Pan Drop Trap "living_tub" object id per stage, for the instant on-pickup
        // animation below - null means the room has no tub prop, so the animation is sound +
        // fall only. See project_pan_drop_trap memory for the full design trail (this mirrors
        // the vanilla lib_36 charging-gag reuse; stage03/stage09 genuinely have no tub prop).
        private string getPanDropTrapTubId(string stagefile)
        {
            string fileName = Path.GetFileName(stagefile);
            if (fileName.StartsWith("stage07")) return "62";
            if (fileName.StartsWith("stage01")) return "161";
            if (fileName.StartsWith("stage02")) return "421";
            if (fileName.StartsWith("stage04")) return "262";
            if (fileName.StartsWith("stage06")) return "115";
            return null;
        }

        // RANDOMIZER: builds the instant Pan Drop Trap animation - fires right in the pickup's
        // own .interact handler (a synchronous, player-in-control moment, same safety class as
        // the outlet-charging trigger) instead of waiting for the next outlet charge. This is
        // purely additional flavor: the item is still delivered over the network as normal and
        // will independently queue/resolve via var(1874)/lib_36 too - see project_pan_drop_trap
        // memory for why a self-found trap firing twice was accepted rather than engineered
        // around.
        private string buildPanDropTrapAnimSnippet(string stagefile)
        {
            string tub = getPanDropTrapTubId(stagefile);
            string nl = Environment.NewLine;

            if (tub != null)
            {
                return
                    nl + "\t; RANDOMIZER: instant Pan Drop Trap animation (this player's own trap)" +
                    nl + "\tcall\t20000.d, 701.d" +
                    nl + "\tset\tvar(1875.d), obj(@dir, " + tub + ".d)" +
                    nl + "\tset\tvar(1876.d), obj(@pos_x, " + tub + ".d)" +
                    nl + "\tset\tvar(1877.d), obj(@pos_y, " + tub + ".d)" +
                    nl + "\tset\tvar(1878.d), obj(@pos_z, " + tub + ".d)" +
                    nl + "\tdir\t" + tub + ".d, obj(@dir, 20000.d)" +
                    nl + "\tpos\t" + tub + ".d, obj(@pos_x, 20000.d), obj(@pos_y, 20000.d), obj(@pos_z, 20000.d)" +
                    nl + "\tsfx\t237.d, 1.d" +
                    nl + "\twait\t@time, 30.w" +
                    nl + "\tanim\t20000.d, 1153.d" +
                    nl + "\twait\t@anim, 20000.d, -1.d" +
                    nl + "\twait\t@time, 45.w" +
                    nl + "\tanim\t" + tub + ".d, 1.w" +
                    nl + "\tanim\t20000.d, 1167.d" +
                    nl + "\twait\t@time, 24.w" +
                    nl + "\tsfx\t236.d, 1.d" +
                    nl + "\twait\t@time, 1.w" +
                    nl + "\tanim\t20000.d, 53.d" +
                    nl + "\twait\t@time, 15.w" +
                    nl + "\tsfx\t237.d, 1.d" +
                    nl + "\twait\t@anim, 20000.d, -1.d" +
                    nl + "\tanim\t20000.d, 54.d" +
                    nl + "\tptcl\t5.d, @obj, " + tub + ".d, 0.w, 1000.w, 0.w, 100.w, 100.w, 100.w, 0.w" +
                    nl + "\twait\t@time, 50.w" +
                    nl + "\tsfx\t70.d, 1.d" +
                    nl + "\twait\t@anim, " + tub + ".d, -1.d" +
                    nl + "\twait\t@time, 80.w" +
                    nl + "\tanim\t20000.d, 55.d" +
                    nl + "\twait\t@anim, 20000.d, -1.d" +
                    nl + "\tanim\t" + tub + ".d, 0.w" +
                    nl + "\tread\t@anim, 20000.d, 0.d" +
                    nl + "\tpos\t" + tub + ".d, var(1876.d), var(1877.d), var(1878.d)" +
                    nl + "\tdir\t" + tub + ".d, var(1875.d)" +
                    nl + "\tanim\t20000.d, 1.d" +
                    nl + "\tcall\t20000.d, 700.d";
            }

            return
                nl + "\t; RANDOMIZER: instant Pan Drop Trap animation (this player's own trap, no" +
                nl + "\t; tub prop in this room - sound + fall only)" +
                nl + "\tcall\t20000.d, 701.d" +
                nl + "\tsfx\t236.d, 1.d" +
                nl + "\twait\t@time, 1.w" +
                nl + "\tanim\t20000.d, 53.d" +
                nl + "\twait\t@time, 60.w" +
                nl + "\tsfx\t70.d, 1.d" +
                nl + "\tanim\t20000.d, 55.d" +
                nl + "\twait\t@time, 45.w" +
                nl + "\tread\t@anim, 20000.d, 0.d" +
                nl + "\tanim\t20000.d, 1.d" +
                nl + "\tcall\t20000.d, 700.d";
        }

        private void addInGameMessages(string stagefile, int objectID, string player, string newObjectName, int locationCode = -1, bool triggerPanDropAnim = false)
        {
            string flagSet = locationCode >= 0
                ? "\r\n\tset\tflag(" + (2100 + locationCode) + ".d), 1.d"
                : "";

            string panDropAnim = triggerPanDropAnim ? buildPanDropTrapAnimSnippet(stagefile) : "";

            string content = File.ReadAllText(stagefile);
            string originalLabel = findExistingInteractLabel(content, objectID);

            if (originalLabel != null)
            {
                // This object already has an interact handler in the original script (e.g., the
                // Giga Charger handler that adds item 48 and triggers Plankbeard's reunion).
                // Redirect .interact to our wrapper, show the AP message, then run the original
                // handler so ALL its logic (item grants, cutscenes, flag sets) still executes.
                string wrapperLabel = "ap_text_" + objectID;
                content = redirectInteract(content, objectID, wrapperLabel);

                string frogRingExtra = newObjectName.Contains("Frog Ring")
                    ? "\r\n\tpushbp\r\n\tsetsp\t0.d\r\n\tlib\t77.w\r\n\tpopbp"
                    : "";

                content +=
                    "\r\n" + wrapperLabel + ":" + Environment.NewLine +
                    "\tmsg\tvoice(" + apItemVoice + ".b)," + Environment.NewLine +
                    "\t\t\"You found " + player + "\'s\", " + Environment.NewLine +
                    "\t\t\"" + " " + newObjectName + "\"," + Environment.NewLine +
                    "\t\twait(254.b)" +
                    frogRingExtra +
                    flagSet +
                    panDropAnim + Environment.NewLine +
                    "\trun\t*" + originalLabel + Environment.NewLine +
                    "\treturn\n" + Environment.NewLine;

                File.WriteAllText(stagefile, content);
            }
            else if (newObjectName.Contains("Frog Ring"))
            {
                File.AppendAllText(
               stagefile,
               "\t.interact  " + objectID + ".d, *ap_text_" + objectID + Environment.NewLine + Environment.NewLine +
               "ap_text_" + objectID + ":" + Environment.NewLine +
               // Add the frog ring to inventory (item(0)) so it can be carried to Jenny.
               // Counting is handled entirely by Jenny's turn-in (item(0)+1 -> var(71)),
               // so do NOT bump var(71)/var(147) here or rings get double-counted.
               // Check result so a full inventory doesn't falsely set the AP flag.
               "\tpushbp" + Environment.NewLine +
               "\tsetsp\t0.d" + Environment.NewLine +
               "\tlib\t77.w" + Environment.NewLine +
               "\tpopbp" + Environment.NewLine +
               "\tif\tresult, else *ap_text_" + objectID + "_skip" + Environment.NewLine +
               "\tmsg\tvoice(" + apItemVoice + ".b)," + Environment.NewLine +
               "\t\t\"You found " + player + "\'s\", " + Environment.NewLine +
               "\t\t\"" + " " + newObjectName + "\"," + Environment.NewLine +
               "\t\twait(254.b)" +
               flagSet +
               panDropAnim + Environment.NewLine +
               "ap_text_" + objectID + "_skip:" + Environment.NewLine +
               "\treturn\n" + Environment.NewLine);
            }
            else
            {
                File.AppendAllText(
                stagefile,
                "\t.interact  " + objectID + ".d, *ap_text_" + objectID + Environment.NewLine + Environment.NewLine +
                "ap_text_" + objectID + ":" + Environment.NewLine +
                "\tmsg\tvoice(" + apItemVoice + ".b)," + Environment.NewLine +
                "\t\t\"You found " + player + "\'s\", " + Environment.NewLine +
                "\t\t\"" + " " + newObjectName + "\"," + Environment.NewLine +
                "\t\twait(254.b)" +
                flagSet +
                panDropAnim + Environment.NewLine +
                "\treturn\n" + Environment.NewLine);
            }

        }

        private void generateAntiRespawnSubroutines()
        {
            var stageLabels = new Dictionary<string, string>
            {
                { "stage07_Edited.us", "07" },
                { "stage01_Edited.us", "01" },
                { "stage02_Edited.us", "02" },
                { "stage03_Edited.us", "03" },
                { "stage04_Edited.us", "04" },
                { "stage06_Edited.us", "06" },
                { "stage09_Edited.us", "09" },
            };

            foreach (var kvp in stageAntiRespawnLocs)
            {
                if (!stageLabels.ContainsKey(kvp.Key)) continue;

                string label = stageLabels[kvp.Key];
                string fullPath = Directory.GetCurrentDirectory() + @"\" + kvp.Key;
                var locs = kvp.Value;

                // Append the per-stage show+hide subroutines
                var sb = new System.Text.StringBuilder();

                // Show subroutine: make all AP items visible on room load (counters the
                // game engine hiding objects that have the "spawn" flag in their JSON flags array)
                sb.Append("\r\nsub_ap_show_" + label + ":");
                for (int i = 0; i < locs.Count; i++)
                {
                    int objID = locs[i].objID;
                    sb.Append("\r\n\tdisp\t" + objID + ".d, 1.d");
                }
                sb.Append("\r\n\treturn\r\n");

                // Hide subroutine: re-hide items whose collection flag is set
                sb.Append("\r\nsub_ap_hide_" + label + ":");
                for (int i = 0; i < locs.Count; i++)
                {
                    int flagNum = 2100 + locs[i].code;
                    int objID = locs[i].objID;
                    sb.Append("\r\n\tif\tflag(" + flagNum + ".d), else *loc_ah_" + label + "_" + i);
                    sb.Append("\r\n\tdisp\t" + objID + ".d, 0.d");
                    sb.Append("\r\nloc_ah_" + label + "_" + i + ":");
                }
                sb.Append("\r\n\treturn\r\n");
                File.AppendAllText(fullPath, sb.ToString());

                // Patch the END of evt_startup's day/night branch to call the hide sub.
                // That branch always ends with: set flag(31.d), 0.X  followed immediately by return.
                // We target only the FIRST occurrence so we land inside evt_startup, not evt_time_cycle.
                string content = File.ReadAllText(fullPath);

                string[] startupEndCandidates = new[]
                {
                    "\tset\tflag(31.d), 0.d\r\n\treturn",
                    "\tset\tflag(31.d), 0.w\r\n\treturn",
                    "\tset\tflag(31.d), 0.d\n\treturn",
                    "\tset\tflag(31.d), 0.w\n\treturn",
                };

                bool patched = false;
                foreach (string candidate in startupEndCandidates)
                {
                    int idx = content.IndexOf(candidate);
                    if (idx >= 0)
                    {
                        string insertion = "\r\n\trun\t*sub_ap_show_" + label + "\r\n\trun\t*sub_ap_hide_" + label;
                        // Insert run call before the return
                        int returnOffset = candidate.LastIndexOf("\treturn");
                        string newChunk = candidate.Substring(0, returnOffset) + insertion + "\r\n" + "\treturn";
                        content = content.Substring(0, idx) + newChunk + content.Substring(idx + candidate.Length);
                        patched = true;
                        break;
                    }
                }

                File.WriteAllText(fullPath, content);
            }
        }

        private void enableATCToolPickup(string stagefile, int objectID, string player, string newObjectName, int toolID)
        {
            if (toolID == 708)
            {
                File.AppendAllText(
                   stagefile,
                   "\t.interact  " + objectID + ".d, *ap_text_" + objectID + Environment.NewLine + Environment.NewLine +
                   "ap_text_" + objectID + ":" + Environment.NewLine +
                   "\tmsg\tvoice(" + apItemVoice + ".b)," + Environment.NewLine +
                   "\t\t\"" + "Found Beasement Teleporter" + "\"," + Environment.NewLine +
                   "\t\twait(254.b)" + Environment.NewLine +
                   "\tset\tvar(708.d), 2.d" + Environment.NewLine +
                   "\treturn\n" + Environment.NewLine);
            }
            else if (toolID == 707)
            {
                File.AppendAllText(
                   stagefile,
                   "\t.interact  " + objectID + ".d, *ap_text_" + objectID + Environment.NewLine + Environment.NewLine +
                   "ap_text_" + objectID + ":" + Environment.NewLine +
                   "\tmsg\tvoice(" + apItemVoice + ".b)," + Environment.NewLine +
                   "\t\t\"" + "Found Bedroom Bridge" + "\"," + Environment.NewLine +
                   "\t\twait(254.b)" + Environment.NewLine +
                   "\tset\tvar(707.d), 2.d" + Environment.NewLine +
                   "\treturn\n" + Environment.NewLine);
            }
            else if (toolID == 706)
            {
                File.AppendAllText(
                   stagefile,
                   "\t.interact  " + objectID + ".d, *ap_text_" + objectID + Environment.NewLine + Environment.NewLine +
                   "ap_text_" + objectID + ":" + Environment.NewLine +
                   "\tmsg\tvoice(" + apItemVoice + ".b)," + Environment.NewLine +
                   "\t\t\"" + "Found Kitchen Bridge" + "\"," + Environment.NewLine +
                   "\t\twait(254.b)" + Environment.NewLine +
                   "\tset\tvar(706.d), 2.d" + Environment.NewLine +
                   "\treturn\n" + Environment.NewLine);
            }
            else if (toolID == 705)
            {
                File.AppendAllText(
                   stagefile,
                   "\t.interact  " + objectID + ".d, *ap_text_" + objectID + Environment.NewLine + Environment.NewLine +
                   "ap_text_" + objectID + ":" + Environment.NewLine +
                   "\tmsg\tvoice(" + apItemVoice + ".b)," + Environment.NewLine +
                   "\t\t\"" + "Found Living Room Bridge" + "\"," + Environment.NewLine +
                   "\t\twait(254.b)" + Environment.NewLine +
                   "\tset\tvar(705.d), 2.d" + Environment.NewLine +
                   "\treturn\n" + Environment.NewLine);
            }
            else if (toolID == 704)
            {
                File.AppendAllText(
                   stagefile,
                   "\t.interact  " + objectID + ".d, *ap_text_" + objectID + Environment.NewLine + Environment.NewLine +
                   "ap_text_" + objectID + ":" + Environment.NewLine +
                   "\tmsg\tvoice(" + apItemVoice + ".b)," + Environment.NewLine +
                   "\t\t\"" + "Found Foyer Teleport" + "\"," + Environment.NewLine +
                   "\t\twait(254.b)" + Environment.NewLine +
                   "\tset\tvar(704.d), 2.d" + Environment.NewLine +
                   "\treturn\n" + Environment.NewLine);
            }
            else if (toolID == 703)
            {
                File.AppendAllText(
                   stagefile,
                   "\t.interact  " + objectID + ".d, *ap_text_" + objectID + Environment.NewLine + Environment.NewLine +
                   "ap_text_" + objectID + ":" + Environment.NewLine +
                   "\tmsg\tvoice(" + apItemVoice + ".b)," + Environment.NewLine +
                   "\t\t\"" + "Found Foyer Ladder" + "\"," + Environment.NewLine +
                   "\t\twait(254.b)" + Environment.NewLine +
                   "\tset\tvar(703.d), 2.d" + Environment.NewLine +
                   "\treturn\n" + Environment.NewLine);
            }
            else if (toolID == 702)
            {
                File.AppendAllText(
                   stagefile,
                   "\t.interact  " + objectID + ".d, *ap_text_" + objectID + Environment.NewLine + Environment.NewLine +
                   "ap_text_" + objectID + ":" + Environment.NewLine +
                   "\tmsg\tvoice(" + apItemVoice + ".b)," + Environment.NewLine +
                   "\t\t\"" + "Found Kitchen Ladder" + "\"," + Environment.NewLine +
                   "\t\twait(254.b)" + Environment.NewLine +
                   "\tset\tvar(702.d), 2.d" + Environment.NewLine +
                   "\treturn\n" + Environment.NewLine);
            }
            else if (toolID == 701)
            {
                File.AppendAllText(
                   stagefile,
                   "\t.interact  " + objectID + ".d, *ap_text_" + objectID + Environment.NewLine + Environment.NewLine +
                   "ap_text_" + objectID + ":" + Environment.NewLine +
                   "\tmsg\tvoice(" + apItemVoice + ".b)," + Environment.NewLine +
                   "\t\t\"" + "Found Living Room Ladder" + "\"," + Environment.NewLine +
                   "\t\twait(254.b)" + Environment.NewLine +
                   "\tset\tvar(701.d), 2.d" + Environment.NewLine +
                   "\treturn\n" + Environment.NewLine);
            }
            else if (toolID == 4)
            {
                File.AppendAllText(
                   stagefile,
                   "\t.interact  " + objectID + ".d, *ap_text_" + objectID + Environment.NewLine + Environment.NewLine +
                   "ap_text_" + objectID + ":" + Environment.NewLine +
                   "\tmsg\tvoice(" + apItemVoice + ".b)," + Environment.NewLine +
                   "\t\t\"" + "Increased Giga Charge" + "\"," + Environment.NewLine +
                   "\t\twait(254.b)" + Environment.NewLine +
                   "\tset\tvar(136.d),\tadd(var(136.d), 1000.w)" + Environment.NewLine +
                   "\tif\tge(var(136.d), 9000.w), else *loc_empty" + Environment.NewLine +
                   "\tset\tvar(136.d), 9000.w" + Environment.NewLine +
                   "\treturn\n" + Environment.NewLine);
            }
            else if (toolID == 3)
            {
                File.AppendAllText(
                   stagefile,
                   "\t.interact  " + objectID + ".d, *ap_text_" + objectID + Environment.NewLine + Environment.NewLine +
                   "ap_text_" + objectID + ":" + Environment.NewLine +
                  "\tmsg\tvoice(" + apItemVoice + ".b)," + Environment.NewLine +
                   "\t\t\"" + newObjectName + "\"," + Environment.NewLine +
                   "\t\twait(254.b)" + Environment.NewLine +
                   "\trun\t*loc_enable_radar" + Environment.NewLine +
                   "\treturn\n" + Environment.NewLine);
            }
            else if (toolID == 2)
            {
                File.AppendAllText(
                   stagefile,
                   "\t.interact  " + objectID + ".d, *ap_text_" + objectID + Environment.NewLine + Environment.NewLine +
                   "ap_text_" + objectID + ":" + Environment.NewLine +
                   "\tmsg\tvoice(" + apItemVoice + ".b)," + Environment.NewLine +
                   "\t\t\"" + newObjectName + "\"," + Environment.NewLine +
                   "\t\twait(254.b)" + Environment.NewLine +
                   "\trun\t*loc_enable_blaster" + Environment.NewLine +
                   "\tset\tflag(333.d), 1.w" + Environment.NewLine +
                   "\tset\tflag(335.d), 1.w" + Environment.NewLine +
                   "\treturn\n" + Environment.NewLine);
            }
            else if (toolID == 1)
            {
                File.AppendAllText(
                   stagefile,
                   "\t.interact  " + objectID + ".d, *ap_text_" + objectID + Environment.NewLine + Environment.NewLine +
                   "ap_text_" + objectID + ":" + Environment.NewLine +
                   "\tmsg\tvoice(" + apItemVoice + ".b)," + Environment.NewLine +
                   "\t\t\"" + newObjectName + "\"," + Environment.NewLine +
                   "\t\twait(254.b)" + Environment.NewLine +
                   "\trun\t*loc_enable_copter" + Environment.NewLine +
                   "\treturn\n" + Environment.NewLine);
            }

        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void openUpstairs_CheckedChanged_1(object sender, EventArgs e)
        {

        }
    }

    public static class Theme
    {
        // ---- Palette ----
        public static readonly Color Background = Color.FromArgb(210, 210, 210); // window background
        public static readonly Color Surface = Color.White;                   // panels / cards
        public static readonly Color InputFill = Color.FromArgb(246, 246, 246); // text boxes, combos
        public static readonly Color Border = Color.FromArgb(225, 225, 225);
        public static readonly Color BorderStrong = Color.FromArgb(208, 208, 208);
        public static readonly Color TextPrimary = Color.FromArgb(26, 26, 26);
        public static readonly Color TextMuted = Color.FromArgb(136, 136, 136);
        public static readonly Color Accent = Color.FromArgb(216, 90, 48); // Chibi-Robo coral
        public static readonly Color AccentHover = Color.FromArgb(193, 74, 36);
        public static readonly Color AccentText = Color.White;

        // ---- Fonts ----
        public static readonly Font TitleFont = new Font("Segoe UI", 14F, FontStyle.Regular);
        public static readonly Font SectionFont = new Font("Segoe UI", 8.25F, FontStyle.Bold);
        public static readonly Font BodyFont = new Font("Segoe UI", 9F, FontStyle.Regular);
        public static readonly Font ButtonFont = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        public static readonly Font MonoFont = new Font("Consolas", 9F, FontStyle.Regular);

        public static void Apply(Form form)
        {
            form.BackColor = Background;
            form.ForeColor = TextPrimary;
            form.Font = BodyFont;
            StyleChildren(form);
        }

        private static void StyleChildren(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                switch (c)
                {
                    // Skip our own custom controls — they paint themselves.
                    case FlatButton _:
                    case FlatProgressBar _:
                    case SectionPanel _:
                    case FlatComboBox _:
                    case FlatCheckBox _:
                        break;
                    case Button b: StyleSecondaryButton(b); break;
                    case TextBox t: StyleInput(t); break;
                    case ComboBox cb: StyleCombo(cb); break;
                    case CheckBox ck: StyleCheck(ck); break;
                    case Label lb: lb.ForeColor = TextPrimary; lb.Font = BodyFont; break;
                }

                // FlatTextBox owns its inner TextBox's styling — do NOT recurse into it,
                // or the inner box gets a FixedSingle border applied over its borderless setup.
                if (c is FlatTextBox) continue;

                if (c.HasChildren) StyleChildren(c);
            }
        }

        // ---- Stock-control stylers (use these if you don't swap in the custom controls) ----

        public static void StyleSecondaryButton(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderColor = BorderStrong;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.MouseOverBackColor = InputFill;
            b.BackColor = Surface;
            b.ForeColor = TextPrimary;
            b.Font = ButtonFont;
            b.Cursor = Cursors.Hand;
        }

        public static void StylePrimaryButton(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = AccentHover;
            b.BackColor = Accent;
            b.ForeColor = AccentText;
            b.Font = new Font("Segoe UI", 11F);
            b.Cursor = Cursors.Hand;
        }

        public static void StyleInput(TextBox t)
        {
            t.BorderStyle = BorderStyle.FixedSingle;
            t.BackColor = InputFill;
            t.ForeColor = TextPrimary;
            t.Font = BodyFont;
        }

        public static void StyleCombo(ComboBox cb)
        {
            cb.FlatStyle = FlatStyle.Flat;
            cb.BackColor = InputFill;
            cb.ForeColor = TextPrimary;
            cb.Font = BodyFont;
        }

        public static void StyleCheck(CheckBox ck)
        {
            ck.Font = BodyFont;
            ck.ForeColor = TextPrimary;
        }


        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        private const int EM_SETCUEBANNER = 0x1501;

        /// <summary>
        /// Show grey placeholder text inside a TextBox until the user types.
        /// Clears any existing Text first (placeholders only show when empty).
        /// Note: does not work on multiline TextBoxes — Win32 limitation.
        /// </summary>
        public static void SetPlaceholder(TextBox t, string text)
        {
            t.Text = string.Empty;
            // wParam = 1 keeps the cue visible even when focused, until the user types.
            SendMessage(t.Handle, EM_SETCUEBANNER, (IntPtr)1, text);
        }

        /// <summary>Build a rounded-rectangle path. Shared by the custom controls.</summary>
        public static GraphicsPath RoundedRect(Rectangle r, int radius)
        {
            int d = Math.Max(1, radius * 2);
            var path = new GraphicsPath();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }

    // ---- Custom-painted flat button (anti-aliased rounded corners) ----
    public class FlatButton : Button
    {
        public bool Primary { get; set; } = false;
        public int Radius { get; set; } = 8;
        private bool _hover;

        public FlatButton()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint
                   | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.Transparent;
            Cursor = Cursors.Hand;
            Font = Theme.ButtonFont;
        }

        protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? Theme.Background);

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            Color text;
            using (var path = Theme.RoundedRect(rect, Radius))
            {
                if (Primary)
                {
                    using (var b = new SolidBrush(_hover ? Theme.AccentHover : Theme.Accent))
                        g.FillPath(b, path);
                    text = Theme.AccentText;
                }
                else
                {
                    using (var b = new SolidBrush(_hover ? Theme.InputFill : Theme.Surface))
                        g.FillPath(b, path);
                    using (var pen = new Pen(Theme.BorderStrong, 1))
                        g.DrawPath(pen, path);
                    text = Theme.TextPrimary;
                }
            }

            TextRenderer.DrawText(g, Text, Font, rect, text,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }
    }

    public class FlatProgressBar : Control
    {
        private int _value;
        private float _offset;          // current scroll position of the stripes
        private int _frame;             // current walker animation frame
        private int _frameTick;         // counts timer ticks between frame advances
        private readonly Timer _timer;

        public int Minimum { get; set; } = 0;
        public int Maximum { get; set; } = 100;
        public Color TrackColor { get; set; } = Color.FromArgb(120, 120, 120); // mid gray track
        public Color FillColor { get; set; } = Color.FromArgb(124, 179, 66); // base green
        public Color StripeColor { get; set; } = Color.FromArgb(168, 214, 102); // lighter green stripes
        public int StripeWidth { get; set; } = 10;  // px per light+dark stripe pair
        public bool Animate { get; set; } = true;
        public int TrackHeight { get; set; } = 14;  // bar thickness; rest of Height is walker space

        // ---- Walker sprite ----
        public Image WalkerImage { get; set; }       // sprite or sprite sheet (null = no walker)
        public int WalkerFrames { get; set; } = 1;  // frames in a horizontal sprite sheet
        public int WalkerTicksPerFrame { get; set; } = 5; // animation speed (higher = slower)
        public float WalkerScale { get; set; } = 1.0f; // >1 enlarges the sprite (needs taller control)

        public int Value
        {
            get => _value;
            set
            {
                // Self-marshal so callers can set Value from a background thread.
                if (InvokeRequired)
                {
                    try { BeginInvoke((Action)(() => Value = value)); } catch { }
                    return;
                }
                _value = Math.Max(Minimum, Math.Min(Maximum, value));
                Invalidate();
            }
        }

        public FlatProgressBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint
                   | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Height = 48; // room for a walker above a 14px track
            _timer = new Timer { Interval = 40 }; // ~25 fps
            _timer.Tick += (s, e) =>
            {
                if (!Animate) return;
                _offset += 1.2f; // stripes travel to the right
                if (_offset >= StripeWidth * 2) _offset = 0;

                if (WalkerImage != null && WalkerFrames > 1 && _value > Minimum)
                {
                    if (++_frameTick >= WalkerTicksPerFrame)
                    {
                        _frameTick = 0;
                        _frame = (_frame + 1) % WalkerFrames;
                    }
                }
                if (_value > Minimum) Invalidate();
            };
            _timer.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _timer?.Dispose();
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? Theme.Background);

            int th = Math.Min(TrackHeight, Height);
            int trackTop = Height - th;          // track sits at the bottom; walker space above
            int r = th / 2;
            var full = new Rectangle(0, trackTop, Width - 1, th - 1);

            // Track.
            using (var path = Theme.RoundedRect(full, r))
            using (var b = new SolidBrush(TrackColor))
                g.FillPath(b, path);

            double frac = Maximum > Minimum ? (double)(_value - Minimum) / (Maximum - Minimum) : 0;
            int w = (int)(frac * (Width - 1));

            // Filled portion (clipped to a rounded rect so it stays inside the track).
            if (w >= 1)
            {
                int fillW = Math.Max(w, Math.Min(th, Width - 1)); // keep a rounded cap visible
                var fillRect = new Rectangle(0, trackTop, fillW, th - 1);

                using (var clip = Theme.RoundedRect(fillRect, r))
                {
                    var oldClip = g.Clip;
                    g.SetClip(clip, CombineMode.Replace);

                    using (var b = new SolidBrush(FillColor))
                        g.FillRectangle(b, fillRect);

                    using (var pen = new Pen(StripeColor, StripeWidth * 0.6f))
                    {
                        float step = StripeWidth * 2f;
                        for (float x = _offset - th; x < fillW + th; x += step)
                            g.DrawLine(pen, x, trackTop + th, x + th, trackTop);
                    }

                    g.Clip = oldClip;
                }
            }

            // Walker sprite: rests on top of the track, horizontally at the fill edge.
            DrawWalker(g, w, trackTop);
        }

        private void DrawWalker(Graphics g, int fillW, int trackTop)
        {
            if (WalkerImage == null) return;
            if (_value <= Minimum || _value >= Maximum) return; // hide at empty/full

            int frames = Math.Max(1, WalkerFrames);
            int srcW = WalkerImage.Width / frames;
            int srcH = WalkerImage.Height;
            if (srcW <= 0 || srcH <= 0) return;

            int walkerSpace = trackTop;                 // available height above the track
            if (walkerSpace < 4) return;

            // Scale the sprite to fit the space above the bar (keep aspect ratio),
            // then apply the optional WalkerScale multiplier.
            float scale = (float)walkerSpace / srcH * (WalkerScale <= 0 ? 1f : WalkerScale);
            int destW = Math.Max(1, (int)(srcW * scale));
            int destH = Math.Max(1, (int)(srcH * scale));

            // Center the walker on the fill edge, clamped to the control bounds.
            int x = fillW - destW / 2;
            if (x < 0) x = 0;
            if (x > Width - destW) x = Width - destW;
            int y = trackTop - destH;                   // bottom of sprite touches top of track

            var srcFrame = new Rectangle(_frame * srcW, 0, srcW, srcH);
            var dest = new Rectangle(x, y, destW, destH);

            var oldInterp = g.InterpolationMode;
            g.InterpolationMode = InterpolationMode.NearestNeighbor; // crisp for pixel sprites
            g.DrawImage(WalkerImage, dest, srcFrame, GraphicsUnit.Pixel);
            g.InterpolationMode = oldInterp;
        }
    }

    // ---- Bordered card with a small header label (the "GroupBox" replacement) ----
    public class SectionPanel : Panel
    {
        public string Header { get; set; } = "";

        public SectionPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint
                   | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            BackColor = Theme.Surface; // so child controls (checkboxes, buttons) clear against white
            Padding = new Padding(14, 32, 14, 14); // leave room at top for the header
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? Theme.Background);

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = Theme.RoundedRect(rect, 8))
            {
                using (var b = new SolidBrush(Theme.Surface))
                    g.FillPath(b, path);
                using (var pen = new Pen(Theme.Border, 1))
                    g.DrawPath(pen, path);
            }

            if (!string.IsNullOrEmpty(Header))
                TextRenderer.DrawText(g, Header, Theme.SectionFont,
                    new Point(14, 11), Theme.TextMuted);
        }
    }

    // ---- Flat combo box: paints over the system dropdown button with a
    //      matching fill + chevron, plus a 1px flat border. Works for both
    //      DropDown and DropDownList styles. ----
    public class FlatComboBox : ComboBox
    {
        public Color BorderColor { get; set; } = Theme.BorderStrong;
        public Color ArrowColor { get; set; } = Theme.TextMuted;
        public int Radius { get; set; } = 6;

        private const int WM_PAINT = 0x000F;
        private const int ButtonWidth = 22;

        public FlatComboBox()
        {
            FlatStyle = FlatStyle.Flat;
            BackColor = Theme.InputFill;
            ForeColor = Theme.TextPrimary;
            Font = Theme.BodyFont;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            ApplyRoundedRegion();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ApplyRoundedRegion();
        }

        private void ApplyRoundedRegion()
        {
            using (var path = Theme.RoundedRect(new Rectangle(0, 0, Width, Height), Radius))
                Region = new Region(path);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_PAINT)
            {
                using (var g = Graphics.FromHwnd(Handle))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    // Cover the system arrow button with our fill colour.
                    var btn = new Rectangle(Width - ButtonWidth, 1, ButtonWidth - 2, Height - 2);
                    using (var b = new SolidBrush(BackColor))
                        g.FillRectangle(b, btn);

                    // Flat chevron.
                    int cx = Width - ButtonWidth / 2 - 2;
                    int cy = Height / 2;
                    using (var pen = new Pen(ArrowColor, 1.6f))
                    {
                        g.DrawLine(pen, cx - 4, cy - 2, cx, cy + 2);
                        g.DrawLine(pen, cx, cy + 2, cx + 4, cy - 2);
                    }

                    // Rounded border to match the text boxes.
                    var rect = new Rectangle(0, 0, Width - 1, Height - 1);
                    using (var path = Theme.RoundedRect(rect, Radius))
                    using (var pen = new Pen(BorderColor))
                        g.DrawPath(pen, path);
                }
            }
        }
    }

    // ---- Flat check box: white box with a coral tick when checked. ----
    public class FlatCheckBox : CheckBox
    {
        public Color BoxFill { get; set; } = Theme.Surface;
        public Color BoxBorder { get; set; } = Theme.BorderStrong;
        public Color CheckColor { get; set; } = Theme.Accent;
        public int BoxSize { get; set; } = 16;

        public FlatCheckBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint
                   | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            Appearance = Appearance.Normal;
            Font = Theme.BodyFont;
            ForeColor = Theme.TextPrimary;
            Cursor = Cursors.Hand;
            AutoSize = false;            // we size ourselves via FitWidth()
            Height = 22;
            FitWidth();
        }

        // Width the label actually needs: box + gap + measured text + padding.
        private void FitWidth()
        {
            int textW = TextRenderer.MeasureText(Text ?? string.Empty, Font).Width;
            Width = BoxSize + 8 + textW + 6;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            FitWidth();
            Invalidate();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            FitWidth();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? Theme.Surface);

            int box = BoxSize;
            int top = (Height - box) / 2;
            var rect = new Rectangle(0, top, box, box);

            // White box with a border (border picks up the accent when checked).
            using (var path = Theme.RoundedRect(rect, 3))
            {
                using (var b = new SolidBrush(BoxFill))
                    g.FillPath(b, path);
                using (var pen = new Pen(Checked ? CheckColor : BoxBorder, 1))
                    g.DrawPath(pen, path);
            }

            // Coral tick.
            if (Checked)
            {
                using (var pen = new Pen(CheckColor, 2f)
                {
                    StartCap = LineCap.Round,
                    EndCap = LineCap.Round,
                    LineJoin = LineJoin.Round
                })
                {
                    float x = rect.X, y = rect.Y;
                    g.DrawLines(pen, new[]
                    {
                     new PointF(x + box * 0.24f, y + box * 0.52f),
                     new PointF(x + box * 0.43f, y + box * 0.70f),
                     new PointF(x + box * 0.76f, y + box * 0.30f)
                 });
                }
            }

            // Label.
            var textRect = new Rectangle(box + 8, 0, Width - box - 8, Height);
            TextRenderer.DrawText(g, Text, Font, textRect, ForeColor,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }
    }

    // ---- Read-only log console: a RichTextBox that never shows the caret,
    //      so it reads as an output panel rather than an editable field. ----
    public class LogBox : RichTextBox
    {
        [DllImport("user32.dll")]
        private static extern bool HideCaret(IntPtr hWnd);

        public LogBox()
        {
            ReadOnly = true;
            TabStop = false;
            BorderStyle = BorderStyle.None;
            BackColor = Theme.Surface;
            ForeColor = Theme.TextPrimary;
            Font = Theme.MonoFont;
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            HideCaret(Handle);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            HideCaret(Handle); // re-assert after every message so it never flickers back
        }
    }

    // ---- Flat rounded text box: a borderless TextBox hosted inside a
    //      custom-painted rounded container, matching the card styling. ----
    public class FlatTextBox : Panel
    {
        public TextBox Inner { get; }
        public int Radius { get; set; } = 6;

        // Passthroughs so existing code that uses .Text keeps working.
        public override string Text
        {
            get => Inner.Text;
            set => Inner.Text = value;
        }

        public FlatTextBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint
                   | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);

            Inner = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = Theme.InputFill,
                ForeColor = Theme.TextPrimary,
                Font = Theme.BodyFont,
                Dock = DockStyle.Fill
            };
            Controls.Add(Inner);

            BackColor = Theme.Surface;
            Padding = new Padding(10, 10, 10, 10);
            Height = 28;   // any resize this triggers now finds Inner already set
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (Inner == null) return;
            Inner.Left = Padding.Left;
            Inner.Width = Width - Padding.Left - Padding.Right;
            Inner.Top = (Height - Inner.Height) / 2;   // now respected — no Dock fighting it
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Parent?.BackColor ?? Theme.Surface);

            var rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (var path = Theme.RoundedRect(rect, Radius))
            {
                using (var b = new SolidBrush(Inner.BackColor))   // exact same fill as the textbox
                    g.FillPath(b, path);
                using (var pen = new Pen(Theme.BorderStrong, 1))
                    g.DrawPath(pen, path);
            }
        }
    }
}
