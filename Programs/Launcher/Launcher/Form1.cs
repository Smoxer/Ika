using System;
using System.IO;
using System.Xml;
using System.Net;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Management;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Launcher
{
    public partial class Form1 : Form
    {
        public static string TOR = "socks://127.0.0.1:9150";
        public const string DATE_TIME_FORMAT = "HH:mm:ss"; // Date and time format
        public const string SERVER = "http://www.ikariamplus.com/";
        public const string SERVER2 = "http://www.ikariamplus.com/";

        private string countryCode, serverCode;
        private bool finishedUI;
        private WebClient client;
        private List<string> passwords;
        private Languages lang;
        private Settings settings;
        private string uniqueAddress = string.Format("{0}|{1}|{2}|{3}", GetCPUInfo(), GetVolumeSerial(), GetMotherBoardSerial(), IsOnVirtualMachine().ToString()); // Computer unique ID

        public Form1()
        {
            InitializeComponent();
            lang = Languages.Instance;
            settings = Settings.Instance;
            #region Translate
            login.Text = lang.Get("Launcher", "Login");
            button2.Text = lang.Get("Launcher", "Login");
            register.Text = lang.Get("Launcher", "Register");
            label5.Text = lang.Get("Misc", "UA");
            label12.Text = lang.Get("Misc", "UA");
            label13.Text = lang.Get("Launcher", "Invite") + "   ?";
            label7.Text = lang.Get("Misc", "Proxy");
            label11.Text = lang.Get("Misc", "Proxy");
            label3.Text = lang.Get("Misc", "Server");
            label4.Text = lang.Get("Misc", "Country");
            label2.Text = lang.Get("Misc", "Password");
            label9.Text = lang.Get("Misc", "Password");
            label10.Text = lang.Get("Misc", "EMail");
            label1.Text = lang.Get("Misc", "Username");
            label8.Text = lang.Get("Misc", "Username");
            label14.Text = lang.Get("Launcher", "HowManyUsers");
            tabPage1.Text = lang.Get("Launcher", "Login");
            tabPage2.Text = lang.Get("Launcher", "Register");
            tabPage3.Text = lang.Get("Launcher", "MassivePiracy");
            tabPage4.Text = lang.Get("Launcher", "News");
            autoActive.Text = lang.Get("Launcher", "ActiveMail");
            button1.Text = lang.Get("Misc", "Add");
            SelectFile.Text = lang.Get("Launcher", "Import");
            ExportUsers.Text = lang.Get("Launcher", "Export");
            fileToolStripMenuItem.Text = lang.Get("Misc", "File");
            languageToolStripMenuItem.Text = lang.Get("Misc", "Language");
            Text = "Ikariam+ - " + lang.Get("Launcher", "Name");
            licenseToolStripMenuItem.Text = lang.Get("Misc", "License");
            sendLogToolStripMenuItem.Text = lang.Get("Launcher", "SendLog");
            accessTokenToolStripMenuItem.Text = lang.Get("Launcher", "SetAccessToken");
            getAccessTokenToolStripMenuItem.Text = lang.Get("Launcher", "GetAccessToken");
            helpToolStripMenuItem.Text = lang.Get("WebView", "Help");
            eMailToolStripMenuItem.Text = lang.Get("Misc", "EMail");
            #endregion

            finishedUI = false;
            countryCode = "";
            serverCode = "";
            passwords = new List<string>();
            client = new WebClient();
            client.Encoding = Encoding.UTF8;
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:49.0) Gecko/20100101 Firefox/49.0");
            userName.Text = Settings.Instance.Get("UserName", "");
            password.Text = Settings.Instance.Get("Password", "");
            apiKey.Text = Settings.Instance.Get("CaptchaUsername", "");
            apiSource.Text = Settings.Instance.Get("CaptchaPassword", "");
            news.Url = new Uri(SERVER2 + "news.php");

            ToolTip infoToolTip = new ToolTip();
            infoToolTip.ToolTipIcon = ToolTipIcon.Info;
            infoToolTip.IsBalloon = true;
            infoToolTip.ShowAlways = true;
            infoToolTip.UseAnimation = true;
            infoToolTip.ToolTipTitle = lang.Get("Misc", "Info");
            infoToolTip.SetToolTip(label13, lang.Get("Launcher", "RegisterInvite"));

            string[] fileEntries = Directory.GetFiles(Languages.LANGUAGES_LOCATION);
            foreach (string fileName in fileEntries)
            {
                ToolStripMenuItem langauge = new ToolStripMenuItem(fileName.Replace(".ini", "").Replace(Languages.LANGUAGES_LOCATION, ""));
                langauge.Click += ChangeLanguage;
                languageToolStripMenuItem.DropDownItems.Add(langauge);
            }
            ToolStripMenuItem addLangauge = new ToolStripMenuItem(lang.Get("Misc", "Add"));
            addLangauge.Click += AddLanguage;
            languageToolStripMenuItem.DropDownItems.Add(addLangauge);

            try
            {
                string countriesHTML = client.DownloadString("https://us.ikariam.gameforge.com/");
                MatchCollection countries = Regex.Matches(countriesHTML, "<li.*><a href=\"(https:|)//(.+?)\\.ikariam\\.gameforge\\.com/\\?kid=.*\" target=\"_top\" rel=\"nofollow\" class=\".*\">(.+?)</a></li>");
                string build = string.Empty;
                foreach (Match match in countries)
                {
                    GroupCollection groups = match.Groups;
                    if (groups.Count >= 2)
                        country.Items.Add(string.Format("{1} - {0}", groups[2].Value, groups[3].Value));
                }
            }
            catch (WebException e)
            {
                try
                {
                    string countriesHTML = client.DownloadString("https://en.ikariam.gameforge.com/");
                    MatchCollection countries = Regex.Matches(countriesHTML, "<li.*><a href=\"//(.+?)\\.ikariam\\.gameforge\\.com/\\?kid=.*\" target=\"_top\" rel=\"nofollow\" class=\".*\">(.+?)</a></li>");
                    string build = string.Empty;
                    foreach (Match match in countries)
                    {
                        GroupCollection groups = match.Groups;
                        if (groups.Count >= 2)
                            country.Items.Add(string.Format("{1} - {0}", groups[1].Value, groups[2].Value));
                    }
                }
                catch (WebException ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            int index = 0;
            index = (Settings.Instance.Get("CaptchaProvider", "Manual").Equals("DeCaptcher") ? 1 : index);
            index = (Settings.Instance.Get("CaptchaProvider", "Manual").Equals("DeathByCaptcha") ? 2 : index);
            index = (Settings.Instance.Get("CaptchaProvider", "Manual").Equals("Manual") ? 3 : index);
            captcha.SelectedIndex = index;
            SetText();
            if (Settings.Instance.Get("UserAgent", "").Trim('\r', '\n', ' ').Equals("")) { ua.Text = "Mozilla/5.0 (X11; Linux x86_64; rv:10.0) Gecko/20100101 Firefox/10.0"; newUA.Text = ua.Text = "Mozilla/5.0 (X11; Linux x86_64; rv:10.0) Gecko/20100101 Firefox/10.0"; }
            else { ua.Text = Settings.Instance.Get("UserAgent", "").Trim('\r', '\n', ' '); newUA.Text = Settings.Instance.Get("UserAgent", "").Trim('\r', '\n', ' '); }
            if (Settings.Instance.Get("EnableProxy", "false").Equals("true"))
            {
                proxy.Text = Settings.Instance.Get("ProxyAddress", "");
                newProxy.Text = Settings.Instance.Get("ProxyAddress", "");

                if (proxy.Text.Equals(TOR))
                {
                    torr.Checked = true;
                    newTor.Checked = true;
                    proxy.Enabled = !torr.Checked;
                    newProxy.Enabled = !newTor.Checked;
                }
            }
            else
            {
                proxy.Text = "";
                newProxy.Text = "";
            }

            for (int i = 0; i < country.Items.Count; i++)
            {
                if (country.Items[i].ToString() != "< Custom Country >")
                {
                    if (country.Items[i].ToString().Split('-')[1].Replace(" ", "").Equals(Settings.Instance.Get("Server", "us")))
                    {
                        countryCode = country.Items[i].ToString().Split('-')[1].Replace(" ", "");
                        country.SelectedIndex = i;
                        break;
                    }
                }
                else
                {
                    countryCode = Settings.Instance.Get("ServerGame", "us");
                    country.SelectedIndex = country.Items.Count - 1;
                }
            }

            for (int i = 0; i < server.Items.Count; i++)
            {
                if (server.Items[i].ToString() != "< Custom Server >")
                {
                    if (server.Items[i].ToString().Split('-')[1].Replace(" ", "").Equals(Settings.Instance.Get("ServerGame", "s1-us").Split('-')[0]))
                    {
                        serverCode = server.Items[i].ToString().Split('-')[1].Replace(" ", "");
                        server.SelectedIndex = i;
                        break;
                    }
                }
                else
                {
                    //serverCode = config.DocumentElement.SelectSingleNode("/Bot/Server").InnerText.Split('-')[0];
                    server.SelectedIndex = country.Items.Count - 1;
                }
            }

            users.Scrollable = true;
            users.View = View.Details;
            users.Columns.Add(lang.Get("Misc", "Username"), 100);
            users.Columns.Add(lang.Get("Misc", "Password"), 100);
            users.Columns.Add(lang.Get("Misc", "Server"), 100);
            users.Columns.Add(lang.Get("Piracy", "Mission"), 100);
            users.Columns.Add(lang.Get("Misc", "UA"), 100);
            users.Columns.Add(lang.Get("Misc", "Proxy"), 100);
            users.Columns.Add(lang.Get("Piracy", "AutoConvertToCrew"), 100);
            users.Columns.Add(lang.Get("Piracy", "StartManufactur"), 100);
            users.Columns.Add(lang.Get("Piracy", "EndManufactur"), 100);
            users.Columns.Add(lang.Get("Misc", "Type"), 100);

            if (!File.Exists("massiveUsers.users"))
            {
                File.WriteAllText("massiveUsers.users", "", Encoding.UTF8);
            }
            RereadUsers();
            
            finishedUI = true;
        }

        private void AddLanguage(object sender, EventArgs e)
        {
            Process.Start(SERVER + "buy.php?page=Contact");
        }

        private void ChangeLanguage(object sender, EventArgs e)
        {
            Settings.Instance.Set("Language", ((ToolStripItem)sender).Text);
            Settings.Instance.Commit();
            Process.Start(Application.ExecutablePath);
            Close();
        }

        private void SetText()
        {
            bool temp = true;
            if (captcha.SelectedIndex == 1)
            {
                //De-Captcher
                label6.Text = lang.Get("Piracy", "DecaptcherUserName");
                captchaPwd.Text = lang.Get("Piracy", "DecaptcherPassword");
            }
            else if(captcha.SelectedIndex == 0)
            {
                //KW
                label6.Text = lang.Get("Piracy", "KWAPIKey");
                captchaPwd.Text = lang.Get("Piracy", "KWAPISource");
            }
            else if(captcha.SelectedIndex == 2)
            {
                //Death By Captcha
                label6.Text = lang.Get("Piracy", "DeathByCaptchaUserName");
                captchaPwd.Text = lang.Get("Piracy", "DeathByCaptchaPassword");
            }
            else
            {
                //Manual
                label6.Text = lang.Get("Piracy", "ManualSolve");
                captchaPwd.Text = lang.Get("Piracy", "ManualSolve");
                temp = false;
            }
            apiKey.Enabled = temp;
            apiSource.Enabled = temp;
        }

        private void login_Click(object sender, EventArgs e)
        {
            if (countryCode != "" && serverCode != "" && userName.Text != "" && password.Text != "")
            {
                settings.Set("ProgramMode", "Login");
                settings.Set("Server", countryCode);
                settings.Set("ServerGame", serverCode + "-" + countryCode);
                settings.Set("UserAgent", ua.Text);
                settings.Set("UserName", userName.Text);
                settings.Set("Password", password.Text);
                settings.Set("CaptchaProvider", captcha.Text);
                settings.Set("CaptchaUsername", apiKey.Text);
                settings.Set("CaptchaPassword", apiSource.Text);

                string proxy_user = "None";
                if (proxy.Text.Equals(""))
                    settings.Set("EnableProxy", "false");
                else
                {
                    settings.Set("EnableProxy", "true");
                    if (proxy.Text.ToLower().Equals(TOR))
                    {
                        Process.Start(Path.Combine(Environment.CurrentDirectory, "Tor\\tor.exe"), "-f \"" + Path.Combine(Environment.CurrentDirectory, "Tor\\torrc-defaults") + "\"");
                        Thread.Sleep(5000);
                        settings.Set("ProxyAddress", TOR);
                        proxy_user = TOR;
                    }
                    else
                    {
                        settings.Set("ProxyAddress", proxy.Text);
                        proxy_user = proxy.Text;
                    }
                }

                Process.Start("IkariamPlus.exe");
                Application.Exit();
            }
        }

        private void server_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (finishedUI)
            {
                if (server.Text != "< Custom Server >")
                    serverCode = server.Text.Split('-')[1].Replace(" ", "");//MessageBox.Show(server.Text.Split('-')[0].Replace(" ", ""), server.Text.Split('-')[1].Replace(" ", ""));
                else
                {
                    string UserAnswer = Interaction.InputBox(lang.Get("Misc", "EnterServerCode"), lang.Get("Misc", "ServerCode"), "");
                    if (UserAnswer != "")
                        serverCode = UserAnswer;
                    else
                        MessageBox.Show(lang.Get("Misc", "InvalidServerCode"), lang.Get("Misc", "Error"));
                }
            }
        }

        private void register_Click(object sender, EventArgs e)
        {
            if(!newUserName.Text.Equals("") && !newPassword.Text.Equals("") && !newEmail.Text.Equals(""))
            {
                settings.Set("ProgramMode", "Register");
                settings.Set("UserAgent", newUA.Text);
                settings.Set("CaptchaProvider", captcha.Text);
                settings.Set("CaptchaUsername", apiKey.Text);
                settings.Set("CaptchaPassword", apiSource.Text);

                string proxy_user = "None";
                if (newProxy.Text.Equals(""))
                    settings.Set("EnableProxy", "false");
                else
                {
                    settings.Set("EnableProxy", "true");
                    if (newProxy.Text.ToLower().Equals(TOR))
                    {
                        Process.Start(Path.Combine(Environment.CurrentDirectory, "Tor\\tor.exe"), "-f \"" + Path.Combine(Environment.CurrentDirectory, "Tor\\torrc-defaults") + "\"");
                        Thread.Sleep(5000);
                        settings.Set("ProxyAddress", TOR);
                        proxy_user = TOR;
                    }
                    else
                    {
                        settings.Set("ProxyAddress", newProxy.Text);
                        proxy_user = proxy.Text;
                    }
                }

                for (int i = 0; i < howmany.Value; i++)
                {
                    string args = "\"" + newUserName.Text + (i + 1).ToString() + "\" \"" + newPassword.Text + "\" \"" + newEmail.Text + "\" \"" + invite.Text + "\"";
                    Process.Start("IkariamPlus.exe", args);
                }
                Application.Exit();
            }

           
        }

        private void country_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (finishedUI)
            {
                if (country.Text != "< Custom Country >")
                    countryCode = country.Text.Split('-')[1].Replace(" ", ""); //MessageBox.Show(country.Text.Split('-')[0].Replace(" ", ""), country.Text.Split('-')[1].Replace(" ", ""));
                else
                {
                    string UserAnswer = Interaction.InputBox(lang.Get("Misc", "EnterCountryCode"), lang.Get("Misc", "CountryCode"), "");
                    if (UserAnswer != "")
                        countryCode = UserAnswer;
                    else
                        MessageBox.Show(lang.Get("Misc", "InvalidCountryCode"), lang.Get("Misc", "Error"));
                }
            }
            LoadServers();
        }

        private void newTor_CheckedChanged(object sender, EventArgs e)
        {
            newProxy.Enabled = !newTor.Checked;
            if (newTor.Checked)
                newProxy.Text = TOR;
            else
                newProxy.Text = "";
        }

        private void torr_CheckedChanged(object sender, EventArgs e)
        {
            proxy.Enabled = !torr.Checked;
            if (torr.Checked)
                proxy.Text = TOR;
            else
                proxy.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            settings.Set("ProgramMode", "Piracy");
            settings.Set("CaptchaProvider", captcha.Text);
            settings.Set("CaptchaUsername", apiKey.Text);
            settings.Set("CaptchaPassword", apiSource.Text);

            bool runTor = false;
            
            foreach (string line in File.ReadAllLines("massiveUsers.users"))
            {
                string[] data = line.Split(',');
                if (data.Length >= 9)
                {
                    if (data[5].Equals(TOR))
                    {
                        runTor = true;
                        break;
                    }
                }
            }
            if (runTor)
            {
                Process.Start(Path.Combine(Environment.CurrentDirectory, "Tor\\tor.exe"), "-f \"" + Path.Combine(Environment.CurrentDirectory, "Tor\\torrc-defaults") + "\"");
                Thread.Sleep(5000);
            }
            
            Process.Start("IkariamPlus.exe");
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var userForm = new User(
                false,
                0,
                "",
                "", //Password
                "",
                0,
                "Mozilla/5.0 (X11; Linux x86_64; rv:10.0) Gecko/20100101 Firefox/10.0",
                "None",
                false,
                DateTime.Now.ToString(DATE_TIME_FORMAT),
                DateTime.Now.AddHours(7).ToString(DATE_TIME_FORMAT),
                true
           ))
            {
                var result = userForm.ShowDialog();
                if (result == (DialogResult)User.FORM_OPTIONS.SAVE)
                {
                    string[] data = {
                        userForm.userName.Text,
                        userForm.password.Text,
                        userForm.server.Text,
                        userForm.missionSelect.SelectedIndex.ToString(),
                        userForm.UA.Text,
                        userForm.proxy.Text,
                        userForm.autoConvert.Checked.ToString(),
                        userForm.startDateManufacture.Value.ToString(DATE_TIME_FORMAT),
                        userForm.endDateManufacture.Value.ToString(DATE_TIME_FORMAT),
                        (userForm.manufactureOption.Checked ? "Manufacture" : "Raider")
                    };
                    string[] newUser = { string.Join(",", data) };
                    File.AppendAllLines("massiveUsers.users", newUser);
                    RereadUsers();
                }
            }
        }

        private void users_ItemActivate(object sender, EventArgs e)
        {
            using (var userForm = new User(
                true,
                users.SelectedItems[0].Index,
                users.SelectedItems[0].SubItems[0].Text,
                passwords[users.SelectedItems[0].Index], //Password
                users.SelectedItems[0].SubItems[2].Text,
                int.Parse(users.SelectedItems[0].SubItems[3].Text),
                users.SelectedItems[0].SubItems[4].Text,
                users.SelectedItems[0].SubItems[5].Text,
                bool.Parse(users.SelectedItems[0].SubItems[6].Text),
                users.SelectedItems[0].SubItems[7].Text,
                users.SelectedItems[0].SubItems[8].Text,
                users.SelectedItems[0].SubItems[9].Text.Equals("Manufacture")
           ))
            {
                var result = userForm.ShowDialog();
                if (result == (DialogResult)User.FORM_OPTIONS.SAVE)
                {
                    string[] lines = File.ReadAllLines("massiveUsers.users");
                    string[] data = {
                        userForm.userName.Text,
                        userForm.password.Text,
                        userForm.server.Text,
                        userForm.missionSelect.SelectedIndex.ToString(),
                        userForm.UA.Text,
                        userForm.proxy.Text,
                        userForm.autoConvert.Checked.ToString(),
                        userForm.startDateManufacture.Value.ToString(DATE_TIME_FORMAT),
                        userForm.endDateManufacture.Value.ToString(DATE_TIME_FORMAT),
                        (userForm.manufactureOption.Checked ? "Manufacture" : "Raider")
                    };
                    lines[userForm.idUser] = string.Join(",", data);
                    File.WriteAllLines("massiveUsers.users", lines);

                } else if (result == (DialogResult)User.FORM_OPTIONS.DUPLICATE)
                {
                    string[] data = {
                        userForm.userName.Text,
                        userForm.password.Text,
                        userForm.server.Text,
                        userForm.missionSelect.SelectedIndex.ToString(),
                        userForm.UA.Text,
                        userForm.proxy.Text,
                        userForm.autoConvert.Checked.ToString(),
                        userForm.startDateManufacture.Value.ToString(DATE_TIME_FORMAT),
                        userForm.endDateManufacture.Value.ToString(DATE_TIME_FORMAT),
                        (userForm.manufactureOption.Checked ? "Manufacture" : "Raider")
                    };
                    string[] newUser = { string.Join(",", data) };
                    File.AppendAllLines("massiveUsers.users", newUser);

                }
                else if (result == (DialogResult)User.FORM_OPTIONS.DELETE)
                {
                    string[] lines = File.ReadAllLines("massiveUsers.users");
                    List<string> users_deleted = new List<string>();
                    for(int i = 0; i < lines.Length; i++)
                    {
                        if (i != userForm.idUser)
                            users_deleted.Add(lines[i]);
                    }
                    File.WriteAllLines("massiveUsers.users", users_deleted.ToArray());
                }
            }
            RereadUsers();
        }

        private void RereadUsers()
        {
            passwords.Clear();
            users.Items.Clear();
            string[] lines = File.ReadAllLines("massiveUsers.users");
            for(int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Replace(" ", "").Equals(""))
                    continue;
                string[] data = lines[i].Split(',');
                passwords.Add(data[1]);
                data[1] = new string('*', data[1].Length);
                List<string> newData = new List<string>();
                newData.AddRange(data);
                if (data.Length == 9)
                {
                    newData.Add("Manufacture");
                    lines[i] += ",Manufacture";
                }
                users.Items.Add(new ListViewItem(newData.ToArray()));
            }
            File.WriteAllLines("massiveUsers.users", lines);
        }

        private void SelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Ikariam+ Users File|*.users|All|*.*";
            if (file.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText("massiveUsers.users", File.ReadAllText(file.FileName));
                RereadUsers();
            }
            
        }

        private void ExportUsers_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "Ikariam+ Users File (.users)|*.users";
            if (sd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sd.FileName, File.ReadAllText("massiveUsers.users"));
            }
        }

        private void users_MouseClick(object sender, MouseEventArgs e)
        {
            bool match = false;

            if (e.Button == MouseButtons.Right)
            {
                foreach (ListViewItem item in users.Items)
                {

                    if (item.Bounds.Contains(new Point(e.X, e.Y)))
                    {
                        ToolStripLabel title = new ToolStripLabel(item.SubItems[0].Text);
                        title.Enabled = false;

                        ToolStripMenuItem edit = new ToolStripMenuItem(lang.Get("Misc", "Edit"));
                        item.Selected = true;
                        edit.Click += users_ItemActivate;
                        
                        ToolStripItem[] menuItems = new ToolStripItem[] { title, new ToolStripSeparator(), edit };

                        ContextMenuStrip menu = new ContextMenuStrip();
                        menu.Items.AddRange(menuItems);
                        users.ContextMenuStrip = menu;
                        match = true;
                        break;
                    }
                }
                if (match)
                {
                    users.ContextMenuStrip.Show(users, new Point(e.X, e.Y));
                }
                else
                {
                    //Show listViews context menu
                }
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            news.Refresh();
        }

        private void licenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(client.DownloadString(string.Format(SERVER + "index.php?UntilUse={0}", uniqueAddress)), lang.Get("License", "ValidUntil"));
        }

        private static string GetCPUInfo()
        {
            ManagementClass management = new ManagementClass("win32_processor");

            foreach (ManagementObject managmentObject in management.GetInstances())
            {
                return managmentObject.Properties["processorID"].Value.ToString();
            }

            return string.Empty;
        }

        private static string GetVolumeSerial()
        {
            string drive = "C";
            ManagementObject managementObject = new ManagementObject(@"win32_logicaldisk.deviceid=""" + drive + @":""");
            managementObject.Get();

            return managementObject["VolumeSerialNumber"].ToString();
        }

        private static string GetMotherBoardSerial()
        {
            ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_BaseBoard");

            foreach (ManagementObject managmentObject in managementObjectSearcher.Get())
            {
                return (string)managmentObject["SerialNumber"];
            }

            return string.Empty;
        }

        private static bool IsOnVirtualMachine()
        {
            using (var searcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
            {
                using (var items = searcher.Get())
                {
                    foreach (var item in items)
                    {
                        string manufacturer = item["Manufacturer"].ToString().ToLower();
                        if ((manufacturer == "microsoft corporation" && item["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL"))
                            || manufacturer.Contains("vmware")
                            || item["Model"].ToString() == "VirtualBox")
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void sendLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists("Log"))
            {
                string[] lines = File.ReadAllLines("Log");
                var reqparm = new System.Collections.Specialized.NameValueCollection();
                reqparm.Add("log", string.Join("<br>", lines));
                client.UploadValues(SERVER + "index.php?SendLog&MAC=" + uniqueAddress, "POST", reqparm);
                try
                {
                    File.Delete("Log");
                }
                catch (Exception ex) { }
            }
            MessageBox.Show(lang.Get("Misc", "LogSent"), lang.Get("Misc", "Info"));
        }

        private void accessTokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string UserAnswer = Interaction.InputBox("Pushbullet " + lang.Get("Launcher", "AccessToken") + ":", lang.Get("Launcher", "AccessToken"), settings.Get("Pushbullet", ""));
            settings.Set("Pushbullet", UserAnswer);
            settings.Commit();
        }

        private void getAccessTokenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.pushbullet.com/#settings/account");
        }

        private void autoActive_CheckedChanged(object sender, EventArgs e)
        {
            newEmail.Enabled = !autoActive.Checked;
            newEmail.Text = "";
            if (autoActive.Checked)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                Random random = new Random();
                newEmail.Text = new string(Enumerable.Repeat(chars, 8)
                  .Select(s => s[random.Next(s.Length)]).ToArray()) + "@mailnesia.com";
            }
        }

        private void eMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(SERVER + "buy.php?page=Contact");
        }

        private void captcha_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetText();
        }

        private void LoadServers()
        {
            server.Items.Clear();
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
                            if (!server.Items.Contains(string.Format("{1} - {0}", groups[1].Value, groups[2].Value)))
                                server.Items.Add(string.Format("{1} - {0}", groups[1].Value, groups[2].Value));
                        }
                    }
                }
                if (server.Items.Count > 0)
                    server.SelectedIndex = 0;
                else
                {
                    string UserAnswer = Interaction.InputBox(lang.Get("Misc", "CouldNotFetchServerList"), lang.Get("Misc", "ServerCode"), "");
                    if (UserAnswer != "")
                    {
                        server.Items.Add("< Custom Server >");
                        serverCode = UserAnswer;

                    }
                    else
                        MessageBox.Show(lang.Get("Misc", "InvalidServerCode"), lang.Get("Misc", "Error"));
                }
            } catch(WebException e)
            {
                MessageBox.Show(e.ToString());
            }
        }
    }
}
