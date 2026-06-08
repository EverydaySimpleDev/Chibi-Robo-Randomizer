using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
 
    public class OptionsForm : Form
    {
        public bool OpenUpstairs { get; set; }
        public bool ChibiVisionOff { get; set; }
        public bool RandomizePasswords { get; set; }

        private readonly FlatCheckBox chkOpenUpstairs;
        private readonly FlatCheckBox chkChibiVision;
        private readonly FlatCheckBox chkPasswords;

        public OptionsForm()
        {
            Text = "Options";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(280, 200);

            var card = new SectionPanel
            {
                Header = "Options",
                Location = new Point(16, 16),
                Size = new Size(248, 120)
            };

            chkOpenUpstairs = new FlatCheckBox { Text = "Open upstairs", Location = new Point(14, 38) };
            chkChibiVision = new FlatCheckBox { Text = "Chibi-Vision off", Location = new Point(14, 64) };
            chkPasswords = new FlatCheckBox { Text = "Randomize passwords", Location = new Point(14, 90) };
            card.Controls.Add(chkOpenUpstairs);
            card.Controls.Add(chkChibiVision);
            card.Controls.Add(chkPasswords);

            var okButton = new FlatButton
            {
                Text = "OK",
                Primary = true,
                Size = new Size(110, 34),
                Location = new Point(16, 150)
            };
            okButton.Click += OkButton_Click;

            var cancelButton = new FlatButton
            {
                Text = "Cancel",
                Size = new Size(110, 34),
                Location = new Point(154, 150)
            };
            cancelButton.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.Add(card);
            Controls.Add(okButton);
            Controls.Add(cancelButton);

            AcceptButton = okButton;
            CancelButton = cancelButton;

            Theme.Apply(this); // reuse the same theme as the main window
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            chkOpenUpstairs.Checked = OpenUpstairs;
            chkChibiVision.Checked = ChibiVisionOff;
            chkPasswords.Checked = RandomizePasswords;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            OpenUpstairs = chkOpenUpstairs.Checked;
            ChibiVisionOff = chkChibiVision.Checked;
            RandomizePasswords = chkPasswords.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}