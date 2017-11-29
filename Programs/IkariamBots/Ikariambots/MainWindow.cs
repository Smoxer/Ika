using System;
using CefSharp;
using System.IO;
using System.Web;
using System.Linq;
using System.Threading;
using IkariamBots.Forms;
using CefSharp.WinForms;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using Library;

namespace IkariamBots
{
    public partial class MainWindow : Form
    {
        public static ChromiumWebBrowser browser;
        public static ScriptLanguage scriptLanguage = new ScriptLanguage();

        private JSCallBack js;
        private static bool canClose;
        public static bool isScriptLanguageCanRun;
        public static string server_address = "";
        private string password = "", ua = "", proxy = "";
        public static string cityID = "", UserName = "", FileFight = "";
        public static int lastOption = 1, Hoplite = 0, SteamGiant = 0, Spearman = 0, Slinger = 0, Swordsman = 0, Archer = 0, SulphurCarabineer = 0, Ram = 0, Catapult = 0, Mortar = 0, Gyrocopter = 0, BalloonBombardier = 0, Cook = 0, Doctor = 0, TransporterShips = 0;
        private string[] arguments = Environment.GetCommandLineArgs();
        public static Dictionary<string, List<string>> buildQueue = new Dictionary<string, List<string>>();
        private static Languages lang = Languages.Instance;

        public MainWindow(string ver)
        {
            InitializeComponent();
            WindowState = FormWindowState.Maximized;

            #region Translate
            scriptToolStripMenuItem.Text = lang.Get("WebView", "Scripts");
            calculatorToolStripMenuItem.Text = lang.Get("WebView", "Calculators");
            optionsToolStripMenuItem1.Text = lang.Get("WebView", "Options");
            helpToolStripMenuItem.Text = lang.Get("WebView", "Help");
            navigateToolStripMenuItem.Text = lang.Get("WebView", "Navigate");
            workingScripts.Text = lang.Get("WebView", "WorkingScripts") + " - ";
            militaryCalculatorToolStripMenuItem.Text = lang.Get("WebView", "Military");
            islandsByTimeToolStripMenuItem.Text = lang.Get("WebView", "IslandsByTime");
            editConfigurationsToolStripMenuItem.Text = lang.Get("WebView", "EditConfigures");
            proxyOptionsToolStripMenuItem.Text = lang.Get("WebView", "ProxyOptions");
            aboutToolStripMenuItem.Text = lang.Get("WebView", "About");
            whatIsMyIPToolStripMenuItem.Text = lang.Get("WebView", "WhatIP");
            buyMeABeerToolStripMenuItem.Text = lang.Get("WebView", "Donate");
            startPageToolStripMenuItem.Text = lang.Get("WebView", "StartPage");
            officialIkariamsForumToolStripMenuItem.Text = lang.Get("WebView", "OfficialForum");
            officialIkariamsWikiToolStripMenuItem.Text = lang.Get("WebView", "OfficialWiki");
            piracyToolStripMenuItem.Text = lang.Get("Misc", "Pirate");
            restartToolStripMenuItem.Text = lang.Get("Misc", "Restart");
            quitToolStripMenuItem.Text = lang.Get("Misc", "Quit");
            reloadToolStripMenuItem.Text = lang.Get("Misc", "Reload");

            #endregion
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            canClose = false;
            CefSettings settings = new CefSettings();
			settings.LogSeverity = LogSeverity.Disable;
            settings.CachePath = Config.CACHE_PATH;
            CefSharpSettings.WcfTimeout = TimeSpan.FromSeconds(0);
            if (Settings.Instance.Get("ProgramMode", "Login").Equals("Login"))
            {
                if (arguments.Length < 6)
                {
                    UserName = Settings.Instance.Get("UserName", "");
                    password = Settings.Instance.Get("Password", "");
                    server_address = Settings.Instance.Get("ServerGame", "");
                    ua = Settings.Instance.Get("UserAgent", "");
                    proxy = "None";
                    if (Settings.Instance.Get("EnableProxy", "false").Equals("true"))
                        proxy = Settings.Instance.Get("ProxyAddress", "");
                }
                else
                {
                    UserName = arguments[1];
                    password = arguments[2];
                    server_address = arguments[3];
                    ua = arguments[4];
                    proxy = arguments[5];
                }

                Text = Config.PROGRAM_NAME + " - " + UserName + " - " + Config.VERSION;

                if(UserName == "" || password == "" || server_address == "")
                {
                    MessageBox.Show("Please edit your configurations first in order to start", "Error");
                    Application.Exit();
                    return;
                }
                
                if(proxy != "None" && proxy != "")
                    settings.CefCommandLineArgs.Add("proxy-server", proxy);
                settings.CefCommandLineArgs.Add("user-agent", ua);
                Cef.Initialize(settings);

                browser = new ChromiumWebBrowser(string.Empty);
                string[] items = new string[] { server_address, UserName, password, ua, proxy, Application.StartupPath.Replace("\\", "/") + "/", Application.StartupPath.Replace("\\", "/") + "/" + settings.CachePath, Application.StartupPath.Replace("\\", "/") + "/Reports/", Cef.CefSharpVersion, Cef.CefVersion, Cef.ChromiumVersion, Config.VERSION, "" };
                for (int i = 0; i < items.Length; i++)
                    items[i] = HttpUtility.HtmlEncode(items[i]);
                if (arguments.Contains("auto_start_piracy") || arguments.Contains("skip_first_page"))
                    items[12] = "document.getElementById('loginForm').submit();";
                browser.LoadHtml(string.Format(IkariamPlus.Resources.about_blank.Replace("{", "{{").Replace("}", "}}").Replace('[', '{').Replace(']', '}'), items), "http://example/");
            }
            
            browser.BrowserSettings.FileAccessFromFileUrls = CefState.Enabled;
            browser.BrowserSettings.ApplicationCache = CefState.Enabled;
            js = new JSCallBack();
            browser.RegisterJsObject("callbackObj", js);

            panel1.Controls.Add(browser);
            browser.FrameLoadEnd += BrowserFrameLoadEnd;
        }

