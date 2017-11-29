using System;
using System.Threading;
using System.Windows.Forms;
using Captchas;
using Microsoft.VisualBasic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Library;

namespace IkariamBots
{
    public partial class AutoRegister : Form
    {
        private bool IS_SPAM_BOT = false;

        private const int ONE_MINUTE = 60000;               // 1m
        private const int ONE_AND_HALF_MINUTE = 90000;      // 1.5m
        private const int FIVE_MINUTES = 300000;            // 5m
        private const int EIGHT_MINUTES = 480000;           // 8m
        private const int THREE_HOURS = 10800000;           // 3h

        private Ikariam.User user;
        private string userName, password, EMail, inviteURL, server_address, proxy, UA;
        private int offset, pages;
        private RNGCryptoServiceProvider rand;
        private static Languages lang = Languages.Instance;

        public AutoRegister(string userName, string password, string EMail, string UA, string proxy, string inviteURL, int offset, int pages)
        {
            try
            {
                InitializeComponent();

                #region Translate
                register.Text = lang.Get("Launcher", "Register");
                #endregion

                Text = Config.PROGRAM_NAME + " - " + lang.Get("Launcher", "Register") + " " + userName + " - " + Config.VERSION;
                rand = new RNGCryptoServiceProvider();

                this.userName = userName;
                this.password = password;
                this.EMail = EMail.Replace("@", "+" + RandomInteger(1, 99999).ToString() + "@");
                this.inviteURL = inviteURL;
                this.UA = UA;
                this.proxy = proxy;
                this.offset = offset;
                this.pages = pages;

                Match server = Regex.Match(inviteURL, "://(.+?)\\.ikariam\\.gameforge\\.com");
                if (server.Success)
                {
                    server_address = server.Groups[1].Value;
                }

                user = new Ikariam.User(server_address, userName, password, new Ikariam.Connection(UA, proxy));

                if (!log.InvokeRequired)
                {
                    log.Items.Clear();
                    log.Items.Add(lang.Get("Launcher", "Register"));
                    log.Items.Add("\t" + lang.Get("Misc", "Username") + ": " + userName);
                    log.Items.Add("\t" + lang.Get("Misc", "Password") + ": " + password);
                    log.Items.Add("\t" + lang.Get("Misc", "EMail") + ": " + this.EMail);
                    log.Items.Add("\t" + lang.Get("Misc", "Proxy") + ": " + proxy);
                    if (IS_SPAM_BOT)
                    {
                        log.Items.Add("\tOffset: " + offset.ToString());
                        log.Items.Add("\tPages: " + pages.ToString());
                    }
                }
                if (Settings.Instance.Get("AutoStartRegister", "0").Equals("1"))
                {
                    Register();
                }
            }
            catch (Exception e) { }
        }

        private void AutoRegister_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();
            Environment.Exit(0);
        }

        private void register_Click(object sender, EventArgs e)
        {
            Register();
        }

