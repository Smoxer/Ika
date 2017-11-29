using System;
using System.Windows.Forms;
using Library;

namespace IkariamBots.Forms
{
    public partial class User : Form
    {
        public DateTime start, end;
        public bool autoConvertCrew;
        public int max;
        private static Languages lang = Languages.Instance;

        public User(string userName, string password, string server, int missionSelect, string UA, string proxy, bool autoConvert, string startDateManufacture, string endDateManufacture)
        {
            InitializeComponent();

            #region Translate
            label5.Text = lang.Get("Misc", "UA");
            label7.Text = lang.Get("Piracy", "AutoConvertToCrew");
            this.autoConvert.Text = lang.Get("Piracy", "AutoConvertToCrew");
            label3.Text = lang.Get("Misc", "Server");
            label4.Text = lang.Get("Piracy", "Mission");
            label2.Text = lang.Get("Misc", "Password");
            label9.Text = lang.Get("Piracy", "EndManufactur");
            label1.Text = lang.Get("Misc", "Username");
            label8.Text = lang.Get("Piracy", "StartManufactur");
            label6.Text = lang.Get("Misc", "Proxy");
            label10.Text = lang.Get("Piracy", "MaxPerDay");
            Text = lang.Get("Piracy", "EditUser");
            #endregion

            this.userName.Text = userName;
            this.password.Text = password;
            this.missionSelect.SelectedIndex = missionSelect;
            this.server.Text = server;
            this.UA.Text = UA;
            this.proxy.Text = proxy;
            this.autoConvert.Checked = autoConvert;
            this.startDateManufacture.Value = DateTime.Parse(startDateManufacture);
            this.startDateManufacture.Format = DateTimePickerFormat.Time;
            this.startDateManufacture.ShowUpDown = true;
            this.startDateManufacture.CustomFormat = Config.DATE_TIME_FORMAT;
            this.endDateManufacture.Value = DateTime.Parse(endDateManufacture);
            this.endDateManufacture.Format = DateTimePickerFormat.Time;
            this.endDateManufacture.ShowUpDown = true;
            this.endDateManufacture.CustomFormat = Config.DATE_TIME_FORMAT;

            int maxPerDay = 0;

            if (int.TryParse(Settings.Instance.Get("MaxCapturePointsPerDay", "27000"), out maxPerDay))
                Maximum.Value = maxPerDay;
            else
                Maximum.Value = 27000;

            Text = userName;
        }

        private void save_Click(object sender, EventArgs e)
        {
            start = startDateManufacture.Value;
            end = endDateManufacture.Value;
            autoConvertCrew = autoConvert.Checked;
            max = (int) Maximum.Value;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