        private void BrowserFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            workingScripts.Text = lang.Get("WebView", "WorkingScripts") + " - ";
            //browser.ShowDevTools();

            if (isScriptLanguageCanRun)
            {
                browser.ExecuteScriptAsync("$('body').append('<div style=\\\'width: 100%; height: 100%; position: fixed; background: rgba(0,0,0,0.5); z-index: 999999; text-align: center; color: #FFFFFF; direction: ltr;\\\' id=\\\'canceluserInput\\\'>A script is running<br /><a href=\\\'javascript: void(0);\\\' onclick=\\\'callbackObj.stopScriptLanguage(); $(\\\\\'#canceluserInput\\\\\\').hide();\\\'>Abort script</a></div>');");
            }

            browser.ExecuteScriptAsync("setInterval(function(){ $('.btn-login').click(); }, 3000);");
            browser.ExecuteScriptAsync("setInterval(function(){ if($(\"#js_selectedCityAction1\").length != 0 && $(\"#sidebarWidget .cityactions .piracyRaid\").length != 0 && $(\"#js_selectedCityAction90\").length == 0) $(\"#sidebarWidget .cityactions\").append(\"<li id=\\\"js_selectedCityAction90\\\" class=\\\"piracyRaid\\\" title=\\\"Add to piracy list\\\"><a href=\\\"javascript: void(0);\\\" onclick=\\\"callbackObj.pirateList(getParameterByName('destinationCityId', $('#sidebarWidget .cityactions .piracyRaid a').attr('href')).toString(), $('#js_selectedCityName').html() + ' from ' + $('#js_selectedCityOwnerName').html())\\\">Add to piracy list</a></li>\"); }, 2000);");

