using System;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Launcher
{
    public partial class User : Form
    {
        /*public const DialogResult CANCEL = DialogResult.OK;
        public const DialogResult SAVE = DialogResult.No;
        public const DialogResult DELETE = DialogResult.Yes;
        public const DialogResult DUPLICATE = DialogResult.Retry;*/
        public enum FORM_OPTIONS
        {
            CANCEL=1,
            SAVE=7,
            DELETE=6,
            DUPLICATE=4
        }
        
        public int idUser;
        private WebClient client;
        private string world;
        private Languages lang;

        public User(bool showAll, int idUser, string userName, string password, string server, int missionSelect, string UA, string proxy, bool autoConvert, string startDateManufacture, string endDateManufacture, bool isManufacture)
        {
            InitializeComponent();
            lang = Languages.Instance;

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
            Text = lang.Get("Piracy", "EditUser");
            cancel.Text = lang.Get("Misc", "Cancel");
            save.Text = lang.Get("Misc", "Save");
            duplicate.Text = lang.Get("Misc", "Duplicate");
            delete.Text = lang.Get("Misc", "DeleteUser");
            manufactureOption.Text = lang.Get("Piracy", "Manufacture");
            raiderOption.Text = lang.Get("Piracy", "Raider");
            #endregion

            this.idUser = idUser;
            this.userName.Text = userName;
            this.password.Text = password;
            this.missionSelect.SelectedIndex = missionSelect;
            this.UA.Text = UA;
            this.proxy.Text = proxy;
            this.autoConvert.Checked = autoConvert;
            this.startDateManufacture.Value = DateTime.Parse(startDateManufacture);
            this.startDateManufacture.Format = DateTimePickerFormat.Time;
            this.startDateManufacture.ShowUpDown = true;
            this.startDateManufacture.CustomFormat = Form1.DATE_TIME_FORMAT;
            this.endDateManufacture.Value = DateTime.Parse(endDateManufacture);
            this.endDateManufacture.Format = DateTimePickerFormat.Time;
            this.endDateManufacture.CustomFormat = Form1.DATE_TIME_FORMAT;
            this.endDateManufacture.ShowUpDown = true;
            manufactureOption.Checked = isManufacture;
            raiderOption.Checked = !isManufacture;

            client = new WebClient();

            if (!showAll)
            {
                duplicate.Enabled = false;
                delete.Enabled = false;
            }
            
            tor.Checked = proxy.Equals(Form1.TOR);

            world = "";
            if (server.Split('-').Length > 0)
                world = server.Split('-')[0];
            client.Encoding = Encoding.UTF8;

            try
            {
                string countriesHTML = client.DownloadString("https://us.ikariam.gameforge.com/");
                MatchCollection countries = Regex.Matches(countriesHTML, "<li.*><a href=\"//(.+?)\\.ikariam\\.gameforge\\.com/\\?kid=.*\" target=\"_top\" rel=\"nofollow\" class=\".*\">(.+?)</a></li>");
                string build = string.Empty;
                foreach (Match match in countries)
                {
                    GroupCollection groups = match.Groups;
                    if (groups.Count >= 2)
                    {
                        this.server.Items.Add(string.Format("{1} - {0}", groups[1].Value, groups[2].Value));
                        if (server.Split('-').Length > 0 && !server.Equals(string.Empty) && groups[1].Value.Equals(server.Split('-')[1])) 
                            this.server.SelectedIndex = this.server.Items.Count - 1;
                    }
                }
            }
            catch (WebException e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = (DialogResult) FORM_OPTIONS.CANCEL;
            Close();
        }

        private void save_Click(object sender, EventArgs e)
        {
            try
            {
                server.Text = worlds.Text.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries)[1] + "-" + server.Text.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries)[1]; ;
                DialogResult = (DialogResult)FORM_OPTIONS.SAVE;
                Close();
            } catch(Exception ex)
            {
                DialogResult = (DialogResult)FORM_OPTIONS.CANCEL;
                Close();
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            DialogResult = (DialogResult)FORM_OPTIONS.DELETE;
            Close();
        }

        private void tor_CheckedChanged(object sender, EventArgs e)
        {
            proxy.Enabled = !tor.Checked;
            if (tor.Checked)
                proxy.Text = Form1.TOR;
            else
                proxy.Text = "";
        }

        private void duplicate_Click(object sender, EventArgs e)
        {
            try { 
                server.Text = worlds.Text.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries)[1] + "-" + server.Text.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries)[1]; ;
                DialogResult = (DialogResult)FORM_OPTIONS.DUPLICATE;
                Close();
            }
            catch (Exception ex)
            {
                DialogResult = (DialogResult)FORM_OPTIONS.CANCEL;
                Close();
            }
        }

        private void server_SelectedIndexChanged(object sender, EventArgs e)
        {
            worlds.Items.Clear();
            string countryCode = server.Text.Split('-')[1].Replace(" ", "");
            try
            {
                string[] serversHTML = client.DownloadString("https://" + countryCode + ".ikariam.gameforge.com/").Replace(" ", "").Replace("\n", "").Replace("\r", "").Split(new string[] { "><" }, StringSplitOptions.None);
                for (int i = 0; i < serversHTML.Length; i++)
                {
                    MatchCollection servers = Regex.Matches(serversHTML[i], "value=\"(.*?)-" + countryCode + "\\.ikariam\\.gameforge\\.com\".*>(.*?)</option");
                    string build = string.Empty;
                    foreach (Match match in servers)
                    {
                        GroupCollection groups = match.Groups;
                        if (groups.Count >= 2)
                        {
                            if (!worlds.Items.Contains(string.Format("{1} - {0}", groups[1].Value, groups[2].Value)))
                            {
                                worlds.Items.Add(string.Format("{1} - {0}", groups[1].Value, groups[2].Value));
                                if (groups[1].Value.Equals(world))
                                    worlds.SelectedIndex = worlds.Items.Count - 1;
                            }
                        }
                    }
                }
                if (worlds.Items.Count == 0)
                {
                    MessageBox.Show(lang.Get("Misc", "InvalidServerCode"), lang.Get("Misc", "Error"));
                }
            }
            catch (WebException ee)
            {
                MessageBox.Show(ee.ToString());
            }
        }

        private void manufactureOption_CheckedChanged(object sender, EventArgs e)
        {
            missionSelect.Enabled = manufactureOption.Checked;
        }
    }
}