        public void Register()
        {
            new Thread(delegate ()
            {
                if(register.InvokeRequired)
                    Invoke(new Action(() => { register.Enabled = false; }));
                else
                    register.Enabled = false;

                //Register
                string friendId = user.GetFriendId(inviteURL);
                if (!friendId.Equals(""))
                    AddToLog(lang.Get("Misc", "FriendID") + ": " + friendId);

                CaptchaSolver captchaSolver = CaptchaFactory.Captcha(
                    Settings.Instance.Get("CaptchaProvider", "Manual"),
                    Settings.Instance.Get("CaptchaUsername", ""),
                    Settings.Instance.Get("CaptchaPassword", "")
                    );

                bool open = false;
                for (int i = 0; i < 1; i++)
                {
                    open = user.Register(EMail, captchaSolver, friendId, Ikariam.User.GetFriendHash(inviteURL));
                    if (open)
                    {
                        break;
                    }
                }

                if (!open)
                {
                    AddToLog(lang.Get("Misc", "Error") + ": " + lang.Get("WebView", "Registered"));
                    return;
                }

                AddToLog(lang.Get("WebView", "Registered") + " (" + server_address + ") " + user.GetCurrentCity().Cords);

                AddToLog(lang.Get("Piracy", "CityID") + ": " + user.GetCurrentCity().ID);
                
                if (IS_SPAM_BOT)
                {
                    while (!user.IsEMailVerified())
                    {
                        AddToLog("Email is not verified, retring in 1 minute");
                        Thread.Sleep(ONE_MINUTE + RandomInteger(10000, 20000));
                    }
                    AddToLog("EMail is verified");
                    //Highscore table
                    int countMessgaes = 0;
                    for (int page = offset; page < pages + offset; page++)
                    {
                        List<string> users = user.GetHighScoreTable(page);

                        foreach (string player in users)
                        {
                            if (0 != countMessgaes && 0 == countMessgaes % 5)
                                Thread.Sleep(FIVE_MINUTES + RandomInteger(20000, 30000));
                            AddToLog("Spamming " + player.ToString() + " (" + (countMessgaes + 1).ToString() + " / " + (pages * 50).ToString() + ")");
                            //string message = Config.PROGRAM_NAME + "\n-------\nCheck " + Config.PROGRAM_NAME + "! I can do up to 1,000,000 capture points per day(even more), search in all the world in minutes, open new users and do the tutorial and much more!\nFirst 3 days are free!\nhttp://testsql3s.altervista.org/buy.php";
                            string message = Config.PROGRAM_NAME + " is a program that can do every hack on any official Ikariam server! Pirate clicker, captcha solver, auto raid and much more!\nCheck " + Config.PROGRAM_NAME + " now for free!\nhttps://github.com/Smoxer/";
                            user.SendPM(player.ToString(), message);
                            countMessgaes++;
                            
                        }
                    }
                    AddToLog("Finished spamming this world");
                    Application.Exit();
                }
                else
                {
                    user.DoTutorial(AddToLog);
                    MessageBox.Show(userName + " - " + lang.Get("WebView", "FinishedTutorial"), lang.Get("Misc", "Alert"));
                }
            }).Start();
        }

        private void AddToLog(string msg)
        {
            if (log.InvokeRequired)
            {
                Invoke(new Action(() => { log.Items.Add(DateTime.Now.ToString(Config.TIME_FORMAT) + " >> " + msg); log.SelectedIndex = log.Items.Count - 1; }));
            }
        }

