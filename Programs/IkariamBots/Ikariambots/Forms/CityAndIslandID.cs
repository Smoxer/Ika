using Library;
using System;
using System.Windows.Forms;

namespace IkariamBots.Forms
{
    public partial class CityAndIslandID : Form
    {
        public string cityID, islandID;
        private static Languages lang = Languages.Instance;

        public CityAndIslandID()
        {
            InitializeComponent();

            #region Translate
            label1.Text = lang.Get("Piracy", "CityID");
            label2.Text = lang.Get("Piracy", "IslandID");
            OK.Text = lang.Get("Misc", "OK");
            Cancel.Text = lang.Get("Misc", "Cancel");
            Text = lang.Get("Piracy", "SetCity");
            #endregion
        }

        private void AllowDigit(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || (e.KeyChar == (char)Keys.Back)))
                e.Handled = true;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            cityID = city.Text;
            islandID = island.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
