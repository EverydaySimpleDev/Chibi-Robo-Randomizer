namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.isoFilePath = new System.Windows.Forms.TextBox();
            this.title = new System.Windows.Forms.Label();
            this.openISO = new System.Windows.Forms.Button();
            this.openDestination = new System.Windows.Forms.Button();
            this.destinationPath = new System.Windows.Forms.TextBox();
            this.settingsLabel = new System.Windows.Forms.Label();
            this.logicSettings = new System.Windows.Forms.ComboBox();
            this.freePJ = new System.Windows.Forms.CheckBox();
            this.openUpstairs = new System.Windows.Forms.CheckBox();
            this.seedLabel = new System.Windows.Forms.Label();
            this.seed = new System.Windows.Forms.TextBox();
            this.statusDialog = new System.Windows.Forms.RichTextBox();
            this.randomizeButton = new System.Windows.Forms.Button();
            this.batteryCharge = new System.Windows.Forms.CheckBox();
            this.openDownstairs = new System.Windows.Forms.CheckBox();
            this.passwordRando = new System.Windows.Forms.CheckBox();
            this.chibiVision = new System.Windows.Forms.CheckBox();
            this.Chibi_Robo_Icon = new System.Windows.Forms.PictureBox();
            this.PBar = new System.Windows.Forms.ProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.walkingBatteryDrain = new System.Windows.Forms.CheckBox();
            this.joggingBatteryDrain = new System.Windows.Forms.CheckBox();
            this.runningDecreasesBattery = new System.Windows.Forms.CheckBox();
            this.apZipPath = new System.Windows.Forms.TextBox();
            this.openAPZip = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Chibi_Robo_Icon)).BeginInit();
            this.SuspendLayout();
            // 
            // isoFilePath
            // 
            this.isoFilePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.isoFilePath.Location = new System.Drawing.Point(123, 60);
            this.isoFilePath.Name = "isoFilePath";
            this.isoFilePath.ReadOnly = true;
            this.isoFilePath.Size = new System.Drawing.Size(490, 26);
            this.isoFilePath.TabIndex = 0;
            this.isoFilePath.Text = "<- Select path to Chibi-Robo NTSC-U ISO";
            this.isoFilePath.TextChanged += new System.EventHandler(this.isoFilePath_TextChanged);
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(17, 9);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(245, 26);
            this.title.TabIndex = 2;
            this.title.Text = "Chibi-Robo Randomizer";
            this.title.Click += new System.EventHandler(this.title_Click);
            // 
            // openISO
            // 
            this.openISO.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openISO.Location = new System.Drawing.Point(17, 56);
            this.openISO.Name = "openISO";
            this.openISO.Size = new System.Drawing.Size(100, 35);
            this.openISO.TabIndex = 3;
            this.openISO.Text = "Open ISO";
            this.openISO.UseVisualStyleBackColor = true;
            this.openISO.Click += new System.EventHandler(this.openISO_Click);
            // 
            // openDestination
            // 
            this.openDestination.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openDestination.Location = new System.Drawing.Point(17, 97);
            this.openDestination.Name = "openDestination";
            this.openDestination.Size = new System.Drawing.Size(100, 35);
            this.openDestination.TabIndex = 4;
            this.openDestination.Text = "Browse";
            this.openDestination.UseVisualStyleBackColor = true;
            this.openDestination.Click += new System.EventHandler(this.openDestination_Click);
            // 
            // destinationPath
            // 
            this.destinationPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.destinationPath.Location = new System.Drawing.Point(123, 101);
            this.destinationPath.Name = "destinationPath";
            this.destinationPath.ReadOnly = true;
            this.destinationPath.Size = new System.Drawing.Size(490, 26);
            this.destinationPath.TabIndex = 5;
            this.destinationPath.Text = "<- Set destination path";
            // 
            // settingsLabel
            // 
            this.settingsLabel.AutoSize = true;
            this.settingsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsLabel.Location = new System.Drawing.Point(13, 201);
            this.settingsLabel.Name = "settingsLabel";
            this.settingsLabel.Size = new System.Drawing.Size(107, 20);
            this.settingsLabel.TabIndex = 7;
            this.settingsLabel.Text = "Mode / Logic: ";
            this.settingsLabel.Click += new System.EventHandler(this.settingsLabel_Click);
            // 
            // logicSettings
            // 
            this.logicSettings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.logicSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.logicSettings.FormattingEnabled = true;
            this.logicSettings.Items.AddRange(new object[] {
            "Glitchless",
            "Glitched",
            "No Logic"});
            this.logicSettings.Location = new System.Drawing.Point(118, 198);
            this.logicSettings.Name = "logicSettings";
            this.logicSettings.Size = new System.Drawing.Size(121, 28);
            this.logicSettings.TabIndex = 8;
            // 
            // freePJ
            // 
            this.freePJ.AutoSize = true;
            this.freePJ.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.freePJ.Location = new System.Drawing.Point(648, 56);
            this.freePJ.Name = "freePJ";
            this.freePJ.Size = new System.Drawing.Size(91, 24);
            this.freePJ.TabIndex = 10;
            this.freePJ.Text = "Free PJs";
            this.freePJ.UseVisualStyleBackColor = true;
            this.freePJ.CheckedChanged += new System.EventHandler(this.freePJ_CheckedChanged);
            // 
            // openUpstairs
            // 
            this.openUpstairs.AutoSize = true;
            this.openUpstairs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openUpstairs.Location = new System.Drawing.Point(804, 56);
            this.openUpstairs.Name = "openUpstairs";
            this.openUpstairs.Size = new System.Drawing.Size(130, 24);
            this.openUpstairs.TabIndex = 11;
            this.openUpstairs.Text = "Open Upstairs";
            this.openUpstairs.UseVisualStyleBackColor = true;
            this.openUpstairs.CheckedChanged += new System.EventHandler(this.openUpstairs_CheckedChanged);
            // 
            // seedLabel
            // 
            this.seedLabel.AutoSize = true;
            this.seedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.seedLabel.Location = new System.Drawing.Point(259, 198);
            this.seedLabel.Name = "seedLabel";
            this.seedLabel.Size = new System.Drawing.Size(51, 20);
            this.seedLabel.TabIndex = 12;
            this.seedLabel.Text = "Seed:";
            this.seedLabel.Click += new System.EventHandler(this.seed_Click);
            // 
            // seed
            // 
            this.seed.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.seed.Location = new System.Drawing.Point(316, 198);
            this.seed.Name = "seed";
            this.seed.Size = new System.Drawing.Size(121, 26);
            this.seed.TabIndex = 13;
            // 
            // statusDialog
            // 
            this.statusDialog.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusDialog.Location = new System.Drawing.Point(12, 384);
            this.statusDialog.Name = "statusDialog";
            this.statusDialog.ReadOnly = true;
            this.statusDialog.Size = new System.Drawing.Size(1130, 155);
            this.statusDialog.TabIndex = 14;
            this.statusDialog.Text = "";
            // 
            // randomizeButton
            // 
            this.randomizeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.randomizeButton.Location = new System.Drawing.Point(12, 329);
            this.randomizeButton.Name = "randomizeButton";
            this.randomizeButton.Size = new System.Drawing.Size(161, 49);
            this.randomizeButton.TabIndex = 15;
            this.randomizeButton.Text = "Randomize";
            this.randomizeButton.UseVisualStyleBackColor = true;
            this.randomizeButton.Click += new System.EventHandler(this.randomizeButton_Click);
            // 
            // batteryCharge
            // 
            this.batteryCharge.AutoSize = true;
            this.batteryCharge.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.batteryCharge.Location = new System.Drawing.Point(960, 56);
            this.batteryCharge.Name = "batteryCharge";
            this.batteryCharge.Size = new System.Drawing.Size(182, 24);
            this.batteryCharge.TabIndex = 16;
            this.batteryCharge.Text = "Charged Giga Battery";
            this.batteryCharge.UseVisualStyleBackColor = true;
            this.batteryCharge.CheckedChanged += new System.EventHandler(this.batteryCharge_CheckedChanged);
            // 
            // openDownstairs
            // 
            this.openDownstairs.AutoSize = true;
            this.openDownstairs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openDownstairs.Location = new System.Drawing.Point(648, 97);
            this.openDownstairs.Name = "openDownstairs";
            this.openDownstairs.Size = new System.Drawing.Size(150, 24);
            this.openDownstairs.TabIndex = 17;
            this.openDownstairs.Text = "Open Downstairs";
            this.openDownstairs.UseVisualStyleBackColor = true;
            this.openDownstairs.CheckedChanged += new System.EventHandler(this.openDownstairs_CheckedChanged);
            // 
            // passwordRando
            // 
            this.passwordRando.AutoSize = true;
            this.passwordRando.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.passwordRando.Location = new System.Drawing.Point(960, 97);
            this.passwordRando.Name = "passwordRando";
            this.passwordRando.Size = new System.Drawing.Size(182, 24);
            this.passwordRando.TabIndex = 18;
            this.passwordRando.Text = "Randomize Password";
            this.passwordRando.UseVisualStyleBackColor = true;
            this.passwordRando.CheckedChanged += new System.EventHandler(this.passwordRando_CheckedChanged);
            // 
            // chibiVision
            // 
            this.chibiVision.AutoSize = true;
            this.chibiVision.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chibiVision.Location = new System.Drawing.Point(804, 97);
            this.chibiVision.Name = "chibiVision";
            this.chibiVision.Size = new System.Drawing.Size(137, 24);
            this.chibiVision.TabIndex = 19;
            this.chibiVision.Text = "Chibi-Vision Off";
            this.chibiVision.UseVisualStyleBackColor = true;
            // 
            // Chibi_Robo_Icon
            // 
            this.Chibi_Robo_Icon.Image = global::ChibiRoboRando.Properties.Resources.chibi_body_icon;
            this.Chibi_Robo_Icon.Location = new System.Drawing.Point(285, 4);
            this.Chibi_Robo_Icon.Name = "Chibi_Robo_Icon";
            this.Chibi_Robo_Icon.Size = new System.Drawing.Size(100, 50);
            this.Chibi_Robo_Icon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Chibi_Robo_Icon.TabIndex = 20;
            this.Chibi_Robo_Icon.TabStop = false;
            this.Chibi_Robo_Icon.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // PBar
            // 
            this.PBar.Location = new System.Drawing.Point(182, 343);
            this.PBar.Name = "PBar";
            this.PBar.Size = new System.Drawing.Size(960, 23);
            this.PBar.TabIndex = 21;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // walkingBatteryDrain
            // 
            this.walkingBatteryDrain.AutoSize = true;
            this.walkingBatteryDrain.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.walkingBatteryDrain.Location = new System.Drawing.Point(648, 140);
            this.walkingBatteryDrain.Name = "walkingBatteryDrain";
            this.walkingBatteryDrain.Size = new System.Drawing.Size(276, 24);
            this.walkingBatteryDrain.TabIndex = 25;
            this.walkingBatteryDrain.Text = "Walking Doesnt Decreases Battery";
            this.walkingBatteryDrain.UseVisualStyleBackColor = true;
            this.walkingBatteryDrain.Visible = false;
            this.walkingBatteryDrain.CheckedChanged += new System.EventHandler(this.walkingBatteryDrain_CheckedChanged);
            // 
            // joggingBatteryDrain
            // 
            this.joggingBatteryDrain.AutoSize = true;
            this.joggingBatteryDrain.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.joggingBatteryDrain.Location = new System.Drawing.Point(648, 170);
            this.joggingBatteryDrain.Name = "joggingBatteryDrain";
            this.joggingBatteryDrain.Size = new System.Drawing.Size(276, 24);
            this.joggingBatteryDrain.TabIndex = 26;
            this.joggingBatteryDrain.Text = "Jogging Doesnt Decreases Battery";
            this.joggingBatteryDrain.UseVisualStyleBackColor = true;
            this.joggingBatteryDrain.Visible = false;
            // 
            // runningDecreasesBattery
            // 
            this.runningDecreasesBattery.AutoSize = true;
            this.runningDecreasesBattery.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runningDecreasesBattery.Location = new System.Drawing.Point(648, 200);
            this.runningDecreasesBattery.Name = "runningDecreasesBattery";
            this.runningDecreasesBattery.Size = new System.Drawing.Size(280, 24);
            this.runningDecreasesBattery.TabIndex = 27;
            this.runningDecreasesBattery.Text = "Running Doesnt Decreases Battery";
            this.runningDecreasesBattery.UseVisualStyleBackColor = true;
            this.runningDecreasesBattery.Visible = false;
            // 
            // apZipPath
            // 
            this.apZipPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.apZipPath.Location = new System.Drawing.Point(123, 142);
            this.apZipPath.Name = "apZipPath";
            this.apZipPath.ReadOnly = true;
            this.apZipPath.Size = new System.Drawing.Size(490, 26);
            this.apZipPath.TabIndex = 31;
            this.apZipPath.Text = "<- Set Archipelago Data To Enable Integration (Optional)";
            // 
            // openAPZip
            // 
            this.openAPZip.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openAPZip.Location = new System.Drawing.Point(17, 138);
            this.openAPZip.Name = "openAPZip";
            this.openAPZip.Size = new System.Drawing.Size(100, 35);
            this.openAPZip.TabIndex = 30;
            this.openAPZip.Text = "Open AP";
            this.openAPZip.UseVisualStyleBackColor = true;
            this.openAPZip.Click += new System.EventHandler(this.openAPZip_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1154, 551);
            this.Controls.Add(this.apZipPath);
            this.Controls.Add(this.openAPZip);
            this.Controls.Add(this.runningDecreasesBattery);
            this.Controls.Add(this.joggingBatteryDrain);
            this.Controls.Add(this.walkingBatteryDrain);
            this.Controls.Add(this.PBar);
            this.Controls.Add(this.Chibi_Robo_Icon);
            this.Controls.Add(this.chibiVision);
            this.Controls.Add(this.passwordRando);
            this.Controls.Add(this.openDownstairs);
            this.Controls.Add(this.batteryCharge);
            this.Controls.Add(this.randomizeButton);
            this.Controls.Add(this.statusDialog);
            this.Controls.Add(this.seed);
            this.Controls.Add(this.seedLabel);
            this.Controls.Add(this.openUpstairs);
            this.Controls.Add(this.freePJ);
            this.Controls.Add(this.logicSettings);
            this.Controls.Add(this.settingsLabel);
            this.Controls.Add(this.destinationPath);
            this.Controls.Add(this.openDestination);
            this.Controls.Add(this.openISO);
            this.Controls.Add(this.title);
            this.Controls.Add(this.isoFilePath);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Chibi-Robo Randomizer";
            ((System.ComponentModel.ISupportInitialize)(this.Chibi_Robo_Icon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox isoFilePath;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Button openISO;
        private System.Windows.Forms.Button openDestination;
        private System.Windows.Forms.TextBox destinationPath;
        private System.Windows.Forms.Label settingsLabel;
        private System.Windows.Forms.ComboBox logicSettings;
        private System.Windows.Forms.CheckBox freePJ;
        private System.Windows.Forms.CheckBox openUpstairs;
        private System.Windows.Forms.Label seedLabel;
        private System.Windows.Forms.TextBox seed;
        private System.Windows.Forms.RichTextBox statusDialog;
        private System.Windows.Forms.Button randomizeButton;
        private System.Windows.Forms.CheckBox batteryCharge;
        private System.Windows.Forms.CheckBox openDownstairs;
        private System.Windows.Forms.CheckBox passwordRando;
        private System.Windows.Forms.CheckBox chibiVision;
        private System.Windows.Forms.PictureBox Chibi_Robo_Icon;
        private System.Windows.Forms.ProgressBar PBar;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox walkingBatteryDrain;
        private System.Windows.Forms.CheckBox joggingBatteryDrain;
        private System.Windows.Forms.CheckBox runningDecreasesBattery;
        private System.Windows.Forms.TextBox apZipPath;
        private System.Windows.Forms.Button openAPZip;
    }
}

