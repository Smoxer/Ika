using System;
using System.Windows.Forms;
using Library;

namespace IkariamBots.Forms
{
    public partial class NumberPicker : Form
    {
        public string number;
        private static Languages lang = Languages.Instance;

        public NumberPicker(string title, int max, System.Drawing.Image image)
        {
            InitializeComponent();

            #region Translate
            Cancel.Text = lang.Get("Misc", "Cancel");
            Trian.Text = lang.Get("Misc", "OK");
            #endregion

            Text = title;
            pictureBox1.Image = image;

            crewLabel.Text = "1";
            Crew.SetRange(1, max);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Trian_Click(object sender, EventArgs e)
        {
            if (Crew.Value > 0)
            {
                number = Crew.Value.ToString();
                DialogResult = DialogResult.OK;
            } else
                DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Crew_Scroll(object sender, EventArgs e)
        {
            crewLabel.Text = Crew.Value.ToString();
        }
    }
}
