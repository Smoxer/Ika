using System;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Threading;
using IkariamBots.Forms;
using Captchas;
using System.Windows.Forms;
using System.Collections.Generic;
using Library;

namespace IkariamBots
{
    public partial class PiracyHide : Form
    {
        public static string CANT_FETCH = "Can't fetch";
        public static int THREE_MINUTES = 180000;
        public static string[] massiveUsersFile;

        private string[] userNames, passwords, server_addresses, missionLevels, proxies, UAes, autoConvertCrewes, endDateManufactures, startDateManufactures, type;
        private static List<RunUser> usersList;
        private bool autoStart;
        private static Languages lang = Languages.Instance;
        private static readonly object blockRotation = new object();
        
        private static int maxPerDay;
        private static PhoneConnection phone;

        public PiracyHide(bool autoStart)
        {
            InitializeComponent();

            CANT_FETCH = lang.Get("Misc", "CantFetch");
            #region Translate
            startAllSimultaneous.Text = lang.Get("Piracy", "StartAllSimultaneous");
            Text = Config.PROGRAM_NAME + " - " + lang.Get("Piracy", "Name");
            #endregion

            string userNames = "", passwords = "", server_addresses = "", missionLevels = "", UAes = "", proxies = "", autoConvertCrewes = "", startDateManufactures = "", endDateManufactures = "", type = "";
            massiveUsersFile = File.ReadAllLines("massiveUsers.users");
            foreach (string line in massiveUsersFile)
            {
                string[] data = line.Split(',');
                if (data.Length == 10)
                {
					userNames += data[0] + ",";
					passwords += data[1] + ",";
					server_addresses += data[2] + ",";
					missionLevels += data[3] + ",";
					UAes += data[4] + ",";
					proxies += (data[5].Equals("") ? "None" : data[5]) + ",";
					autoConvertCrewes += data[6] + ",";
					startDateManufactures += data[7] + ",";
					endDateManufactures += data[8] + ",";
                    type += data[9] + ",";
                }
            }

            userNames = userNames.Remove(userNames.Length - 1);
            passwords = passwords.Remove(passwords.Length - 1);
            server_addresses = server_addresses.Remove(server_addresses.Length - 1);
            missionLevels = missionLevels.Remove(missionLevels.Length - 1);
            UAes = UAes.Remove(UAes.Length - 1);
            proxies = proxies.Remove(proxies.Length - 1);
            autoConvertCrewes = autoConvertCrewes.Remove(autoConvertCrewes.Length - 1);
            startDateManufactures = startDateManufactures.Remove(startDateManufactures.Length - 1);
            endDateManufactures = endDateManufactures.Remove(endDateManufactures.Length - 1);
            type = type.Remove(type.Length - 1);
            
            this.userNames = userNames.Split(',');
            this.passwords = passwords.Split(',');
            this.server_addresses = server_addresses.Split(',');
            this.proxies = proxies.Split(',');
            this.UAes = UAes.Split(',');
            this.autoConvertCrewes = autoConvertCrewes.Split(',');
            this.endDateManufactures = endDateManufactures.Split(',');
            this.startDateManufactures = startDateManufactures.Split(',');
            this.missionLevels = missionLevels.Split(',');
            this.type = type.Split(',');
            this.autoStart = autoStart;
            
            usersList = new List<RunUser>();
            phone = new PhoneConnection(Settings.Instance.Get("Pushbullet", ""));
        }

        private void PiracyHide_Load(object sender, EventArgs e)
        {
            Text = Config.PROGRAM_NAME + " - " + lang.Get("Piracy", "Name");

            users.Scrollable = true;
            users.View = View.Details;
            users.Columns.Add(lang.Get("Misc", "Username"), 100);           // 0
            users.Columns.Add(lang.Get("Misc", "Server"), 50);              // 1
            users.Columns.Add(lang.Get("Piracy", "Mission"), 50);           // 2
            users.Columns.Add(lang.Get("Piracy", "CapturePoints"), 100);    // 3
            users.Columns.Add(lang.Get("Piracy", "CrewStrength"), 100);     // 4
            users.Columns.Add(lang.Get("Piracy", "AutoConvert"), 100);      // 5
            users.Columns.Add(lang.Get("Piracy", "CityID"), 100);           // 6
            users.Columns.Add(lang.Get("Piracy", "IslandID"), 100);         // 7
            users.Columns.Add(lang.Get("Piracy", "StartManufactur"), 100);  // 8
            users.Columns.Add(lang.Get("Piracy", "EndManufactur"), 100);    // 9
            users.Columns.Add(lang.Get("Misc", "Log"), 100);                // 10
            users.Columns.Add(lang.Get("Piracy", "LastUpdate"), 100);       // 11
            users.Columns.Add(lang.Get("Misc", "Type"), 100);               // 12
            users.View = View.Details;

            startAllSimultaneous.Enabled = false;

            maxPerDay = 27000;

            int.TryParse(Settings.Instance.Get("MaxCapturePointsPerDay", "27000"), out maxPerDay);

            for (int i = 0; i < userNames.Length; i++)
            {
                users.Items.Add(new ListViewItem(new string[] { userNames[i], server_addresses[i], (int.Parse(missionLevels[i])+1).ToString(), CANT_FETCH, CANT_FETCH, autoConvertCrewes[i], CANT_FETCH, CANT_FETCH, startDateManufactures[i], endDateManufactures[i], "Log", DateTime.Now.ToString(Config.TIME_FORMAT), type[i] }));
                usersList.Add(new RunUser(i, userNames[i], passwords[i], server_addresses[i], missionLevels[i], proxies[i], UAes[i], bool.Parse(autoConvertCrewes[i]), DateTime.ParseExact(endDateManufactures[i], Config.TIME_FORMAT, null), DateTime.ParseExact(startDateManufactures[i], Config.TIME_FORMAT, null), type[i]));
                Thread.Sleep(500);
            }

            startAllSimultaneous.Enabled = true;
            if (autoStart)
                startAllSimultaneous.PerformClick();
        }

        private void PiracyHide_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();
            Environment.Exit(0);
        }

        private void startAllSimultaneous_Click(object sender, EventArgs e)
        {
            if (startAllSimultaneous.Text.Equals(lang.Get("Piracy", "StartAllSimultaneous")))
            {
                foreach (RunUser user in usersList)
                {
                    try
                    {
                        user.canRunThreadCaptcha = true;
                        user.AddToLog(lang.Get("Piracy", "StartingCapturing"));
                    }
                    catch (System.Net.WebException exp) { }
                }
                startAllSimultaneous.Text = lang.Get("Piracy", "StopAll");
            } else
            {
                foreach (RunUser user in usersList)
                {
                    try
                    {
                        user.canRunThreadCaptcha = false;
                        user.AddToLog(lang.Get("Piracy", "EndManufactur"));
                    }
                    catch (System.Net.WebException exp) { }
                }
                startAllSimultaneous.Text = lang.Get("Piracy", "StartAllSimultaneous");
            }
        }

        private void users_ItemActivate(object sender, EventArgs e)
        {
            usersList[users.SelectedItems[0].Index].ShowInfo(null, null);
        }