            string currentView = HttpUtility.ParseQueryString(new Uri(e.Url).Query).Get("view");
            /*if (currentView != null && (currentView.Equals("worldmap_iso") || currentView.Equals("island")))
            {
                XmlNodeList locationsNames = _XMLConfig.GetElementsByTagName("Name");
                XmlNodeList locationsXY = _XMLConfig.GetElementsByTagName("XY");
                int count = locationsNames.Count;
                string totalTextLocations = "";
                for (int i = 0; i < count; i++)
                {
                    totalTextLocations += "<li ";
                    if (i == 0)
                        totalTextLocations += "class=\\\"first-child\\\"";
                    totalTextLocations += " selectvalue =\\\"" + locationsXY[i].InnerXml + "\\\"><a>[" + locationsXY[i].InnerXml + "] " + locationsNames[i].InnerXml + "</a></li>";
                }
                browser.ExecuteScriptAsync("$(\"#js_homeCitySelectContainer\").click(function(){ $(\"#dropDown_js_homeCitySelectContainer div\").css(\"height\", \"" + (32 * count).ToString() + "px\"); $(\"#dropDown_js_homeCitySelectContainer\").css({\"height\": \"" + (32 * count).ToString() + "px\", \"top\": \"auto\", \"bottom\": \"" + (20 + 32 * (locationsXY.Count - 2)).ToString() + "px\"}); }); $(\"#dropDown_js_homeCitySelectContainer div ul li:first\").removeClass(\"first-child\"); var initial = $(\"#dropDown_js_homeCitySelectContainer div ul\").html(); $(\"#dropDown_js_homeCitySelectContainer div ul\").html(\"" + totalTextLocations + "\" + initial);");
                if (currentView.Equals("island"))
                    browser.ExecuteScriptAsync("var Hoplite = " + Hoplite.ToString() + "; var SteamGiant = " + SteamGiant.ToString() + "; var Spearman = " + Spearman.ToString() + "; var Slinger = " + Slinger.ToString() + "; var Swordsman = " + Swordsman.ToString() + "; var Archer = " + Archer.ToString() + "; var SulphurCarabineer = " + SulphurCarabineer.ToString() + "; var Ram = " + Ram.ToString() + "; var Catapult = " + Catapult.ToString() + "; var Mortar = " + Mortar.ToString() + "; var Gyrocopter = " + Gyrocopter.ToString() + "; var BalloonBombardier = " + BalloonBombardier.ToString() + "; var Cook = " + Cook.ToString() + "; var Doctor = " + Doctor.ToString() + "; var TransporterShips = " + TransporterShips.ToString() + "; " + server.DownloadFile("ikariam_auto_fight") + " setInterval(\"writeScriptedData()\", 1000);");
            }
            else if (currentView != null && currentView.Equals("city"))
            {
                browser.ExecuteScriptAsync("var UserAPIKEY = \"" + _XMLConfig.DocumentElement.SelectSingleNode("/Bot/User/APIKey9K").InnerText + "\"; var UserAPISource = \"" + _XMLConfig.DocumentElement.SelectSingleNode("/Bot/User/APISource9K").InnerText + "\"; " + server.DownloadFile("ikariam_piracy"));
                browser.ExecuteScriptAsync(server.DownloadFile("ikariam_build_list"));

                XmlNodeList CitiesIDs = _XMLConfig.GetElementsByTagName("CityID");
                XmlNodeList CitiesNames = _XMLConfig.GetElementsByTagName("CityName");
                int count = CitiesIDs.Count;
                string totalTextCities = "";
                for (int i = 0; i < count; i++)
                    totalTextCities += "<li class=\\\"cityBox\\\"><a class=\\\"city_box_bg\\\" title=\\\"To " + CitiesNames[i].InnerXml + "\\\" onclick=\\\"ajaxHandlerCall(this.href);return false;\\\" href=\\\"?view=transport&destinationCityId=" + CitiesIDs[i].InnerXml + "&activeTab=tabSendTransporter&backgroundView=city&templateView=port\\\"><div class=\\\"boxContent\\\"><div class=\\\"coords\\\">Ikarim+</div><div class=\\\"symbol level1\\\" id=\\\"js_cityBoxLevel60132\\\"></div><div class=\\\"name\\\"><span class=\\\"before\\\"></span><span>" + CitiesNames[i].InnerXml + "</span><span class=\\\"after\\\"></span></div></div></a></li>";
                browser.ExecuteScriptAsync("$(\"#js_CityPosition1Link, #js_CityPosition2Link\").click(function(){ setTimeout(function(){ $(\"#tabSendTransporter ul\").append(\"" + totalTextCities + "\"); }, 3000); });");
            }*/
            browser.ExecuteScriptAsync(File.ReadAllText("Scripts/ikariam_html2canvas"));
            canClose = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseProgram();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.Reload();
        }

