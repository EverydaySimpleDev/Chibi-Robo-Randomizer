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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.logicSettings = new WindowsFormsApp1.FlatComboBox();
            this.settingsLabel = new System.Windows.Forms.Label();
            this.seedLabel = new System.Windows.Forms.Label();
            this.sectionPanel4 = new WindowsFormsApp1.SectionPanel();
            this.openAPZip = new WindowsFormsApp1.FlatButton();
            this.openDestination = new WindowsFormsApp1.FlatButton();
            this.openISO = new WindowsFormsApp1.FlatButton();
            this.sectionPanel2 = new WindowsFormsApp1.SectionPanel();
            this.optionsButton = new WindowsFormsApp1.FlatButton();
            this.PBar = new WindowsFormsApp1.FlatProgressBar();
            this.randomizeButton = new WindowsFormsApp1.FlatButton();
            this.statusDialog = new WindowsFormsApp1.LogBox();
            this.isoFilePath = new WindowsFormsApp1.FlatTextBox();
            this.destinationPath = new WindowsFormsApp1.FlatTextBox();
            this.apZipPath = new WindowsFormsApp1.FlatTextBox();
            this.seed = new WindowsFormsApp1.FlatTextBox();
            this.sectionPanel4.SuspendLayout();
            this.sectionPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // logicSettings
            // 
            this.logicSettings.ArrowColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            this.logicSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.logicSettings.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.logicSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.logicSettings.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.logicSettings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.logicSettings.FormattingEnabled = true;
            this.logicSettings.Items.AddRange(new object[] {
            "AP Logic"});
            this.logicSettings.Location = new System.Drawing.Point(149, 54);
            this.logicSettings.Name = "logicSettings";
            this.logicSettings.Size = new System.Drawing.Size(121, 23);
            this.logicSettings.TabIndex = 14;
            // 
            // settingsLabel
            // 
            this.settingsLabel.AutoSize = true;
            this.settingsLabel.BackColor = System.Drawing.Color.Transparent;
            this.settingsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsLabel.Location = new System.Drawing.Point(13, 52);
            this.settingsLabel.Name = "settingsLabel";
            this.settingsLabel.Size = new System.Drawing.Size(99, 20);
            this.settingsLabel.TabIndex = 7;
            this.settingsLabel.Text = "Mode / Logic";
            this.settingsLabel.Click += new System.EventHandler(this.settingsLabel_Click);
            // 
            // seedLabel
            // 
            this.seedLabel.AutoSize = true;
            this.seedLabel.BackColor = System.Drawing.Color.Transparent;
            this.seedLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.seedLabel.Location = new System.Drawing.Point(17, 93);
            this.seedLabel.Name = "seedLabel";
            this.seedLabel.Size = new System.Drawing.Size(47, 20);
            this.seedLabel.TabIndex = 12;
            this.seedLabel.Text = "Seed";
            this.seedLabel.Click += new System.EventHandler(this.seed_Click);
            // 
            // sectionPanel4
            // 
            this.sectionPanel4.BackColor = System.Drawing.Color.White;
            this.sectionPanel4.Controls.Add(this.apZipPath);
            this.sectionPanel4.Controls.Add(this.destinationPath);
            this.sectionPanel4.Controls.Add(this.isoFilePath);
            this.sectionPanel4.Controls.Add(this.openAPZip);
            this.sectionPanel4.Controls.Add(this.openDestination);
            this.sectionPanel4.Controls.Add(this.openISO);
            this.sectionPanel4.Header = "Game files";
            this.sectionPanel4.Location = new System.Drawing.Point(18, 14);
            this.sectionPanel4.Name = "sectionPanel4";
            this.sectionPanel4.Padding = new System.Windows.Forms.Padding(14, 32, 14, 14);
            this.sectionPanel4.Size = new System.Drawing.Size(831, 211);
            this.sectionPanel4.TabIndex = 40;
            // 
            // openAPZip
            // 
            this.openAPZip.BackColor = System.Drawing.Color.Transparent;
            this.openAPZip.Cursor = System.Windows.Forms.Cursors.Hand;
            this.openAPZip.FlatAppearance.BorderSize = 0;
            this.openAPZip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openAPZip.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.openAPZip.Location = new System.Drawing.Point(21, 129);
            this.openAPZip.Name = "openAPZip";
            this.openAPZip.Primary = false;
            this.openAPZip.Radius = 8;
            this.openAPZip.Size = new System.Drawing.Size(91, 38);
            this.openAPZip.TabIndex = 34;
            this.openAPZip.Text = "Open AP";
            this.openAPZip.UseVisualStyleBackColor = false;
            this.openAPZip.Click += new System.EventHandler(this.openAPZip_Click);
            // 
            // openDestination
            // 
            this.openDestination.BackColor = System.Drawing.Color.Transparent;
            this.openDestination.Cursor = System.Windows.Forms.Cursors.Hand;
            this.openDestination.FlatAppearance.BorderSize = 0;
            this.openDestination.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openDestination.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.openDestination.Location = new System.Drawing.Point(21, 89);
            this.openDestination.Name = "openDestination";
            this.openDestination.Primary = false;
            this.openDestination.Radius = 8;
            this.openDestination.Size = new System.Drawing.Size(91, 38);
            this.openDestination.TabIndex = 33;
            this.openDestination.Text = "Browse";
            this.openDestination.UseVisualStyleBackColor = false;
            this.openDestination.Click += new System.EventHandler(this.openDestination_Click);
            // 
            // openISO
            // 
            this.openISO.BackColor = System.Drawing.Color.Transparent;
            this.openISO.Cursor = System.Windows.Forms.Cursors.Hand;
            this.openISO.FlatAppearance.BorderSize = 0;
            this.openISO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.openISO.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.openISO.Location = new System.Drawing.Point(21, 48);
            this.openISO.Name = "openISO";
            this.openISO.Primary = false;
            this.openISO.Radius = 8;
            this.openISO.Size = new System.Drawing.Size(91, 38);
            this.openISO.TabIndex = 32;
            this.openISO.Text = "Open ISO";
            this.openISO.UseVisualStyleBackColor = false;
            this.openISO.Click += new System.EventHandler(this.openISO_Click);
            // 
            // sectionPanel2
            // 
            this.sectionPanel2.BackColor = System.Drawing.Color.White;
            this.sectionPanel2.Controls.Add(this.seed);
            this.sectionPanel2.Controls.Add(this.optionsButton);
            this.sectionPanel2.Controls.Add(this.seedLabel);
            this.sectionPanel2.Controls.Add(this.logicSettings);
            this.sectionPanel2.Controls.Add(this.settingsLabel);
            this.sectionPanel2.Header = "Generation";
            this.sectionPanel2.Location = new System.Drawing.Point(855, 12);
            this.sectionPanel2.Name = "sectionPanel2";
            this.sectionPanel2.Padding = new System.Windows.Forms.Padding(14, 32, 14, 14);
            this.sectionPanel2.Size = new System.Drawing.Size(287, 213);
            this.sectionPanel2.TabIndex = 41;
            // 
            // optionsButton
            // 
            this.optionsButton.BackColor = System.Drawing.Color.Transparent;
            this.optionsButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.optionsButton.FlatAppearance.BorderSize = 0;
            this.optionsButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optionsButton.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.optionsButton.Location = new System.Drawing.Point(17, 173);
            this.optionsButton.Name = "optionsButton";
            this.optionsButton.Primary = false;
            this.optionsButton.Radius = 8;
            this.optionsButton.Size = new System.Drawing.Size(75, 23);
            this.optionsButton.TabIndex = 43;
            this.optionsButton.Text = "Options";
            this.optionsButton.UseVisualStyleBackColor = false;
            this.optionsButton.Click += new System.EventHandler(this.optionsButton_Click);
            // 
            // PBar
            // 
            this.PBar.Animate = true;
            this.PBar.FillColor = System.Drawing.Color.Transparent;
            this.PBar.Location = new System.Drawing.Point(164, 463);
            this.PBar.Maximum = 100;
            this.PBar.Minimum = 0;
            this.PBar.Name = "PBar";
            this.PBar.Size = new System.Drawing.Size(978, 76);
            this.PBar.StripeColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(214)))), ((int)(((byte)(102)))));
            this.PBar.StripeWidth = 10;
            this.PBar.TabIndex = 42;
            this.PBar.Text = "PBar";
            this.PBar.TrackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(236)))), ((int)(((byte)(236)))));
            this.PBar.TrackHeight = 14;
            this.PBar.Value = 0;
            this.PBar.WalkerFrames = 1;
            this.PBar.WalkerImage = null;
            this.PBar.WalkerScale = 1F;
            this.PBar.WalkerTicksPerFrame = 5;
            // 
            // randomizeButton
            // 
            this.randomizeButton.BackColor = System.Drawing.Color.Transparent;
            this.randomizeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.randomizeButton.FlatAppearance.BorderSize = 0;
            this.randomizeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.randomizeButton.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.randomizeButton.Location = new System.Drawing.Point(18, 490);
            this.randomizeButton.Name = "randomizeButton";
            this.randomizeButton.Primary = true;
            this.randomizeButton.Radius = 8;
            this.randomizeButton.Size = new System.Drawing.Size(140, 49);
            this.randomizeButton.TabIndex = 43;
            this.randomizeButton.Text = "Randomize";
            this.randomizeButton.UseVisualStyleBackColor = false;
            this.randomizeButton.Click += new System.EventHandler(this.randomizeButton_Click);
            // 
            // statusDialog
            // 
            this.statusDialog.BackColor = System.Drawing.Color.White;
            this.statusDialog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statusDialog.Font = new System.Drawing.Font("Consolas", 9F);
            this.statusDialog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.statusDialog.Location = new System.Drawing.Point(18, 231);
            this.statusDialog.Name = "statusDialog";
            this.statusDialog.ReadOnly = true;
            this.statusDialog.Size = new System.Drawing.Size(1124, 226);
            this.statusDialog.TabIndex = 44;
            this.statusDialog.TabStop = false;
            this.statusDialog.Text = "";
            // 
            // isoFilePath
            // 
            this.isoFilePath.BackColor = System.Drawing.Color.Transparent;
            this.isoFilePath.Location = new System.Drawing.Point(123, 52);
            this.isoFilePath.Name = "isoFilePath";
            this.isoFilePath.Padding = new System.Windows.Forms.Padding(10);
            this.isoFilePath.Radius = 6;
            this.isoFilePath.Size = new System.Drawing.Size(681, 30);
            this.isoFilePath.TabIndex = 35;
            this.isoFilePath.Text = "<- Select path to Chibi-Robo NTSC-U ISO";
            // 
            // destinationPath
            // 
            this.destinationPath.BackColor = System.Drawing.Color.White;
            this.destinationPath.Location = new System.Drawing.Point(123, 91);
            this.destinationPath.Name = "destinationPath";
            this.destinationPath.Padding = new System.Windows.Forms.Padding(10);
            this.destinationPath.Radius = 6;
            this.destinationPath.Size = new System.Drawing.Size(681, 28);
            this.destinationPath.TabIndex = 36;
            this.destinationPath.Text = "<- Set destination path";
            // 
            // apZipPath
            // 
            this.apZipPath.BackColor = System.Drawing.Color.White;
            this.apZipPath.Location = new System.Drawing.Point(123, 130);
            this.apZipPath.Name = "apZipPath";
            this.apZipPath.Padding = new System.Windows.Forms.Padding(10);
            this.apZipPath.Radius = 6;
            this.apZipPath.Size = new System.Drawing.Size(681, 28);
            this.apZipPath.TabIndex = 37;
            this.apZipPath.Text = "<- Set Archipelago Data To Enable Integration";
            // 
            // seed
            // 
            this.seed.BackColor = System.Drawing.Color.White;
            this.seed.Location = new System.Drawing.Point(70, 91);
            this.seed.Name = "seed";
            this.seed.Padding = new System.Windows.Forms.Padding(10);
            this.seed.Radius = 6;
            this.seed.Size = new System.Drawing.Size(200, 28);
            this.seed.TabIndex = 44;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1154, 551);
            this.Controls.Add(this.statusDialog);
            this.Controls.Add(this.randomizeButton);
            this.Controls.Add(this.PBar);
            this.Controls.Add(this.sectionPanel2);
            this.Controls.Add(this.sectionPanel4);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Chibi-Robo: Unplugged";
            this.sectionPanel4.ResumeLayout(false);
            this.sectionPanel2.ResumeLayout(false);
            this.sectionPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label settingsLabel;
        private System.Windows.Forms.Label seedLabel;
        private System.Windows.Forms.Timer timer1;
        private FlatComboBox logicSettings;
        private SectionPanel sectionPanel4;
        private SectionPanel sectionPanel2;
        private FlatProgressBar PBar;
        private FlatButton optionsButton;
        private FlatButton openISO;
        private FlatButton openDestination;
        private FlatButton openAPZip;
        private FlatButton randomizeButton;
        private LogBox statusDialog;
        private FlatTextBox isoFilePath;
        private FlatTextBox apZipPath;
        private FlatTextBox destinationPath;
        private FlatTextBox seed;
    }
}