        private void users_MouseClick(object sender, MouseEventArgs e)
        {
            bool match = false;

            if (MouseButtons.Right == e.Button || MouseButtons.Left == e.Button)
            {
                foreach (ListViewItem item in users.Items)
                {
                    
                    if (item.Bounds.Contains(new Point(e.X, e.Y)))
                    {
                        ToolStripLabel title = new ToolStripLabel(item.SubItems[0].Text + " | " + (usersList[item.Index].canRunThreadCaptcha ? lang.Get("Misc", "Yes") : lang.Get("Misc", "No")));
                        title.Enabled = false;

                        ToolStripItem[] menuItems = null;
                        
                        if (usersList[item.Index].enableSelect)
                        {
                            ToolStripMenuItem info = new ToolStripMenuItem(lang.Get("Misc", "Info"));
                            info.Click += usersList[item.Index].ShowInfo;

                            ToolStripMenuItem relogin = new ToolStripMenuItem(lang.Get("Misc", "Relogin"));
                            relogin.Click += usersList[item.Index].Relogin_Click;

                            ToolStripMenuItem refresh = new ToolStripMenuItem(lang.Get("Misc", "Refresh"));
                            refresh.Click += usersList[item.Index].Refresh_Click;

                            ToolStripMenuItem highscore = new ToolStripMenuItem(lang.Get("Piracy", "HighScore"));
                            highscore.Click += usersList[item.Index].HighScore_Click;

                            ToolStripMenuItem stop = new ToolStripMenuItem(lang.Get("Misc", "Stop"));
                            stop.Click += usersList[item.Index].Stop_Click;

                            ToolStripMenuItem resume = new ToolStripMenuItem(lang.Get("Misc", "Resume"));
                            resume.Click += usersList[item.Index].Resume_Click;

                            ToolStripMenuItem raid = new ToolStripMenuItem(lang.Get("Piracy", "SendRaid"));
                            raid.Click += usersList[item.Index].Raid_Click;

                            ToolStripMenuItem train = new ToolStripMenuItem(lang.Get("Piracy", "TrainCrew"));
                            train.Click += usersList[item.Index].Train_Click;

                            ToolStripMenuItem city = new ToolStripMenuItem(lang.Get("Piracy", "ChangeCurrentCity"));
                            city.Click += usersList[item.Index].City_Click;

                            ToolStripMenuItem treaty = new ToolStripMenuItem(lang.Get("Piracy", "SendCulturalAssetTreaty"));
                            treaty.Click += usersList[item.Index].SendCulturalAssetTreaty;

                            ToolStripMenuItem remove = new ToolStripMenuItem(lang.Get("Misc", "RemoveFromList"));
                            remove.Click += usersList[item.Index].Remove_Click;

                            ToolStripMenuItem open = new ToolStripMenuItem(lang.Get("Piracy", "Open"));
                            open.Click += usersList[item.Index].Open_Click;

                            menuItems = new ToolStripItem[] { title, new ToolStripSeparator(), info, relogin, refresh, highscore, stop, resume, raid, train, city, treaty, remove, open };
                        }
                        else
                        {
                            ToolStripMenuItem busy = new ToolStripMenuItem(lang.Get("Misc", "Busy"));
                            busy.Enabled = false;

                            ToolStripMenuItem info = new ToolStripMenuItem(lang.Get("Misc", "Info"));
                            info.Click += usersList[item.Index].ShowInfo;

                            ToolStripMenuItem relogin = new ToolStripMenuItem(lang.Get("Misc", "Relogin"));
                            relogin.Click += usersList[item.Index].Relogin_Click;

                            menuItems = new ToolStripItem[] { title, new ToolStripSeparator(), info, relogin, busy };
                        }

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
            }
        }

        private class RunUser
        {
            private Ikariam.User user;

            private string userName, password, server_address, UA, proxy, type;
            private bool autoConvert, isSearchingCities, isMaxPerDay, isSolvingCaptcha;
            private int id, missionSelect;
            private Random random;
            private CaptchaSolver captchaSolver = null;
            private DateTime endDate, startDate;
            private List<Ikariam.City> cities;

            public bool canRunThreadCaptcha, enableSelect;

            public RunUser(int id, string userName, string password, string server_address, string missionSelect, string proxy, string UA, bool autoConvert, DateTime endDate, DateTime startDate, string type)
            {
                this.id = id;
                this.autoConvert = autoConvert;
                this.startDate = startDate;
                this.endDate = endDate;
                this.missionSelect = int.Parse(missionSelect);
                this.userName = userName;
                this.password = password;
                this.server_address = server_address;
                this.UA = UA;
                this.proxy = proxy;
                this.type = type;
                user = new Ikariam.User(server_address, userName, password, new Ikariam.Connection(UA, proxy));
                isMaxPerDay = false;
                isSolvingCaptcha = false;
                isSearchingCities = false;
                canRunThreadCaptcha = false;

                captchaSolver = CaptchaFactory.Captcha(
                        Settings.Instance.Get("CaptchaProvider", "Manual"),
                        Settings.Instance.Get("CaptchaUsername", ""),
                        Settings.Instance.Get("CaptchaPassword", "")
                    );
                AddToLog(captchaSolver.provider + " " + lang.Get("Piracy", "Balance") + ": " + captchaSolver.GetBalance());

                new Thread(delegate ()
                {
                    random = new Random();
                    Login();
                }).Start();
            }

            private void Login(bool login=true)
            {
                AddToLog(lang.Get("Misc", "LoginToUser") + " " + userName + " - " + server_address);
                SetLastUpdate(DateTime.Now.ToString(Config.TIME_FORMAT));
                enableSelect = false;
                
                bool canLogin = true;
                if (login)
                {
                    canLogin = user.Login();
                }
                else
                {
                    canLogin = user.Relogin();
                }

                if (!canLogin)
                {
                    AddToLog(lang.Get("Misc", "Error") + " " + lang.Get("Misc", "LoginToUser"));
                    if (!users.InvokeRequired)
                        users.Items[id].BackColor = ColorTranslator.FromHtml("#" + Settings.Instance.Get("LoginErrorColour", "ffff00"));
                    else
                        users.BeginInvoke(new Action(() => { users.Items[id].BackColor = ColorTranslator.FromHtml("#" + Settings.Instance.Get("LoginErrorColour", "ffff00")); }));
                    return;
                }


                AddToLog(lang.Get("Piracy", "FetchingAllCities"));
                cities = user.FetchAllCities();
                foreach (Ikariam.City city in cities)
                {
                    if (city.HasPirate)
                    {
                        user.SetCurrentCity(city);

                        SetCityID(city.ID);
                        SetIslandID(city.IslandId);
                        break;
                    }
                }

                RefreshData();
                
                AddToLog(lang.Get("Misc", "LoggedIn"));
                SetLastUpdate(DateTime.Now.ToString(Config.TIME_FORMAT));
                enableSelect = true;
                
                if (!user.IsEMailVerified())
                {
                    new Thread(delegate ()
                    {
                        while (true)
                        {
                            if (!users.InvokeRequired)
                                users.Items[id].BackColor = ColorTranslator.FromHtml("#" + Settings.Instance.Get("WarningColour", "f46700"));
                            else
                                users.BeginInvoke(new Action(() => { users.Items[id].BackColor = ColorTranslator.FromHtml("#" + Settings.Instance.Get("WarningColour", "f46700")); }));
                            Thread.Sleep(1000);
                        }
                    }).Start();
                }

                if (type.Equals("Manufacture"))
                    ClickPirateAsync();
                else
                    RaidCities(Settings.Instance.Get("RaidsPlaceHolders" + userName + server_address.Replace("-", ""), "").Split(','), true);
            }

            private void RefreshData()
            {
                if (user.IsUnderAttack())
                {
                    //UNDER ATTACK
                    phone.SendPush(Config.PROGRAM_NAME + " - " + userName + ", " + server_address, lang.Get("Piracy", "UnderAttack"));

                    if (!users.InvokeRequired)
                        users.Items[id].BackColor = ColorTranslator.FromHtml("#" + Settings.Instance.Get("AlertColour", "ff0000"));
                    else
                        users.BeginInvoke(new Action(() => { users.Items[id].BackColor = ColorTranslator.FromHtml("#" + Settings.Instance.Get("AlertColour", "ff0000")); }));
                }
                else
                {
                    if (!users.InvokeRequired)
                        users.Items[id].BackColor = Color.White;
                    else
                        users.BeginInvoke(new Action(() => { users.Items[id].BackColor = Color.White; }));
                }
                
                SetLastUpdate(DateTime.Now.ToString(Config.TIME_FORMAT));
                SetCapturePoints(user.GetCapturePoints());
                SetCrewStrength(user.GetCrewStrength());
                SetCityID(user.GetCurrentCity().ID);
                SetIslandID(user.GetCurrentCity().IslandId);
            }

            public void Refresh_Click(object sender, EventArgs e)
            {
                new Thread(delegate()
                {
                    RefreshData();
                }).Start();
            }

            public void AddToLog(string msg)
            {
                if (!users.InvokeRequired)
                    users.Items[id].SubItems[10].Text = msg;
                else
                    users.BeginInvoke(new Action(() => { users.Items[id].SubItems[10].Text = msg; }));
            }

            private void SetCapturePoints(string value)
            {
                if (!users.InvokeRequired)
                    users.Items[id].SubItems[3].Text = value;
                else
                    users.BeginInvoke(new Action(() => { users.Items[id].SubItems[3].Text = value; }));
            }

            private void SetCrewStrength(string value)
            {
                if (!users.InvokeRequired)
                    users.Items[id].SubItems[4].Text = value;
                else
                    users.BeginInvoke(new Action(() => { users.Items[id].SubItems[4].Text = value; }));
            }

            private void SetCityID(string value)
            {
                if (!users.InvokeRequired)
                    users.Items[id].SubItems[6].Text = value;
                else
                    users.BeginInvoke(new Action(() => { users.Items[id].SubItems[6].Text = value; }));
            }

            private void SetStartTime(string value)
            {
                if (!users.InvokeRequired)
                    users.Items[id].SubItems[8].Text = value;
                else
                    users.BeginInvoke(new Action(() => { users.Items[id].SubItems[8].Text = value; }));
            }

            private void SetEndTime(string value)
            {
                if (!users.InvokeRequired)
                    users.Items[id].SubItems[9].Text = value;
                else
                    users.BeginInvoke(new Action(() => { users.Items[id].SubItems[9].Text = value; }));
            }

            private void SetLastUpdate(string value)
            {
                if (!users.InvokeRequired)
                    users.Items[id].SubItems[11].Text = value;
                else
                    users.BeginInvoke(new Action(() => { users.Items[id].SubItems[11].Text = value; }));
            }

            private void SetIslandID(string value)
            {
                if (!users.InvokeRequired)
                    users.Items[id].SubItems[7].Text = value;
                else
                    users.BeginInvoke(new Action(() => { users.Items[id].SubItems[7].Text = value; }));
            }

            private void SetAutoConvert(bool value)
            {
                if (!users.InvokeRequired)
                    users.Items[id].SubItems[5].Text = value.ToString();
                else
                    users.BeginInvoke(new Action(() => { users.Items[id].SubItems[5].Text = value.ToString(); }));
            }

            private string GetCapturePoints()
            {
                if (!users.InvokeRequired)
                    return (users.Items[id].SubItems[3].Text);
                return (string)users.Invoke(
                    new Func<string>(() => GetCapturePoints())
                );
            }

            private string GetCrewStrength()
            {
                if (!users.InvokeRequired)
                    return users.Items[id].SubItems[4].Text;
                return (string)users.Invoke(
                    new Func<string>(() => GetCapturePoints())
                );
            }

            public string GetCurrentCityId()
            {
                return user.GetCurrentCity().ID;
            }

            public string GetCurrentIslandId()
            {
                return user.GetCurrentCity().IslandId;
            }

            public void Remove_Click(object sender, EventArgs e)
            {
                try
                {
                    users.Items.RemoveAt(id);
                    usersList.RemoveAt(id);

                    string[] lines = File.ReadAllLines("massiveUsers.users");
                    List<string> users_deleted = new List<string>();
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (i != id)
                            users_deleted.Add(lines[i]);
                    }
                    File.WriteAllLines("massiveUsers.users", users_deleted.ToArray());
                }
                catch (Exception ex) { }
            }

            public void ClickPirate()
            {
                if (isSearchingCities)
                    return;
                RefreshData();
                if (!canRunThreadCaptcha || isSolvingCaptcha)
                    return;

                int capturePointsTodayInt = user.GetCapturePointsToday();
                isMaxPerDay = capturePointsTodayInt < maxPerDay;

                if (!isMaxPerDay)
                {
                    AddToLog(lang.Get("Piracy", "MaxPoints") + " (" + capturePointsTodayInt.ToString() + " / " + maxPerDay.ToString() + ")");
                    return;
                }

                SetLastUpdate(DateTime.Now.ToString(Config.TIME_FORMAT));

                if (user.IsCrewOnGoingMission())
                {
                    AddToLog(lang.Get("Piracy", "CrewIn") + " " + user.GetOnGoingMissionType());
                    return;
                }

                TimeSpan start = new TimeSpan(startDate.Hour, startDate.Minute, startDate.Second);
                TimeSpan end = new TimeSpan(endDate.Hour, endDate.Minute, endDate.Second);
                TimeSpan now = DateTime.Now.TimeOfDay;

                bool canProduce = (start < end && now > start && now < end) || (start > end && (now < end || now > start));

                if (canProduce)
                {
                    if (!user.GetCapturePoints().Equals(CANT_FETCH))
                    {
                        AddToLog(lang.Get("Piracy", "RunningMission") + " " + (missionSelect + 1).ToString());

                        isSolvingCaptcha = true;
                        user.PirateMission((2 * missionSelect + 1).ToString(), captchaSolver);
                        isSolvingCaptcha = false;
                        
                        if (autoConvert)
                        {
                            AddToLog(lang.Get("Piracy", "AutoConvertToCrew"));

                            int force = 0;
                            int.TryParse(user.GetCapturePoints(), out force);
                            user.ConvertToCrew(force / 10);
                        }
                        
                    }
                    else
                    {
                        AddToLog(lang.Get("Misc", "CantFetch"));
                    }
                }
                else
                {
                    AddToLog(lang.Get("Piracy", "TimeLimit"));
                }
            }

            public void ClickPirateAsync()
            {
                new Thread(delegate ()
                {
                    Thread.Sleep(random.Next(1000, 1500));
                    while (true)
                    {
                        ClickPirate();
                        Thread.Sleep(150000 + random.Next(10000, 12000));
                    }
                }).Start();
            }

            public void ShowInfo(object sender, EventArgs e)
            {
                using (var form = new User(userName, password, server_address, missionSelect, UA, proxy, autoConvert, startDate.ToString(Config.DATE_TIME_FORMAT), endDate.ToString(Config.DATE_TIME_FORMAT)))
                {
                    var result = form.ShowDialog();
                    if (DialogResult.OK == result)
                    {
                        startDate = form.start;
                        endDate = form.end;
                        autoConvert = form.autoConvertCrew;

                        SetStartTime(startDate.ToString(Config.TIME_FORMAT));
                        SetEndTime(endDate.ToString(Config.TIME_FORMAT));
                        SetAutoConvert(autoConvert);

                        string[] lines = File.ReadAllLines("massiveUsers.users");
                        string[] data = {
                            userName,
                            password,
                            server_address,
                            missionSelect.ToString(),
                            UA,
                            proxy,
                            autoConvert.ToString(),
                            startDate.ToString(Config.TIME_FORMAT),
                            endDate.ToString(Config.TIME_FORMAT),
                            type
                        };
                        lines[id] = string.Join(",", data);
                        File.WriteAllLines("massiveUsers.users", lines);
                        maxPerDay = form.max;

                        Settings.Instance.Set("MaxCapturePointsPerDay", form.max.ToString());
                        Settings.Instance.Commit();
                    }
                }
            }

            public void Relogin_Click(object sender, EventArgs e)
            {
                new Thread(delegate ()
                {
                    Login(false);
                }).Start();
            }

            public void Stop_Click(object sender, EventArgs e)
            {
                users.ContextMenuStrip.Close();
                canRunThreadCaptcha = false;
                AddToLog(lang.Get("Misc", "Stop"));
            }

            public void Resume_Click(object sender, EventArgs e)
            {
                users.ContextMenuStrip.Close();
                canRunThreadCaptcha = true;
                AddToLog(lang.Get("Misc", "Resume"));
            }

            public void Raid_Click(object sender, EventArgs e)
            {
                users.ContextMenuStrip.Close();
                canRunThreadCaptcha = false;
                RefreshData();

                //if (!hasOngoingMission)
                //{
                new Thread(delegate ()
                {
                    isSearchingCities = true;
                    AddToLog(lang.Get("Piracy", "FetchingAllCities"));
                    List<UserItem> users = new List<UserItem>();
                    List<string[]> cities = new List<string[]>();

                    foreach (Ikariam.City city in this.cities)
                    {
                        cities.Add(new string[] { city.ID, city.Name, city.Cords, (city.HasPirate ? lang.Get("Misc", "Yes") : lang.Get("Misc", "No")) });
                    }

                    foreach (RunUser user in usersList)
                    {
                        if (user.userName != userName || user.server_address != server_address)
                            users.Add(new UserItem(user.userName, user.server_address, user.GetCurrentCityId(), user.GetCurrentIslandId()));
                    }
                    string placeholder = Settings.Instance.Get("RaidsPlaceHolders" + userName + server_address.Replace("-", ""), "");

                    using (var form = new SelectUsers(users, cities, placeholder, userName, server_address))
                    {
                        var result = form.ShowDialog();
                        if (DialogResult.OK == result)
                        {
                            RaidCities(form.result.Split(','), form.repeat);
                        }
                        else
                        {
                            canRunThreadCaptcha = true;
                        }
                    }
                    isSearchingCities = false;
                }).Start();
            }

            private void RaidCities(string[] cities, bool repeat)
            {
                canRunThreadCaptcha = false;
                if (!type.Equals("Raider"))
                    enableSelect = false;
                new Thread(delegate ()
                {
                    while (true)
                    {
                        if (user.IsCrewOnGoingMission())
                        {
                            RefreshData();
                            AddToLog(lang.Get("Piracy", "WaitingForPirates"));
                            Thread.Sleep(60000);
                            continue;
                        }

                        do
                        {
                            TimeSpan start = new TimeSpan(startDate.Hour, startDate.Minute, startDate.Second);
                            TimeSpan end = new TimeSpan(endDate.Hour, endDate.Minute, endDate.Second);
                            TimeSpan now = DateTime.Now.TimeOfDay;

                            bool canRaid = (start < end && now > start && now < end) || (start > end && (now < end || now > start));
                            if (type.Equals("Raider") && !canRaid)
                            {
                                AddToLog(lang.Get("Piracy", "TimeLimit"));
                                Thread.Sleep(TimeSpan.FromMinutes(2));
                                continue;
                            }

                            if (autoConvert)
                            {
                                AddToLog(lang.Get("Piracy", "AutoConvertToCrew"));

                                int force = 0;
                                int.TryParse(user.GetCapturePoints(), out force);
                                user.ConvertToCrew(force/10);
                            }

                            foreach (string city in cities)
                            {
                                if (2 == city.Split('|').Length)
                                {
                                    AddToLog(lang.Get("Piracy", "Raiding") + " " + city.Split('|')[0]);
                                    user.RaidCity(city.Split('|')[0], city.Split('|')[1]);

                                    do
                                    {
                                        SetLastUpdate(DateTime.Now.ToString(Config.TIME_FORMAT));
                                        RefreshData();
                                        Thread.Sleep(THREE_MINUTES + random.Next(10000, 12000));
                                    } while (user.IsCrewOnGoingMission());
                                }
                                else if (1 == city.Split('|').Length)
                                {
                                    AddToLog(lang.Get("Piracy", "ChangesToCity") + " " + city.Split('|')[0]);
                                    user.SetCurrentCity(city);
                                    RefreshData();
                                }
                            }
                        } while (repeat);
                        AddToLog(lang.Get("Piracy", "RaidsCompleted"));
                        //canRunThreadCaptcha = true;
                        enableSelect = true;
                        break;
                    }
                }).Start();
            }

            public void City_Click(object sender, EventArgs e)
            {
                //canRunThreadCaptcha = false;
                if (!isSearchingCities)
                {
                    new Thread(delegate ()
                    {
                        isSearchingCities = true;
                        AddToLog(lang.Get("Piracy", "FetchingAllCities"));
                        
                        List<string[]> cities = new List<string[]>();
                        foreach (Ikariam.City city in this.cities)
                        {
                            cities.Add(new string[] { city.ID, city.Name, city.Cords, (city.HasPirate ? lang.Get("Misc", "Yes") : lang.Get("Misc", "No")) });
                        }

                        using (var form = new SelectCity(cities))
                        {
                            var result = form.ShowDialog();
                            if (DialogResult.OK == result)
                            {
                                user.SetCurrentCity(form.cityID);
                            }
                            else if (DialogResult.No == result)
                            {
                                user.BuildBuilding(Ikariam.City.BUILDINGS.PIRATE_FORTRESS, Ikariam.City.POSITIONS.PIRATE_FORTRESS);
                                user.BuildingSpeedUp(Ikariam.City.POSITIONS.PIRATE_FORTRESS);
                                user.BuildingSpeedUp(Ikariam.City.POSITIONS.PIRATE_FORTRESS);

                                user.SetCurrentCity(form.cityID);
                            }
                            else if (DialogResult.Retry == result)
                            {
                                user.UpgradeBuilding(Ikariam.City.POSITIONS.PIRATE_FORTRESS);

                                user.SetCurrentCity(form.cityID);
                            }
                        }
                        RefreshData();
                        AddToLog(lang.Get("Piracy", "ChangesToCity") + " " + user.GetCurrentCity().Name);

                        //canRunThreadCaptcha = true;
                        isSearchingCities = false;
                    }).Start();
                }
            }

            public void SendCulturalAssetTreaty(object sender, EventArgs e)
            {
                enableSelect = false;
                isSearchingCities = true;
                canRunThreadCaptcha = false;
                new Thread(delegate()
                {
                    using (var form = new NumberPicker(lang.Get("Piracy", "SendCulturalAssetTreaty"), 5, IkariamPlus.Resources.icon_message))
                    {
                        var result = form.ShowDialog();
                        AddToLog(lang.Get("Misc", "Search") + "...");
                        if (DialogResult.OK == result)
                        {
                            int x = 0;
                            for (int i = 1; x < 5; i++)
                            {
                                List<string> users = user.GetHighScoreTable(i);

                                foreach (string userTop in users)
                                {
                                    if (!user.IsHaveCulturalAssetTreaty(userTop))
                                    {
                                        user.SendCulturalAssetTreaty(userTop);
                                        x++;
                                    }
                                    if (x >= int.Parse(form.number))
                                        break;
                                }
                                if (x >= int.Parse(form.number))
                                    break;
                            }
                        }
                        AddToLog(lang.Get("WebView", "Finished"));
                    }
                    enableSelect = true;
                    isSearchingCities = false;
                    canRunThreadCaptcha = true;
                }).Start();
                
            }

            public void Train_Click(object sender, EventArgs e)
            {
                int force = 0;
                int.TryParse(GetCapturePoints(), out force);
                if (CANT_FETCH != GetCapturePoints() && (force / 10) > 1)
                {
                    using (var form = new NumberPicker(lang.Get("Piracy", "TrainCrew"), (int.Parse(GetCapturePoints()) / 10), IkariamPlus.Resources.piratecrew))
                    {
                        var result = form.ShowDialog();
                        if (DialogResult.OK == result)
                        {
                            AddToLog(lang.Get("Piracy", "AutoConvertToCrew"));

                            user.ConvertToCrew(int.Parse(form.number));
                        }
                    }
                }
            }

            public void Open_Click(object sender, EventArgs e)
            {
                Settings settings = Settings.Instance;
                settings.Set("ProgramMode", "Login");
                settings.Set("ServerGame", server_address);
                settings.Set("UserAgent", UA);
                settings.Set("UserName", userName);
                settings.Set("Password", password);
                settings.Set("EnableProxy", (proxy.Equals("") || proxy.Equals("None") ? "false" : "true"));
                settings.Set("ProxyAddress", proxy);

                System.Diagnostics.Process.Start("IkariamPlus.exe");

                try
                {
                    users.Items.RemoveAt(id);
                    usersList.RemoveAt(id);
                }
                catch (Exception ex) { }
            }

            public void HighScore_Click(object sender, EventArgs e)
            {
                PiracyInfo piracyInfo = new PiracyInfo(server_address, id);
                piracyInfo.UpdateTop(user.GetPiracyTop());
                piracyInfo.UpdateTimeEnd(user.GetPiracyTimeEnd(), server_address);

                if (!piracyInfo.InvokeRequired)
                {
                    piracyInfo.Show();
                    piracyInfo.BringToFront();
                }
                else
                    piracyInfo.BeginInvoke(new Action(() => { piracyInfo.Show(); piracyInfo.BringToFront(); }));
            }
        }
    }
    /*
     * private class RunUser
        {
            private PlayableWebClient client;
            private string actionRequest, cityId, missionLevel, parameters, url, response, userName, password, server_address, UA, proxy, ongoingMissionType, islandID, type;
            private bool autoConvert, hasOngoingMission, isSearchingCities, isMaxPerDay, isSolvingCaptcha;
            private int id, missionSelect;
            private Random random;
            private CaptchaSolver captchaSolver = null;
            private DateTime endDate, startDate;
            private List<CityDetails> allCities;

            public bool canRunThreadCaptcha, enableSelect;

            public RunUser(int id, string userName, string password, string server_address, string missionSelect, string proxy, string UA, bool autoConvert, DateTime endDate, DateTime startDate, string type)
            {
                this.id = id;
                this.autoConvert = autoConvert;
                this.startDate = startDate;
                this.endDate = endDate;
                this.missionSelect = int.Parse(missionSelect);
                this.userName = userName;
                this.password = password;
                this.server_address = server_address;
                this.UA = UA;
                this.proxy = proxy;
                this.type = type;
				isMaxPerDay = false;
                isSolvingCaptcha = false;
                allCities = new List<CityDetails>();
                isSearchingCities = false;
                canRunThreadCaptcha = false;

                new Thread(delegate ()
                {
                    client = new PlayableWebClient();
                    client.SetUA(UA);
                    if (proxy != "None" && proxy != "")
                        client.SetProxy(proxy);
                    url = Config.PROTOCOL + string.Format("://{0}.ikariam.gameforge.com/index.php", server_address);
                    random = new Random();
                    Login();
                }).Start();
            }

            private void Login()
            {
                AddToLog(lang.Get("Misc", "LoginToUser") + " " + userName + " - " + server_address);
                SetLastUpdate(DateTime.Now.ToString(Config.TIME_FORMAT));
                enableSelect = false;

                try
                {
                    //string response = client.DownloadString("?view=city&action=loginAvatar&function=login&", server_address, userName, password));
                    string response = client.UploadString(url + "?view=city&action=loginAvatar&function=login", "name=" + userName + "&password=" + password + "&uni_url=" + server_address + ".ikariam.gameforge.com&pwat_uid&pwat_checksum&startPageShown=1&detectedDevice=1&kid&autoLogin=on");
                    
                    cityId = Regex.Match(response, "cityId=\\s*(.+?)\\s*\"").Groups[1].Value;
                    UpdateActionRequest(response);
                }
                catch (Exception exc) { AddToLog(exc.ToString()); }

                parameters = string.Format("view=city&actionRequest={0}&ajax=1", actionRequest);
                try
                {
                    response = client.DownloadString(url + "?" + parameters);
                    if (response.Contains("[[\"changeView\",[\"error\""))
                    {
                        AddToLog(lang.Get("Misc", "Error") + " " + lang.Get("Misc", "LoginToUser"));
                        if (!users.InvokeRequired)
                            users.Items[id].BackColor = Color.FromArgb(100, 255, 255, 0);
                        else
                            users.BeginInvoke(new Action(() => { users.Items[id].BackColor = Color.FromArgb(100, 255, 255, 0); }));
                        return;
                    }

                    UpdateActionRequest(response, false);
                }
                catch (Exception exc) { AddToLog(exc.ToString()); }

                GetAllCities();

                foreach (CityDetails city in allCities)
                {
                    if (city.hasPirate)
                    {
                        cityId = city.cityId;

                        SetCityID(cityId);
                        SetIslandID(city.islandId);
                        break;
                    }
                }
                
                RefreshData();

                if (null == captchaSolver)
                {
                    captchaSolver = CaptchaFactory.Captcha(
                        MainWindow.XMLConfig.DocumentElement.SelectSingleNode("/Bot/User/CaptcherProvider").InnerText,
                        MainWindow.XMLConfig.DocumentElement.SelectSingleNode("/Bot/User/APIKey9K").InnerText,
                        MainWindow.XMLConfig.DocumentElement.SelectSingleNode("/Bot/User/APISource9K").InnerText
                    );
                    AddToLog(captchaSolver.provider + " " + lang.Get("Piracy", "Balance") + ": " + captchaSolver.GetBalance());
                }

                SetLastUpdate(DateTime.Now.ToString(Config.TIME_FORMAT));
                enableSelect = true;
                AddToLog(lang.Get("Misc", "LoggedIn"));

                parameters = string.Format("view=options&backgroundView=city&currentCityId={1}&actionRequest={0}&ajax=1", actionRequest, cityId);
                try
                {
                    response = client.DownloadString(url + "?" + parameters);
                    if (response.Contains("emailInvalidWarning"))
                    {
                        AddToLog(lang.Get("Misc", "Error") + " " + lang.Get("Launcher", "ActiveMail"));
                        new Thread(delegate ()
                        {
                            while (true)
                            {
                                if (!users.InvokeRequired)
                                    users.Items[id].BackColor = Color.FromArgb(100, 244, 103, 0);
                                else
                                    users.BeginInvoke(new Action(() => { users.Items[id].BackColor = Color.FromArgb(100, 244, 103, 0); }));
                                Thread.Sleep(1000);
                            }
                        }).Start();
                        
                    }

                    UpdateActionRequest(response, false);
                }
                catch (Exception exc) { AddToLog(exc.ToString()); }

                if (type.Equals("Manufacture"))
                    ClickPirateAsync();
                else
                    RaidCities(Settings.Instance.Get("RaidsPlaceHolders" + userName + server_address.Replace("-", ""), "").Split(','), true);
            }

            private void GetAllCities()
            {
                MatchCollection cities_regex = null;
                parameters = string.Format("view=city&actionRequest={0}", actionRequest);
                try
                {
                    response = client.DownloadString(url + "?" + parameters);
                    cities_regex = new Regex("{\\\\\"id\\\\\":(.+?),\\\\\"name\\\\\":\\\\\"(.+?)\\\\\",\\\\\"coords\\\\\":\\\\\"(.+?)\\\\\",\\\\\"tradegood\\\\\":\\\\\"(1|2|3|4)\\\\\",\\\\\"relationship\\\\\":\\\\\"ownCity\\\\\"}").Matches(response.Replace(" ", "").Replace("\n", "").Replace("\r", ""));
                    UpdateActionRequest(response, false);
                }
                catch (Exception exc) { AddToLog(exc.ToString()); }

                try
                {
                    foreach (Match city in cities_regex)
                    {
                        if (city.Success)
                        {
                            parameters = string.Format("view=pirateFortress&activeTab=tabBootyQuest&cityId={0}&position=17&backgroundView=city&currentCityId={0}&templateView=pirateFortress&actionRequest={1}&ajax=1", city.Groups[1].Value, actionRequest);
                            try
                            {
                                response = client.DownloadString(url + "?" + parameters);

                                string cityName = Regex.Replace(
                                    city.Groups[2].Value.Replace("\\\\", "\\"),
                                    @"\\u(?<Value>[a-zA-Z0-9]{4})",
                                    m =>
                                    {
                                        return ((char)int.Parse(m.Groups["Value"].Value, System.Globalization.NumberStyles.HexNumber)).ToString();
                                    }
                                );

                                bool pirateFortress = UpdateActionRequest(response);
                                allCities.Add(new CityDetails(cityName, city.Groups[1].Value, city.Groups[3].Value, islandID, pirateFortress));
                            }
                            catch (Exception exc) { AddToLog(exc.ToString()); }

                        }
                    }
                }
                catch (Exception e) { AddToLog(e.ToString()); }
            }

            private bool UpdateActionRequest(string response, bool updateUI = true)
            {
                response = response.Replace(" ", "").Replace("\n", "").Replace("\r", "");
                GroupCollection actionRequestVal = Regex.Match(response, "\"actionRequest\":\"(.+?)\"").Groups;
                if (actionRequestVal.Count > 0 && !actionRequestVal[1].Value.Equals(""))
                    actionRequest = actionRequestVal[1].Value;

                actionRequestVal = Regex.Match(response, "actionRequest:\"(.+?)\"").Groups;
                if (actionRequestVal.Count > 0 && !actionRequestVal[1].Value.Equals(""))
                    actionRequest = actionRequestVal[1].Value;

                actionRequestVal = Regex.Match(response, "name=\"actionRequest\"value=\"(.+?)\"/>").Groups;
                if (actionRequestVal.Count > 0 && !actionRequestVal[1].Value.Equals(""))
                    actionRequest = actionRequestVal[1].Value;

                actionRequestVal = Regex.Match(response, "id=\"js_islandBread\"class=\"yellow\"href=\"\\?view=island&islandId=(.+?)\"").Groups;
                if (actionRequestVal.Count > 0 && !actionRequestVal[1].Value.Equals(""))
                    islandID = actionRequestVal[1].Value;

                if (updateUI)
                {
                    Match capturePoints = Regex.Match(response, ",\\\\\"capturePoints\\\\\":\\\\\"(.+?)\\\\\",");
                    if (capturePoints.Success)
                        SetCapturePoints(capturePoints.Groups[1].Value);

    Match crewStrength = new Regex(",\\\\\"completeCrewPoints\\\\\":(.+?),").Match(response);
                    if (crewStrength.Success)
                        SetCrewStrength(crewStrength.Groups[1].Value);


    Match missionType = new Regex(",\\\\\"ongoingMissionType\\\\\":\\\\\"(.+?)\\\\\",").Match(response);
                    if (missionType.Success)
                        ongoingMissionType = missionType.Groups[1].Value;
                    else
                        ongoingMissionType = CANT_FETCH;

                    hasOngoingMission = new Regex(",\\\\\"hasOngoingMission\\\\\":true,").Match(response).Success;

                    return crewStrength.Success && capturePoints.Success;
                }
                return false;
            }

            private void RefreshData(string city = "None")
{
    if (city.Equals("None"))
        city = cityId;
    parameters = string.Format("view=pirateFortress&activeTab=tabBootyQuest&cityId={0}&position=17&backgroundView=city&currentCityId={0}&templateView=pirateFortress&actionRequest={1}&ajax=1", city, actionRequest);
    for (int i = 0; i < 5; i++)
    {
        try
        {

            if (!client.IsBusy)
            {
                response = client.DownloadString(url + "?" + parameters);

                if (response.Replace(" ", "").Replace("\n", "").Replace("\r", "").Contains("\"advisors\":{\"military\":{\"link\":\"?view=militaryAdvisor&oldView=pirateFortress&cityId=" + cityId + "&position=17\",\"cssclass\":\"normalalert\",\"premiumlink\":\"?view=premiumDetails&oldView=pirateFortress&cityId=" + cityId + "&position=17\"},"))
                {
                    //UNDER ATTACK
                    phone.SendPush(Config.PROGRAM_NAME + " - " + userName + ", " + server_address, lang.Get("Piracy", "UnderAttack"));

                    if (!users.InvokeRequired)
                        users.Items[id].BackColor = Color.FromArgb(100, 255, 0, 0);
                    else
                        users.BeginInvoke(new Action(() => { users.Items[id].BackColor = Color.FromArgb(100, 255, 0, 0); }));
                }
                else
                {
                    if (!users.InvokeRequired)
                        users.Items[id].BackColor = Color.White;
                    else
                        users.BeginInvoke(new Action(() => { users.Items[id].BackColor = Color.White; }));
                }

                UpdateActionRequest(response);
                break;
            }
            else Thread.Sleep(5000);

        }
        catch (Exception exc) { AddToLog(exc.ToString()); Thread.Sleep(5000); }
    }
    SetLastUpdate(DateTime.Now.ToString(Config.TIME_FORMAT));
}

public void Refresh_Click(object sender, EventArgs e)
{
    RefreshData();
}

public void AddToLog(string msg)
{
    if (!users.InvokeRequired)
        users.Items[id].SubItems[10].Text = msg;
    else
        users.BeginInvoke(new Action(() => { users.Items[id].SubItems[10].Text = msg; }));
}

private void SetCapturePoints(string value)
{
    if (!users.InvokeRequired)
        users.Items[id].SubItems[3].Text = value;
    else
        users.BeginInvoke(new Action(() => { users.Items[id].SubItems[3].Text = value; }));
}

private void SetCrewStrength(string value)
{
    if (!users.InvokeRequired)
        users.Items[id].SubItems[4].Text = value;
    else
        users.BeginInvoke(new Action(() => { users.Items[id].SubItems[4].Text = value; }));
}

private void SetCityID(string value)
{
    if (!users.InvokeRequired)
        users.Items[id].SubItems[6].Text = value;
    else
        users.BeginInvoke(new Action(() => { users.Items[id].SubItems[6].Text = value; }));
}

private void SetStartTime(string value)
{
    if (!users.InvokeRequired)
        users.Items[id].SubItems[8].Text = value;
    else
        users.BeginInvoke(new Action(() => { users.Items[id].SubItems[8].Text = value; }));
}

private void SetEndTime(string value)
{
    if (!users.InvokeRequired)
        users.Items[id].SubItems[9].Text = value;
    else
        users.BeginInvoke(new Action(() => { users.Items[id].SubItems[9].Text = value; }));
}

private void SetLastUpdate(string value)
{
    if (!users.InvokeRequired)
        users.Items[id].SubItems[11].Text = value;
    else
        users.BeginInvoke(new Action(() => { users.Items[id].SubItems[11].Text = value; }));
}

private void SetIslandID(string value)
{
    if (!users.InvokeRequired)
        users.Items[id].SubItems[7].Text = value;
    else
        users.BeginInvoke(new Action(() => { users.Items[id].SubItems[7].Text = value; }));
}

private void SetAutoConvert(bool value)
{
    if (!users.InvokeRequired)
        users.Items[id].SubItems[5].Text = value.ToString();
    else
        users.BeginInvoke(new Action(() => { users.Items[id].SubItems[5].Text = value.ToString(); }));
}

private string GetCapturePoints()
{
    if (!users.InvokeRequired)
        return (users.Items[id].SubItems[3].Text);
    return (string)users.Invoke(
        new Func<string>(() => GetCapturePoints())
    );
}

private string GetCrewStrength()
{
    if (!users.InvokeRequired)
        return users.Items[id].SubItems[4].Text;
    return (string)users.Invoke(
        new Func<string>(() => GetCapturePoints())
    );
}

public void Remove_Click(object sender, EventArgs e)
{
    try
    {
        users.Items.RemoveAt(id);
        usersList.RemoveAt(id);

        string[] lines = File.ReadAllLines("massiveUsers.users");
        List<string> users_deleted = new List<string>();
        for (int i = 0; i < lines.Length; i++)
        {
            if (i != id)
                users_deleted.Add(lines[i]);
        }
        File.WriteAllLines("massiveUsers.users", users_deleted.ToArray());
    }
    catch (Exception ex) { }
}

public void ClickPirate()
{
    if (isSearchingCities)
        return;
    RefreshData();
    if (!canRunThreadCaptcha || isSolvingCaptcha)
        return;

    parameters = string.Format("view=dailyTasks&oldBackgroundView=city&cityWorldviewScale=1&backgroundView=city&currentCityId={1}&actionRequest={0}&ajax=1", actionRequest, cityId);
    int capturePointsTodayInt = 0;

    try
    {
        response = client.DownloadString(url + "?" + parameters);
        UpdateActionRequest(response, false);

        response = response.Replace(" ", "").Replace("\n", "").Replace("\r", "");

        GroupCollection capturePointsToday = Regex.Match(response, Regex.Escape("<\\/td>\\n\\n<tdclass=\\\"rightsmallright\\\">") + "([0-9,]+)" + Regex.Escape("<\\/td><tdclass=\\\"centersmall\\\">&nbsp;\\/&nbsp;<\\/td><tdclass=\\\"leftsmall\\\">1,500<")).Groups;
        GroupCollection capturePointsTodayLowOption = Regex.Match(response, Regex.Escape("<\\/td>\\n\\n<tdclass=\\\"rightsmallright\\\">") + "([0-9,]+)" + Regex.Escape("<\\/td><tdclass=\\\"centersmall\\\">&nbsp;\\/&nbsp;<\\/td><tdclass=\\\"leftsmall\\\">500<\\/td><tdclass=\\\"leftsmallleft\\\"><imgsrc=\\\"skin\\/resources\\/capturePoints_small.png")).Groups;
        if (capturePointsToday.Count > 0 && !capturePointsToday[1].Value.Equals(""))
        {
            capturePointsTodayInt = int.Parse(capturePointsToday[1].Value.Replace(",", ""));

            isMaxPerDay = capturePointsTodayInt < maxPerDay;

            if (!isMaxPerDay)
            {
                AddToLog(lang.Get("Piracy", "MaxPoints") + " (" + capturePointsTodayInt.ToString() + " / " + maxPerDay.ToString() + ")");
                return;
            }

        }
        else if (capturePointsTodayLowOption.Count > 0 && !capturePointsTodayLowOption[1].Value.Equals(""))
        {
            capturePointsTodayInt = int.Parse(capturePointsTodayLowOption[1].Value.Replace(",", ""));

            isMaxPerDay = capturePointsTodayInt < maxPerDay;

            if (!isMaxPerDay)
            {
                AddToLog(lang.Get("Piracy", "MaxPoints") + " (" + capturePointsTodayInt.ToString() + " / " + maxPerDay.ToString() + ")");
                return;
            }

        }
        else
        {
            AddToLog(lang.Get("Piracy", "CantFetchToday"));
        }
    }
    catch (Exception exc) { AddToLog(exc.ToString()); }

    SetLastUpdate(DateTime.Now.ToString(Config.TIME_FORMAT));

    if (hasOngoingMission)
    {
        AddToLog(lang.Get("Piracy", "CrewIn") + " " + ongoingMissionType);
        return;
    }

    TimeSpan start = new TimeSpan(startDate.Hour, startDate.Minute, startDate.Second);
    TimeSpan end = new TimeSpan(endDate.Hour, endDate.Minute, endDate.Second);
    TimeSpan now = DateTime.Now.TimeOfDay;

    bool canProduce = (start < end && now > start && now < end) || (start > end && (now < end || now > start));

    if (canProduce)
    {
        if (!GetCapturePoints().Equals(CANT_FETCH))
        {
            missionLevel = (2 * missionSelect + 1).ToString();
            AddToLog(lang.Get("Piracy", "RunningMission") + " " + (missionSelect + 1).ToString());


            parameters = string.Format("action=PiracyScreen&function=capture&buildingLevel={2}&view=pirateFortress&cityId={1}&position=17&activeTab=tabBootyQuest&backgroundView=city&currentCityId={1}&templateView=pirateFortress&actionRequest={0}", actionRequest, cityId, missionLevel);
            try
            {
                response = client.DownloadString(url + "?" + parameters);

                UpdateActionRequest(response);
            }
            catch (Exception exc) { AddToLog(exc.ToString()); }

            if (response.Contains("captchaNeeded=1"))
            {
                AddToLog(lang.Get("Piracy", "CaptchaDetected"));
                byte[] captchaData = null;
                try
                {
                    captchaData = client.DownloadData(url + "?action=Options&function=createCaptcha&rand=" + random.Next(0, 99999).ToString());
                }
                catch (Exception exc) { AddToLog(exc.ToString()); }

                //Captcha solve

                if (canRunThreadCaptcha)
                {
                    isSolvingCaptcha = true;
                    captchaSolver.image = captchaData;

                    string captchaID;
                    bool success;
                    string captchaValue = captchaSolver.SendCaptcha(out captchaID, out success);
                    if (!success)
                    {
                        AddToLog(lang.Get("Piracy", "CaptchaRecognazationFailed") + ": " + captchaValue);
                        captchaValue = "None";
                    }
                    else
                    {
                        AddToLog(lang.Get("Piracy", "CaptchaValue") + ": " + captchaValue);
                    }

                    //parameters = "action=PiracyScreen&function=capture&cityId=" + cityId + "&position=17&captchaNeeded=1&activeTab=tabBootyQuest&backgroundView=city&templateView=pirateFortress&currentCityId=" + cityId + "&buildingLevel=" + missionLevel + "&ajax=1&captcha=" + captchaValue + "&actionRequest=" + actionRequest;
                    parameters = string.Format("action=PiracyScreen&function=capture&cityId={0}&position=17&captchaNeeded=1&activeTab=tabBootyQuest&backgroundView=city&templateView=pirateFortress&currentCityId={0}&buildingLevel={1}&ajax=1&captcha={2}&actionRequest={3}", cityId, missionLevel, captchaValue, actionRequest);
                    try
                    {
                        response = client.DownloadString(url + "?" + parameters);
                    }
                    catch (Exception exc) { AddToLog(exc.ToString()); }

                    if (success)
                        captchaSolver.CaptchaWorked(captchaID, !response.Contains("captchaNeeded=1"));

                    Thread.Sleep(10000);
                    isSolvingCaptcha = false;
                }
            }
            else
            {
                if (autoConvert)
                {
                    AddToLog(lang.Get("Piracy", "AutoConvertToCrew"));

                    int force = 0;
                    int.TryParse(GetCapturePoints(), out force);
                    parameters = string.Format("action=PiracyScreen&function=convert&view=pirateFortress&crewPoints={2}&cityId={1}&position=17&activeTab=tabCrew&backgroundView=city&currentCityId={1}&templateView=pirateFortress&actionRequest={0}&ajax=1", actionRequest, cityId, (force / 10).ToString());
                    try
                    {
                        response = client.DownloadString(url + "?" + parameters);

                        UpdateActionRequest(response, false);
                    }
                    catch (Exception exc) { AddToLog(exc.ToString()); }
                }
            }
        }
        else
        {
            AddToLog(lang.Get("Misc", "CantFetch"));
        }
    }
    else
    {
        AddToLog(lang.Get("Piracy", "TimeLimit"));
    }
}

public void ClickPirateAsync()
{
    new Thread(delegate ()
    {
        Thread.Sleep(random.Next(1000, 1500));
        while (true)
        {
            ClickPirate();
            Thread.Sleep(150000 + random.Next(10000, 12000));
        }
    }).Start();
}

public void ShowInfo(object sender, EventArgs e)
{
    using (var form = new User(userName, password, server_address, missionSelect, UA, proxy, autoConvert, startDate.ToString(Config.DATE_TIME_FORMAT), endDate.ToString(Config.DATE_TIME_FORMAT)))
    {
        var result = form.ShowDialog();
        if (DialogResult.OK == result)
        {
            startDate = form.start;
            endDate = form.end;
            autoConvert = form.autoConvertCrew;

            SetStartTime(startDate.ToString(Config.TIME_FORMAT));
            SetEndTime(endDate.ToString(Config.TIME_FORMAT));
            SetAutoConvert(autoConvert);

            string[] lines = File.ReadAllLines("massiveUsers.users");
            string[] data = {
                            userName,
                            password,
                            server_address,
                            missionSelect.ToString(),
                            UA,
                            proxy,
                            autoConvert.ToString(),
                            startDate.ToString(Config.TIME_FORMAT),
                            endDate.ToString(Config.TIME_FORMAT),
                            type
                        };
            lines[id] = string.Join(",", data);
            File.WriteAllLines("massiveUsers.users", lines);
            maxPerDay = form.max;

            Settings.Instance.Set("MaxCapturePointsPerDay", form.max.ToString());
            Settings.Instance.Commit();
        }
    }
}

public void Relogin_Click(object sender, EventArgs e)
{
    new Thread(delegate ()
    {
        client = new PlayableWebClient();
        client.SetUA(UA);
        if (proxy != "None" && proxy != "")
            client.SetProxy(proxy);
        Login();
    }).Start();
}

public void Stop_Click(object sender, EventArgs e)
{
    users.ContextMenuStrip.Close();
    canRunThreadCaptcha = false;
    AddToLog(lang.Get("Misc", "Stop"));
}

public void Resume_Click(object sender, EventArgs e)
{
    users.ContextMenuStrip.Close();
    canRunThreadCaptcha = true;
    AddToLog(lang.Get("Misc", "Resume"));
}

public void Raid_Click(object sender, EventArgs e)
{
    users.ContextMenuStrip.Close();
    canRunThreadCaptcha = false;
    RefreshData();

    //if (!hasOngoingMission)
    //{
    new Thread(delegate ()
    {
        isSearchingCities = true;
        AddToLog(lang.Get("Piracy", "FetchingAllCities"));
        List<UserItem> users = new List<UserItem>();
        List<string[]> cities = new List<string[]>();

        foreach (CityDetails city in allCities)
        {
            cities.Add(new string[] { city.cityId, city.name, city.cords, (city.hasPirate ? lang.Get("Misc", "Yes") : lang.Get("Misc", "No")) });
        }

        foreach (RunUser user in usersList)
        {
            if (user.userName != userName || user.server_address != server_address)
                users.Add(new UserItem(user.userName, user.server_address, user.cityId, user.islandID));
        }
        string placeholder = Settings.Instance.Get("RaidsPlaceHolders" + userName + server_address.Replace("-", ""), "");

        using (var form = new SelectUsers(users, cities, placeholder, userName, server_address))
        {
            var result = form.ShowDialog();
            if (DialogResult.OK == result)
            {
                RaidCities(form.result.Split(','), form.repeat);
            }
            else
            {
                canRunThreadCaptcha = true;
            }
        }
        isSearchingCities = false;
    }).Start();
}

private void RaidCities(string[] cities, bool repeat)
{
    canRunThreadCaptcha = false;
    if (!type.Equals("Raider"))
        enableSelect = false;
    new Thread(delegate ()
    {
        while (true)
        {
            if (hasOngoingMission)
            {
                RefreshData();
                AddToLog(lang.Get("Piracy", "WaitingForPirates"));
                Thread.Sleep(60000);
                continue;
            }

            do
            {
                TimeSpan start = new TimeSpan(startDate.Hour, startDate.Minute, startDate.Second);
                TimeSpan end = new TimeSpan(endDate.Hour, endDate.Minute, endDate.Second);
                TimeSpan now = DateTime.Now.TimeOfDay;

                bool canRaid = (start < end && now > start && now < end) || (start > end && (now < end || now > start));
                if (type.Equals("Raider") && !canRaid)
                {
                    AddToLog(lang.Get("Piracy", "TimeLimit"));
                    Thread.Sleep(TimeSpan.FromMinutes(2));
                    continue;
                }

                if (autoConvert)
                {
                    AddToLog(lang.Get("Piracy", "AutoConvertToCrew"));

                    int force = 0;
                    int.TryParse(GetCapturePoints(), out force);
                    parameters = string.Format("action=PiracyScreen&function=convert&view=pirateFortress&crewPoints={2}&cityId={1}&position=17&activeTab=tabCrew&backgroundView=city&currentCityId={1}&templateView=pirateFortress&actionRequest={0}&ajax=1", actionRequest, cityId, (force / 10).ToString());
                    try
                    {
                        response = client.DownloadString(url + "?" + parameters);

                        UpdateActionRequest(response, false);
                    }
                    catch (Exception exc) { AddToLog(exc.ToString()); }
                }

                foreach (string city in cities)
                {
                    if (2 == city.Split('|').Length)
                    {
                        AddToLog(lang.Get("Piracy", "Raiding") + " " + city.Split('|')[0]);
                        parameters = string.Format("action=PiracyScreen&function=raid&destinationIslandId={3}&destinationCityId={2}&backgroundView=city&currentCityId={1}&templateView=piracyRaid&actionRequest={0}&ajax=1", actionRequest, cityId, city.Split('|')[0], city.Split('|')[1]);
                        try
                        {
                            response = client.DownloadString(url + "?" + parameters);

                            UpdateActionRequest(response);
                        }
                        catch (Exception exc) { AddToLog(exc.ToString()); }

                        do
                        {
                            SetLastUpdate(DateTime.Now.ToString(Config.TIME_FORMAT));
                            RefreshData();
                            Thread.Sleep(THREE_MINUTES + random.Next(10000, 12000));
                        } while (hasOngoingMission);
                    }
                    else if (1 == city.Split('|').Length)
                    {
                        AddToLog(lang.Get("Piracy", "ChangesToCity") + " " + city.Split('|')[0]);
                        cityId = city;
                        RefreshData();
                    }
                }
            } while (repeat);
            AddToLog(lang.Get("Piracy", "RaidsCompleted"));
            //canRunThreadCaptcha = true;
            enableSelect = true;
            break;
        }
    }).Start();
}

public void City_Click(object sender, EventArgs e)
{
    //canRunThreadCaptcha = false;
    if (!isSearchingCities)
    {
        new Thread(delegate ()
        {
            isSearchingCities = true;
            AddToLog(lang.Get("Piracy", "FetchingAllCities"));

            GetAllCities();

            List<string[]> cities = new List<string[]>();
            foreach (CityDetails city in allCities)
            {
                cities.Add(new string[] { city.cityId, city.name, city.cords, (city.hasPirate ? lang.Get("Misc", "Yes") : lang.Get("Misc", "No")) });
            }

            using (var form = new SelectCity(cities))
            {
                var result = form.ShowDialog();
                if (DialogResult.OK == result)
                {
                    cityId = form.cityID;
                    SetCityID(cityId);

                    SetIslandID(islandID);
                }
                else if (DialogResult.Cancel == result)
                {
                    parameters = string.Format("action=CityScreen&function=build&cityId={0}&position=17&building=30&backgroundView=city&currentCityId={0}&templateView=buildingGround&actionRequest={1}&ajax=1", form.cityID, actionRequest);
                    try
                    {
                        response = client.DownloadString(url + "?" + parameters);

                        UpdateActionRequest(response, false);
                    }
                    catch (Exception exc) { AddToLog(exc.ToString()); }

                    parameters = string.Format("action=Premium&function=buildingSpeedup&cityId={0}&position=17&level=0&backgroundView=city&currentCityId={0}&templateView=pirateFortress&currentTab=tabBootyQuest&actionRequest={1}&ajax=1", form.cityID, actionRequest);
                    try
                    {
                        response = client.DownloadString(url + "?" + parameters);

                        UpdateActionRequest(response, false);
                    }
                    catch (Exception exc) { AddToLog(exc.ToString()); }

                    parameters = string.Format("action=Premium&function=buildingSpeedup&cityId={0}&position=17&level=0&backgroundView=city&currentCityId={0}&templateView=pirateFortress&currentTab=tabBootyQuest&actionRequest={1}&ajax=1", form.cityID, actionRequest);
                    try
                    {
                        response = client.DownloadString(url + "?" + parameters);

                        UpdateActionRequest(response, false);
                    }
                    catch (Exception exc) { AddToLog(exc.ToString()); }

                    cityId = form.cityID;
                    SetCityID(cityId);

                    SetIslandID(islandID);
                    GetAllCities();
                }
                else if (DialogResult.Retry == result)
                {
                    parameters = string.Format("action=CityScreen&function=upgradeBuilding&actionRequest={1}&cityId={0}&position=17&activeTab=tabBootyQuest&backgroundView=city&currentCityId={0}&templateView=pirateFortress&ajax=1", form.cityID, actionRequest);
                    try
                    {
                        response = client.DownloadString(url + "?" + parameters);

                        UpdateActionRequest(response, false);
                    }
                    catch (Exception exc) { AddToLog(exc.ToString()); }
                    cityId = form.cityID;
                    SetCityID(cityId);

                    SetIslandID(islandID);
                    GetAllCities();
                }
            }
            RefreshData();
            AddToLog(lang.Get("Piracy", "ChangesToCity") + " " + cityId);

            //canRunThreadCaptcha = true;
            isSearchingCities = false;
        }).Start();
    }
}

public void Train_Click(object sender, EventArgs e)
{
    int force = 0;
    int.TryParse(GetCapturePoints(), out force);
    if (CANT_FETCH != GetCapturePoints() && (force / 10) > 1)
    {
        using (var form = new NumberPicker((int.Parse(GetCapturePoints()) / 10)))
        {
            var result = form.ShowDialog();
            if (DialogResult.OK == result)
            {
                AddToLog(lang.Get("Piracy", "AutoConvertToCrew"));

                parameters = string.Format("action=PiracyScreen&function=convert&view=pirateFortress&crewPoints={2}&cityId={1}&position=17&activeTab=tabCrew&backgroundView=city&currentCityId={1}&templateView=pirateFortress&actionRequest={0}&ajax=1", actionRequest, cityId, form.number);
                try
                {
                    response = client.DownloadString(url + "?" + parameters);

                    UpdateActionRequest(response, false);
                }
                catch (Exception exc) { AddToLog(exc.ToString()); }
            }
        }
    }
}

public void Open_Click(object sender, EventArgs e)
{
    XmlNode root = MainWindow.XMLConfig.DocumentElement;
    root.SelectSingleNode("/Bot/Option").InnerText = "Login";
    root.SelectSingleNode("/Bot/Server").InnerText = server_address.Split('-')[1];
    root.SelectSingleNode("/Bot/UserAgent").InnerText = UA;
    root.SelectSingleNode("/Bot/ServerGame").InnerText = server_address;
    root.SelectSingleNode("/Bot/User/UserName").InnerText = userName;
    root.SelectSingleNode("/Bot/User/Password").InnerText = password;
    root.SelectSingleNode("/Bot/Proxy/EnableProxy").InnerText = (proxy.Equals("") || proxy.Equals("None") ? "false" : "true");

    MainWindow.XMLConfig.Save("config.xml");
    System.Diagnostics.Process.Start("IkariamPlus.exe");

    try
    {
        users.Items.RemoveAt(id);
        usersList.RemoveAt(id);
    }
    catch (Exception ex) { }
}

public void HighScore_Click(object sender, EventArgs e)
{
    PiracyInfo piracyInfo = new PiracyInfo(server_address, id);
    parameters = string.Format("view=pirateFortress&activeTab=tabBootyQuest&cityId={0}&position=17&backgroundView=city&currentCityId={0}&templateView=pirateFortress&actionRequest={1}&ajax=1", cityId, actionRequest);
    while (true)
    {
        try
        {
            while (client.IsBusy)
            {
                Thread.Sleep(400);
            }
            response = client.DownloadString(url + "?" + parameters);
            List<PiracyInfo.UserTop> topTxt = new List<PiracyInfo.UserTop>();
            Regex topRegex = new Regex("\\{\\\\\"place\\\\\":\\\\\"(\\d+?)\\\\\",\\\\\"oldPlace\\\\\":\\\\\"(\\d+?)\\\\\",\\\\\"avatarId\\\\\":\\\\\"(\\d+?)\\\\\",\\\\\"capturePoints\\\\\":\\\\\"(\\d+?)\\\\\",\\\\\"name\\\\\":\\\\\"(.+?)\\\\\"(,\\\\\"distance\\\\\":(.+?)|),\\\\\"cityId\\\\\":(\\d+?),\\\\\"islandId\\\\\":(\\\\\"(\\d+?)\\\\\"|(\\d+?))\\}");
            //place - [1]
            //oldPlace - [2]
            //avatarId - [3]
            //capturePoints - [4]
            //name - [5]
            //distance - [7]
            //cityId - [8]
            //islandId - [9]

            MatchCollection topPlaces = topRegex.Matches(response);
            foreach (Match place in topPlaces)
            {
                GroupCollection result = place.Groups;
                string name = Regex.Replace(
                            result[5].Value.Replace("\\\\", "\\"),
                            @"\\u(?<Value>[a-zA-Z0-9]{4})",
                            m =>
                            {
                                return ((char)int.Parse(m.Groups["Value"].Value, System.Globalization.NumberStyles.HexNumber)).ToString();
                            }
                        );

                topTxt.Add(new PiracyInfo.UserTop(result[1].Value, result[2].Value, result[3].Value, result[4].Value, name, result[7].Value, result[8].Value, result[9].Value, server_address));
            }
            piracyInfo.UpdateTop(topTxt);

            Match endOfRotation = Regex.Match(response, "\\\"enddate\\\":(.+?),");
            if (endOfRotation.Success)
            {
                DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddSeconds(long.Parse(endOfRotation.Groups[1].Value)).ToLocalTime();

                piracyInfo.UpdateTimeEnd(dtDateTime, server_address);
            }
            if (!piracyInfo.InvokeRequired)
            {
                piracyInfo.Show();
                piracyInfo.BringToFront();
            }
            else
                piracyInfo.BeginInvoke(new Action(() => { piracyInfo.Show(); piracyInfo.BringToFront(); }));

            UpdateActionRequest(response);
            break;
        }
        catch (Exception ex) { Thread.Sleep(500); }
    }
}

private class CityDetails
{
    public string name, cityId, cords, islandId;
    public bool hasPirate;

    public CityDetails(string name, string cityId, string cords, string islandId, bool hasPirate)
    {
        this.name = name;
        this.cityId = cityId;
        this.cords = cords;
        this.islandId = islandId;
        this.hasPirate = hasPirate;
    }
}
        }
    }*/
}