        public static void CloseProgram()
        {
            if (canClose && browser.IsBrowserInitialized)
            {
                Cef.Shutdown();
                Application.ExitThread();
                Application.Exit();
            } 
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseProgram();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string command = "https://github.com/Smoxer/";
            Process.Start(command);
        }

        private void startQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.ExecuteScriptAsync("start();// $(\"body\").append('<div style =\"width: 100%; height: 100%; position: fixed; background: rgba(0,0,0,0.5); z-index: 999999; text-align: center; color: #FFFFFF\" id=\"canceluserInput\">A script is running<br /><a href=\"javascript: void(0);\" onclick=\"callbackObj.resetQueue(); location = 'index.php?view=city';\">Abort script</a></div>');");
            if (workingScripts.Text != lang.Get("WebView", "WorkingScripts") + " - ")
                workingScripts.Text += ", Build list";
            else
                workingScripts.Text += " Build list";

            /*new Thread(delegate () {
                foreach (KeyValuePair<string, List<string>> building in buildQueue)
                {
                    if (building.Key != null)
                    {
                        browser.ExecuteScriptAsync("if ($(\"#dropDown_js_citySelectContainer\").css(\"display\") == \"none\"){ $(\"#js_citySelectContainer span\").click(); setTimeout(function() { $(\"#dropDown_js_citySelectContainer li[selectvalue='" + building.Key + "']\").click(); }, 500); } else $(\"#dropDown_js_citySelectContainer li[selectvalue='" + building.Key + "']\").click();");
                        Thread.Sleep(6000);
                        browser.ExecuteScriptAsync("$('#js_CityPosition" + building.Value.First() + "Link').click();");
                        Thread.Sleep(6000);
                        browser.ExecuteScriptAsync("if(!$(\"#js_buildingUpgradeButton\").hasClass(\"disabled\")) { $('#js_buildingUpgradeButton').click(); callbackObj.removeFromQueue('" + building.Key + "'); }");
                        Thread.Sleep(6000);
                    }
                }
            }).Start();*/
        }

        private void showQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.ExecuteScriptAsync("alertQueue();");
        }

        private void stopQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buildQueue.Clear();
            browser.ExecuteScriptAsync("buildList = []; $(\"#canceluserInput\").remove(); alert('Queue had restarted');");
            workingScripts.Text = workingScripts.Text.Replace(" Build list", "").Replace(", Build list", "");
        }

        private void searchByAllyTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*string UserAnswer = Interaction.InputBox("Please enter ally tag, split by \",\"\n\nPay attention: it is highly recommend to use proxy for this feature", "Search", "");
            if (UserAnswer != "")
            {
                browser.ExecuteScriptAsync("var UserName = \"" + UserAnswer + "\"; var opt = \"Ally\"; " + server.DownloadFile("ikariam_search_user"));
                workingScripts.Text = "Working scripts - Search";
            }*/
            browser.ExecuteScriptAsync("$(body).html('Searching . . . .');");
            using (var form = new Search(Search.ALLY, UserName, password, server_address, ua, proxy))
            {
                var result = form.ShowDialog();
            }
            browser.ExecuteScriptAsync("Location.reload();");
        }

        private void searchByPlayerNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*string UserAnswer = Interaction.InputBox("Please enter user name, split by \",\"\n\nPay attention: it is highly recommend to use proxy for this feature", "Search", "");
            if (UserAnswer != "")
            {
                browser.ExecuteScriptAsync("var UserName = \"" + UserAnswer + "\"; var opt = \"UserName\"; " + server.DownloadFile("ikariam_search_user"));
                workingScripts.Text = "Working scripts - Search";
            }*/
            browser.ExecuteScriptAsync("$(body).html('Searching . . . .');");
            using (var form = new Search(Search.USER_NAME, UserName, password, server_address, ua, proxy))
            {
                var result = form.ShowDialog();
            }
            browser.ExecuteScriptAsync("Location.reload();");
        }

        private void searchByCityNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*string UserAnswer = Interaction.InputBox("Please enter city name, split by \",\"\n\nPay attention: it is highly recommend to use proxy for this feature", "Search", "");
            if (UserAnswer != "")
            {
                browser.ExecuteScriptAsync("var UserName = \"" + UserAnswer + "\"; var opt = \"City\"; " + server.DownloadFile("ikariam_search_user"));
                workingScripts.Text = "Working scripts - Search";
            }*/
            browser.ExecuteScriptAsync("$(body).html('Searching . . . .');");
            using (var form = new Search(Search.CITY_NAME, UserName, password, server_address, ua, proxy))
            {
                var result = form.ShowDialog();
            }
            browser.ExecuteScriptAsync("Location.reload();");
        }

        private void startScriptToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string UserAnswer = Interaction.InputBox("Please enter cities IDs, split by \",\"", "Send spies", "");
            if (UserAnswer != "")
            {
                browser.ExecuteScriptAsync("var Citiesht = \"" + UserAnswer + "\"; " + File.ReadAllText("Scripts/ikariam_spy"));
                workingScripts.Text = lang.Get("WebView", "WorkingScripts") + " - Spy";
            }
        }

        private void editConfigurationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", "config.xml");
        }

        private void proxyOptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string txt = "Proxy enabled: " + (!proxy.Equals("None") ? "true" : "false") + "\n";
            txt += "Proxy address: " + proxy + "\n";
            txt += "Edit option in the config.xml file";
            MessageBox.Show(txt, "Proxy");
        }

        private void officialIkariamsWikiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.Load("http://ikariam.wikia.com");
        }

        private void cleanCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Cache is being cleaned", Config.PROGRAM_NAME);
            new Thread(delegate () {
                if (Directory.Exists("Reports/Islands" + server_address))
                    Array.ForEach(Directory.GetFiles("Reports/Islands" + server_address), File.Delete);
                MessageBox.Show("Cache cleaned", Config.PROGRAM_NAME);
            }).Start();
        }

        private void editScriptedAttackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!File.Exists("AttackedFight.ikr"))
            {
                File.Create("AttackedFight.ikr");
                Process.Start("notepad.exe", "AttackedFight.ikr");
            }
        }

        private void howToUseToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            js.Alert("Run the scripts -> Write islands coordinates which you want to start colonies seperated by commas (eg: \"98:5,50:50,X:Y\")", "How to use");
        }

        private void cSVReaderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CSV().Show();
        }

        private void islandsByTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new IslandsByTime().Show();
        }

        private void startScriptToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = Config.PROGRAM_NAME + "|*.ikr|All|*.*";
            if (file.ShowDialog() == DialogResult.OK)
            {
                Thread _ScriptLanguage = new Thread(scriptLanguage.RunScript);
                isScriptLanguageCanRun = true;
                _ScriptLanguage.Start(File.ReadAllText(file.FileName));
            }
        }

        private void piracyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This option is no longer available. Use \"Massive piracy use\" from the launcher.");
        }

        private void whatIsMyIPToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //MessageBox.Show("Your IP address is: " + server.GetIP() + "\nDoesn't work with proxy", "IP - " + Config.PROGRAM_NAME);
            browser.Load("http://whatismyipaddress.com/");
        }

        private void startPageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.Load("http://example/");
        }

        private void officialIkariamsForumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.Load("http://board." + server_address.Split('-')[1] + ".ikariam.gameforge.com/");
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("Launcher.exe");
            CloseProgram();
        }

        private void scriptMyselfFightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string totalText = "Hoplites: " + Hoplite.ToString() + "\n";
            totalText += "Steam giants: " + SteamGiant.ToString() + "\n";
            totalText += "Spearmen: " + Spearman.ToString() + "\n";
            totalText += "Slingers: " + Slinger.ToString() + "\n";
            totalText += "Swordsmen: " + Swordsman.ToString() + "\n";
            totalText += "Archers: " + Archer.ToString() + "\n";
            totalText += "Sulphur carabineers: " + SulphurCarabineer.ToString() + "\n";
            totalText += "Rams: " + Ram.ToString() + "\n";
            totalText += "Catapults: " + Catapult.ToString() + "\n";
            totalText += "Mortars: " + Mortar.ToString() + "\n";
            totalText += "Gyrocopters: " + Gyrocopter.ToString() + "\n";
            totalText += "Balloon bombardiers: " + BalloonBombardier.ToString() + "\n";
            totalText += "Cooks: " + Cook.ToString() + "\n";
            totalText += "Doctors: " + Doctor.ToString() + "\n";
            totalText += "Transporter ships: " + TransporterShips.ToString() + "\n";
            totalText += "\nWhen finished, go to the city and press \"Raid\"";
            MessageBox.Show(totalText, "Auto fight");
        }

        private void editScriptedRaidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ScriptEditor().Show();
        }

        private void buyMeABeerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Smoxer/");
        }

        private void militaryCalculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Military().Show();
        }

        private void captureAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.EvaluateScriptAsync("canvasAll();");
        }

        private void captureFightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.EvaluateScriptAsync("canvasFight();");
        }

        private void captureSpyReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.EvaluateScriptAsync("canvasSpy();");
        }

        private void attacksOnAllyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.ExecuteScriptAsync(File.ReadAllText("Scripts/ikariam_check_attack"));
            browser.EvaluateScriptAsync("var checkI = setInterval(checkAttackAlly, 60000); checkAttackAlly();");
            if (workingScripts.Text != lang.Get("WebView", "WorkingScripts") + " -")
                workingScripts.Text += ", Check attack";
            else
                workingScripts.Text += " Check attack";
        }

        private void attacksOnMeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.ExecuteScriptAsync(File.ReadAllText("Scripts/ikariam_check_attack"));
            browser.EvaluateScriptAsync("var checkI = setInterval(checkAttackMe, 60000); checkAttackMe();");
            if (workingScripts.Text != lang.Get("WebView", "WorkingScripts") + " -")
                workingScripts.Text += ", Check attack";
            else
                workingScripts.Text += " Check attack";
        }

        private void howToUseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            js.Alert("Open embassy -> Run the script (works only if you're the general of your ally)", "How to use");
        }

        private void howToUseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            js.Alert("Put a list of islands ids splited by comma (e.g: \"98:5,50:50,15:61,55:91,x:y\") and run the script. It'll open a new city on one of the islands the moment it can.", "How to use");
        }

        private void howToUseToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            js.Alert("Open fight report/spy report -> Run the script. It'll save the reports in [Ikariam+ Directory]/Reports/", "How to use");
        }

        private void howToUseToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            js.Alert("Run the scripts -> Write cities IDs which you want to send spies seperated by commas (eg: \"12345,54321\")", "How to use");
        }

        private void captureResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browser.EvaluateScriptAsync("canvasResult();");
        }
    }
}