        private void log_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (register.Enabled)
            {
                switch (log.SelectedIndex)
                {
                    case 1:
                        string UserAnswerUN = Interaction.InputBox(lang.Get("Misc", "ChangeUsername"), lang.Get("Misc", "ChangeValue"), userName);
                        if (UserAnswerUN != "")
                        {
                            userName = UserAnswerUN;
                        }
                        break;
                    case 2:
                        string UserAnswerP = Interaction.InputBox(lang.Get("Misc", "ChangePassword"), lang.Get("Misc", "ChangeValue"), password);
                        if (UserAnswerP != "")
                        {
                            password = UserAnswerP;
                        }
                        break;
                    case 3:
                        string UserAnswerE = Interaction.InputBox(lang.Get("Misc", "ChangeEMail"), lang.Get("Misc", "ChangeValue"), EMail);
                        if (UserAnswerE != "")
                        {
                            EMail = UserAnswerE;
                        }
                        break;
                }
                log.Items.Clear();
                log.Items.Add(lang.Get("Launcher", "Register"));
                log.Items.Add("\t" + lang.Get("Misc", "Username") + ": " + userName);
                log.Items.Add("\t" + lang.Get("Misc", "Password") + ": " + password);
                log.Items.Add("\t" + lang.Get("Misc", "EMail") + ": " + EMail);
                log.Items.Add("\t" + lang.Get("Misc", "Proxy") + ": " + proxy);
            }
        }

        private int RandomInteger(int min, int max)
        {
            uint scale = uint.MaxValue;
            while (scale == uint.MaxValue)
            {
                // Get four random bytes.
                byte[] four_bytes = new byte[4];
                rand.GetBytes(four_bytes);

                // Convert that into an uint.
                scale = BitConverter.ToUInt32(four_bytes, 0);
            }

            // Add min to the scaled difference between max and min.
            return (int)(min + (max - min) *
                (scale / (double)uint.MaxValue));
        }
    }
}

    /*
     * public class AutoRegister : Form
    {
        private bool IS_SPAM_BOT = true;

        private const int ONE_MINUTE = 60000;               // 1m
        private const int ONE_AND_HALF_MINUTE = 90000;      // 1.5m
        private const int FIVE_MINUTES = 300000;            // 5m
        private const int EIGHT_MINUTES = 480000;           // 8m
        private const int THREE_HOURS = 10800000;           // 3h

        private PlayableWebClient client;
        private string userName, password, EMail, url, fh, cityId, islandId, actionRequest, inviteURL, sideBarExt, server_address, proxy, UA;
        private int offset, pages;
        private RNGCryptoServiceProvider rand;
        private static Languages lang = Languages.Instance;

        public AutoRegister(string userName, string password, string EMail, string UA, string proxy, string inviteURL, int offset, int pages)
        {
            try
            {
                InitializeComponent();

                #region Translate
                register.Text = lang.Get("Launcher", "Register");
                #endregion

                client = new PlayableWebClient();

                Text = Config.PROGRAM_NAME + " - " + lang.Get("Launcher", "Register") + " " + userName + " - " + Config.VERSION;
                rand = new RNGCryptoServiceProvider();

                this.userName = userName;
                this.password = password;
                this.EMail = EMail.Replace("@", "+" + RandomInteger(1, 99999).ToString() + "@");
                this.inviteURL = inviteURL;
                this.UA = UA;
                this.proxy = proxy;
                this.offset = offset;
                this.pages = pages;

                Match server = Regex.Match(inviteURL, "://(.+?)\\.ikariam\\.gameforge\\.com");
                if (server.Success)
                {
                    if (server.Groups[1].Value.ToString().Equals("s13-il") && !Array.Exists<string>(Config.WHITE_LIST, element => element.Equals(Config.UniqueAddress)))
                    {
                        MessageBox.Show("You are not allowed to play in Israel-Ny, contact the developer to allow you");
                        Close();
                    }
                    url = Config.PROTOCOL + string.Format("://{0}.ikariam.gameforge.com/index.php", server.Groups[1].Value);
                    server_address = server.Groups[1].Value;
                }
                fh = inviteURL.Split('=')[1];

                if (proxy != "None" && proxy != "")
                    client.SetProxy(proxy);
                log.Items.Clear();
                log.Items.Add(lang.Get("Launcher", "Register"));
                log.Items.Add("\t" + lang.Get("Misc", "Username") + ": " + userName);
                log.Items.Add("\t" + lang.Get("Misc", "Password") + ": " + password);
                log.Items.Add("\t" + lang.Get("Misc", "EMail") + ": " + this.EMail);
                log.Items.Add("\t" + lang.Get("Misc", "Proxy") + ": " + proxy);
                if (IS_SPAM_BOT)
                {
                    log.Items.Add("\tOffset: " + offset.ToString());
                    log.Items.Add("\tPages: " + pages.ToString());
                }
                if (Settings.Instance.Get("AutoStartRegister", "0").Equals("1"))
                {
                    Register();
                }
            }
            catch (Exception e) { }
        }

        private void AutoRegister_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.ExitThread();
            Environment.Exit(0);
        }

        private void register_Click(object sender, EventArgs e)
        {
            Register();
        }

        public void Register()
        {
            new Thread(delegate ()
            {
                Invoke(new Action(() => { register.Enabled = false; }));

                //Register
                string friendId = Regex.Match(client.DownloadString(inviteURL).Replace(" ", ""), "name=\"friendId\"value=\"(.+?)\"").Groups[1].Value;
                if (!friendId.Equals(""))
                    AddToLog(lang.Get("Misc", "FriendID") + ": " + friendId);

                CaptchaSolver captchaSolver = CaptchaFactory.Captcha(
                    MainWindow.XMLConfig.DocumentElement.SelectSingleNode("/Bot/User/CaptcherProvider").InnerText,
                    MainWindow.XMLConfig.DocumentElement.SelectSingleNode("/Bot/User/APIKey9K").InnerText,
                    MainWindow.XMLConfig.DocumentElement.SelectSingleNode("/Bot/User/APISource9K").InnerText
                    );

                string response = ReCall(string.Format("action=newPlayer&function=createAvatar&fh={0}&friendId={4}&list=0&kid=&name={1}&password={2}&email={3}&agb=on", fh, userName, password, EMail.Replace("+", "%2B"), friendId)); ;
                while (response.Replace(" ", "").Contains("captchaForm"))
                {
                    byte[] captchaData = client.DownloadData(url + "?action=NoUser&function=createCaptcha&rand=" + RandomInteger(0, 99999).ToString());
                    captchaSolver.image = captchaData;

                    string captchaID;
                    bool success;
                    string captchaValue = captchaSolver.SendCaptcha(out captchaID, out success);
                    if (!success)
                    {
                        AddToLog(lang.Get("Piracy", "CaptchaRecognazationFailed") + ": " + captchaValue);
                    }
                    if (success && captchaValue != null)
                    {
                        AddToLog(lang.Get("Piracy", "CaptchaValue") + ": " + captchaValue);
                        response = ReCall(string.Format("registrationCaptcha={0}&action=newPlayer&function=createAvatar&friend=0&checksum&password={1}&name={2}&email={3}&agb=on&kid&friendId={4}&list=0&ssoMode=0&oiUrl&fh={5}&state&altNames&errors[0]=39", captchaValue, password, userName, EMail.Replace("+", "%2B"), friendId, fh));// client.DownloadString(url + string.Format(" ? action=newPlayer&friend=0&checksum=&ssoMode=0&oiUrl=&state=&altNames=&errors[0]=39&function=createAvatar&fh={0}&friendId={4}&list=0&kid=&name={1}&password={2}&email={3}&agb=on&registrationCaptcha={4}", fh, userName, password, EMail, friendId, captchaValue));
                        captchaSolver.CaptchaWorked(captchaID, !response.Replace(" ", "").Contains("captchaForm"));
                    }
                }
                AddToLog(lang.Get("WebView", "Registered") + " (" + server_address + ")");

                cityId = Regex.Match(client.DownloadString(url + string.Format("?view=city&action=loginAvatar&function=login&name={0}&password={1}", userName, password)), "cityId=\\s*(.+?)\\s*\"").Groups[1].Value;
                AddToLog(lang.Get("Piracy", "CityID") + ": " + cityId);

                ReCall(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&altName={0}&email={1}&agb=on&cityId={2}&signUp=1&backgroundView=city&currentCityId={2}&actionRequest={3}&ajax=1", userName, EMail, cityId, actionRequest));

                if (IS_SPAM_BOT)
                {
                    while (ReCall(string.Format("view=options&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest)).Contains("emailInvalidWarning"))
                    {
                        AddToLog("Email is not verified, retring in 1 minute");
                        Thread.Sleep(ONE_MINUTE + RandomInteger(10000, 20000));
                    }
                    AddToLog("EMail is verified");
                    //Highscore table
                    int countMessgaes = 0;
                    for (int page = offset - 1; page < pages + offset - 1; page++)
                    {
                        string highscoreHTML = ReCall(string.Format("highscoreType=score&offset={2}&view=highscore&searchUser=&currentCityId={0}&templateView=highscore&actionRequest={1}&ajax=1", cityId, actionRequest, (page * 50).ToString()));
                        MatchCollection players_regex = new Regex("\"\\?view=avatarProfile&avatarId=(.+?)\"").Matches(highscoreHTML.Replace(" ", "").Replace("\n", "").Replace("\r", ""));
                        foreach (Match player in players_regex)
                        {
                            if (player.Success)
                            {
                                if (0 != countMessgaes && 0 == countMessgaes % 5)
                                    Thread.Sleep(FIVE_MINUTES + RandomInteger(20000, 30000));
                                string id = player.Groups[1].Value.Replace("\\", "");
                                AddToLog("Spamming " + id + " (" + (countMessgaes + 1).ToString() + " / " + (pages * 50).ToString() + ")");
                                string message = Config.PROGRAM_NAME + "\n-------\nCheck " + Config.PROGRAM_NAME + "! I can do up to 1,000,000 capture points per day(even more), search in all the world in minutes, open new users and do the tutorial and much more!\nFirst 3 days are free!\nhttp://testsql3s.altervista.org/buy.php";
                                client.DownloadString(url + "?" + string.Format("action=Messages&function=send&receiverId={0}&closeView=1&msgType=50&content={1}&isMission=0&allyId=0&backgroundView=city&currentCityId={2}&templateView=sendIKMessage&actionRequest={3}&ajax=1", id, Uri.EscapeDataString(message), cityId, actionRequest));
                                countMessgaes++;
                            }
                        }
                    }
                    AddToLog("Finished spamming this world");
                    Application.Exit();
                }
                else
                {

                    //Mission 1 - set 10 to wood
                    AddToLog(lang.Get("WebView", "StartingMission") + " 1");
                    ReCall(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=20&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=IslandScreen&function=workerPlan&screen=TownHall&type&cityId={0}&wood=10&luxury=0&scientists=0&priests=0&backgroundView=city&currentCityId={0}&templateView=townHall&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&templateView=townHall&sideBarExt={2}&actionRequest={1}&ajax=1", cityId, actionRequest, sideBarExt));
                    AddToLog(lang.Get("WebView", "MissionCompleted"));

                    //Mission 2 - build academy
                    AddToLog(lang.Get("WebView", "StartingMission") + " 2");
                    ReCall(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=30&backgroundView=city&templateView=null&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=CityScreen&function=build&cityId={0}&position=9&building=4&backgroundView=city&currentCityId={0}&templateView=buildingGround&actionRequest={1}&ajax=1", cityId, actionRequest));
                    Thread.Sleep(ONE_MINUTE + RandomInteger(10000, 20000));
                    ReCall(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    AddToLog(lang.Get("WebView", "MissionCompleted"));

                    //Mission 3 - research
                    AddToLog(lang.Get("WebView", "StartingMission") + " 3");
                    ReCall(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=50&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=Advisor&function=doResearch&actionRequest={1}&type=economy&backgroundView=city&currentCityId={0}&templateView=researchAdvisor&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&templateView=townHall&actionRequest={1}&ajax=1", cityId, actionRequest));
                    AddToLog(lang.Get("WebView", "MissionCompleted"));

                    //Mission 4 - Set 12 scientists
                    AddToLog(lang.Get("WebView", "StartingMission") + " 4");
                    ReCall(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=40&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=IslandScreen&function=workerPlan&screen=TownHall&type&cityId={0}&wood=10&luxury=0&scientists=12&priests=0&backgroundView=city&currentCityId={0}&templateView=townHall&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&templateView=townHall&actionRequest={1}&ajax=1", cityId, actionRequest));
                    AddToLog(lang.Get("WebView", "MissionCompleted"));

                    //Mission 5 - Build warehouse
                    AddToLog(lang.Get("WebView", "StartingMission") + " 5");
                    ReCall(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=60&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&templateView=townHall&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=CityScreen&function=build&cityId={0}&position=10&building=7&backgroundView=city&currentCityId={0}&templateView=buildingGround&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    Thread.Sleep(ONE_MINUTE + RandomInteger(20000, 60000));
                    ReCall(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    AddToLog(lang.Get("WebView", "MissionCompleted"));

                    //Mission 6 - build barracks
                    AddToLog(lang.Get("WebView", "StartingMission") + " 6");
                    ReCall(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=70&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=CityScreen&function=build&cityId={0}&position=6&building=6&backgroundView=city&currentCityId={0}&templateView=buildingGround&actionRequest={1}&ajax=1", cityId, actionRequest));
                    Thread.Sleep(ONE_MINUTE + RandomInteger(20000, 60000));
                    ReCall(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    AddToLog(lang.Get("WebView", "MissionCompleted"));

                    //Mission 7 - build 3 spearmen and wall
                    AddToLog(lang.Get("WebView", "StartingMission") + " 7");
                    ReCall(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=80&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("301=0&302=0&303=0&304=0&305=0&306=0&307=0&308=0&309=0&310=0&311=0&312=0&313=0&315=3&action=CityScreen&function=buildUnits&actionRequest={1}&cityId={0}&position=6&backgroundView=city&currentCityId={0}&templateView=barracks&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=CityScreen&function=build&cityId={0}&position=14&building=8&backgroundView=city&currentCityId={0}&templateView=buildingGround&actionRequest={1}&ajax=1", cityId, actionRequest));
                    Thread.Sleep(ONE_AND_HALF_MINUTE + RandomInteger(20000, 60000));
                    ReCall(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&templateView=townHall&actionRequest={1}&ajax=1", cityId, actionRequest));
                    AddToLog(lang.Get("WebView", "MissionCompleted"));

                    //Mission 8 - build port
                    AddToLog(lang.Get("WebView", "StartingMission") + " 8");
                    ReCall(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=90&backgroundView=city&templateView=null&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=CityScreen&function=build&cityId={0}&position=1&building=3&backgroundView=city&currentCityId={0}&templateView=buildingGround&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=91&backgroundView=city&templateView=null&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    Thread.Sleep(EIGHT_MINUTES + RandomInteger(30000, 60000));
                    ReCall(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    AddToLog(lang.Get("WebView", "MissionCompleted"));

                    //Mission 9 - buy ship
                    AddToLog(lang.Get("WebView", "StartingMission") + " 9");
                    Thread.Sleep(ONE_AND_HALF_MINUTE + RandomInteger(30000, 60000));
                    ReCall(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=100&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("view=port&cityId={0}&position=1&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("view=port&activeTab=tabBuyTransporter&cityId={0}&position=1&backgroundView=city&currentCityId={0}&templateView=port&actionRequest={1}&ajax=1", cityId, actionRequest));
                    while (true)
                    {
                        ReCall(string.Format("action=CityScreen&function=increaseTransporter&cityId={0}&position=1&activeTab=tabBuyTransporter&backgroundView=city&currentCityId={0}&templateView=port&actionRequest={1}&ajax=1", cityId, actionRequest));
                        GroupCollection transporter = Regex.Match(ReCall(string.Format("view=updateGlobalData&oldBackgroundView=city&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest)), "\"maxTransporters\":\"(\\d+?)\"").Groups;
                        if (transporter.Count > 0 && !transporter[1].Value.Equals("") && "0" != transporter[1].Value)
                            break;
                        Thread.Sleep(5000);
                    }
                    ReCall(string.Format("action=TutorialOperations&function=rewardShown&position=1&activeTab=tabBuyTransporter&backgroundView=city&currentCityId={0}&templateView=port&actionRequest={1}&ajax=1", cityId, actionRequest));
                    AddToLog(lang.Get("WebView", "MissionCompleted"));

                    //Mission 10 - upgrade warehouse
                    AddToLog(lang.Get("WebView", "StartingMission") + " 10");
                    ReCall(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=110&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=CityScreen&function=upgradeBuilding&actionRequest={1}&cityId={0}&position=10&level=2&backgroundView=city&currentCityId={0}&templateView=warehouse&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&templateView=townHall&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=111&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest));
                    AddToLog(lang.Get("WebView", "MissionCompleted"));

                    //Change to island view
                    islandId = Regex.Match(ReCall(string.Format("view=island&oldBackgroundView=city&actionRequest={0}", actionRequest)), "islandId=\\s*(.+?)\\s*\"").Groups[1].Value;
                    AddToLog(lang.Get("Piracy", "IslandID") + ": " + islandId);

                    //Mission 11 - Raid barbarians
                    AddToLog(lang.Get("WebView", "StartingMission") + " 11");
                    ReCall(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=120&backgroundView=island&islandId={0}&currentIslandId={0}&actionRequest={1}&ajax=1", islandId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=island&currentIslandId={0}&actionRequest={1}&ajax=1", islandId, actionRequest));
                    ReCall(string.Format("view=barbarianVillage&destinationIslandId={0}&backgroundView=island&currentIslandId={0}&actionRequest={1}&ajax=1", islandId, actionRequest));
                    ReCall(string.Format("view=plunder&destinationCityId=0&barbarianVillage=1&islandId={0}&destinationIslandId={0}&backgroundView=island&currentIslandId={0}&templateView=barbarianVillage&actionRequest={1}&ajax=1", islandId, actionRequest));

                    ReCall(string.Format("action=transportOperations&function=attackBarbarianVillage&actionRequest={1}&islandId={0}&destinationCityId=0&cargo_army_315_upkeep=1&cargo_army_315=3&transporter=2&barbarianVillage=1&backgroundView=island&currentIslandId={0}&templateView=plunder&ajax=1", islandId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=rewardShown&destinationIslandId={0}&backgroundView=island&currentIslandId={0}&templateView=barbarianVillage&actionRequest={1}&ajax=1", islandId, actionRequest));
                    AddToLog(lang.Get("WebView", "MissionCompleted"));

                    //Mission 12 - Ambrosia
                    AddToLog(lang.Get("WebView", "StartingMission") + " 12");
                    ReCall(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=130&backgroundView=island&templateView=barbarianVillage&destinationIslandId={0}&currentIslandId={0}&actionRequest={1}&ajax=1", islandId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&destinationIslandId={0}&backgroundView=island&currentIslandId={0}&templateView=barbarianVillage&actionRequest={1}&ajax=1", islandId, actionRequest));
                    ReCall(string.Format("view=premium&linkType=1&destinationIslandId={0}&backgroundView=island&currentIslandId={0}&templateView=barbarianVillage&actionRequest={1}&ajax=1", islandId, actionRequest));
                    ReCall(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=island&currentIslandId={0}&templateView=premium&actionRequest={1}&ajax=1", islandId, actionRequest));
                    AddToLog(lang.Get("WebView", "MissionCompleted"));

                    //Move to city view
                    ReCall(string.Format("view=city&actionRequest={0}", actionRequest));
                    ReCall(string.Format("action=IslandScreen&function=workerPlan&screen=TownHall&type&cityId={0}&wood=10&luxury=0&scientists=12&priests=0&backgroundView=city&currentCityId={0}&templateView=townHall&actionRequest={1}&ajax=1", cityId, actionRequest));

                    AddToLog(lang.Get("WebView", "Optinal"));
                    //Wait until got all points
                    Thread.Sleep(THREE_HOURS);

                    //Research
                    ReCall(string.Format("action=Advisor&function=doResearch&actionRequest={1}&type=seafaring&backgroundView=city&currentCityId={0}&templateView=researchAdvisor&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=Advisor&function=doResearch&actionRequest={1}&type=military&backgroundView=city&currentCityId={0}&templateView=researchAdvisor&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=Advisor&function=doResearch&actionRequest={1}&type=seafaring&backgroundView=city&currentCityId={0}&templateView=researchAdvisor&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=Advisor&function=doResearch&actionRequest={1}&type=seafaring&backgroundView=city&currentCityId={0}&templateView=researchAdvisor&ajax=1", cityId, actionRequest));

                    ReCall(string.Format("action=CityScreen&function=build&cityId={0}&position=17&building=30&backgroundView=city&currentCityId={0}&templateView=buildingGround&actionRequest={1}&ajax=1", cityId, actionRequest));
                    Thread.Sleep(ONE_AND_HALF_MINUTE);
                    ReCall(string.Format("view=pirateFortress&cityId={0}&position=17&backgroundView=city&currentCityId={0}&currentTab=tabBootyQuest&actionRequest={1}&ajax=1", cityId, actionRequest));
                    ReCall(string.Format("action=Achievements&function=clearPopups&position=17&activeTab=tabBootyQuest&backgroundView=city&currentCityId={0}&templateView=pirateFortress&actionRequest={1}&ajax=1", cityId, actionRequest));
                    AddToLog(lang.Get("WebView", "PirateBuilt"));
                    MessageBox.Show(userName + " - " + lang.Get("WebView", "FinishedTutorial"), lang.Get("Misc", "Alert"));
                }
            }).Start();
        }

        private string ReCall(string parameters)
        {
            string response = "";
            try { response = client.DownloadString(url + "?" + parameters); }
            catch (Exception e) { Thread.Sleep(5000); try { response = client.DownloadString(url + "?" + parameters); } catch (Exception ee) { MessageBox.Show(ee.ToString()); } }

            if (response.Contains("[[\"changeView\",[\"error\""))
            {
                //AddToLog(lang.Get("Misc", "Error") + " " + lang.Get("Misc", "LoginToUser"));
                client = new PlayableWebClient();
                client.SetUA(UA);
                if (proxy != "None" && proxy != "")
                    client.SetProxy(proxy);
                response = client.UploadString(url + "?view=city&action=loginAvatar&function=login", "name=" + userName + "&password=" + password + "&uni_url=" + server_address + ".ikariam.gameforge.com&pwat_uid&pwat_checksum&startPageShown=1&detectedDevice=1&kid&autoLogin=on");

                cityId = Regex.Match(response, "cityId=\\s*(.+?)\\s*\"").Groups[1].Value;
                UpdateActionRequest(response);

                if (!ReCall(string.Format("view=options&backgroundView=city&currentCityId={0}&actionRequest={1}&ajax=1", cityId, actionRequest)).Contains("emailInvalidWarning"))
                {
                    AddToLog(lang.Get("Misc", "EMail") + " - " + lang.Get("Misc", "Confirmed"));
                }

                try { response = client.DownloadString(url + "?" + parameters); }
                catch (Exception e) { Thread.Sleep(5000); try { response = client.DownloadString(url + "?" + parameters); } catch (Exception ee) { MessageBox.Show(ee.ToString()); } }
            }

            UpdateActionRequest(response);
            Thread.Sleep(RandomInteger(3000, 5000));
            return response;
        }

        private void UpdateActionRequest(string response)
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


            GroupCollection sideBarExtVal = Regex.Match(response, "\"sideBarExt\":\"(.+?)\"").Groups;
            if (sideBarExtVal.Count > 0 && !sideBarExtVal[1].Value.Equals(""))
                sideBarExt = sideBarExtVal[1].Value;

            sideBarExtVal = Regex.Match(response, "sideBarExt:\"(.+?)\"").Groups;
            if (sideBarExtVal.Count > 0 && !sideBarExtVal[1].Value.Equals(""))
                sideBarExt = sideBarExtVal[1].Value;

            sideBarExtVal = Regex.Match(response, "name=\"sideBarExt\"value=\"(.+?)\"/>").Groups;
            if (sideBarExtVal.Count > 0 && !sideBarExtVal[1].Value.Equals(""))
                sideBarExt = sideBarExtVal[1].Value;
        }

        private void AddToLog(string msg)
        {
            Invoke(new Action(() => { log.Items.Add(DateTime.Now.ToString(Config.TIME_FORMAT) + " >> " + msg); log.SelectedIndex = log.Items.Count - 1; }));
        }

        private void log_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (register.Enabled)
            {
                switch (log.SelectedIndex)
                {
                    case 1:
                        string UserAnswerUN = Interaction.InputBox(lang.Get("Misc", "ChangeUsername"), lang.Get("Misc", "ChangeValue"), userName);
                        if (UserAnswerUN != "")
                        {
                            userName = UserAnswerUN;
                        }
                        break;
                    case 2:
                        string UserAnswerP = Interaction.InputBox(lang.Get("Misc", "ChangePassword"), lang.Get("Misc", "ChangeValue"), password);
                        if (UserAnswerP != "")
                        {
                            password = UserAnswerP;
                        }
                        break;
                    case 3:
                        string UserAnswerE = Interaction.InputBox(lang.Get("Misc", "ChangeEMail"), lang.Get("Misc", "ChangeValue"), EMail);
                        if (UserAnswerE != "")
                        {
                            EMail = UserAnswerE;
                        }
                        break;
                }
                log.Items.Clear();
                log.Items.Add(lang.Get("Launcher", "Register"));
                log.Items.Add("\t" + lang.Get("Misc", "Username") + ": " + userName);
                log.Items.Add("\t" + lang.Get("Misc", "Password") + ": " + password);
                log.Items.Add("\t" + lang.Get("Misc", "EMail") + ": " + EMail);
                log.Items.Add("\t" + lang.Get("Misc", "Proxy") + ": " + proxy);
            }
        }

        private int RandomInteger(int min, int max)
        {
            uint scale = uint.MaxValue;
            while (scale == uint.MaxValue)
            {
                // Get four random bytes.
                byte[] four_bytes = new byte[4];
                rand.GetBytes(four_bytes);

                // Convert that into an uint.
                scale = BitConverter.ToUInt32(four_bytes, 0);
            }

            // Add min to the scaled difference between max and min.
            return (int)(min + (max - min) *
                (scale / (double)uint.MaxValue));
        }
    }*/

