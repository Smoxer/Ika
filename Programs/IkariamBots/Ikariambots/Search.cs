using System;
using System.IO;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Library;

namespace IkariamBots
{
    public partial class Search : Form
    {
        public static int ALLY { get { return 1; } }
        public static int USER_NAME { get { return 2; } }
        public static int CITY_NAME { get { return 3; } }
        public static int CUSTOM { get { return 3; } }

        private static string[] wonders, goods;

        private int option, islandsPerTime;
        private PlayableWebClient client;
        private List<PlayableWebClient> clients;
        private string userName, password, server, actionRequest, url, _DateAndTime, proxy, UA;
        private string currentCity, currentStage; //For debug
        private ReaderWriterLockSlim lock_ = new ReaderWriterLockSlim();
        private List<short> islands;
        private List<string> data;
        private static Languages lang = Languages.Instance;
        private Stopwatch stopWatch;

        public Search(int option, string userName, string password, string server, string UA, string proxy)
        {
            InitializeComponent();

            #region Translate
            Text = lang.Get("Misc", "Search");
            label1.Text = lang.Get("Misc", "SearchBy");
            allyOption.Text = lang.Get("Misc", "Ally");
            userNameOption.Text = lang.Get("Misc", "Username");
            cityNameOption.Text = lang.Get("Piracy", "CityName");
            useCache.Text = lang.Get("Misc", "UseCache");
            start.Text = lang.Get("Misc", "Search");
            close.Text = lang.Get("Misc", "Close");
            speed.Text = lang.Get("Misc", "Speed");
            label2.Text = lang.Get("Misc", "Simultaneously");
            custom.Text = lang.Get("Misc", "Custom");
            #endregion

            this.option = option;
            this.userName = userName;
            this.password = password;
            this.server = server;
            this.UA = UA;
            this.proxy = proxy;
            islandsPerTime = 0;
            clients = new List<PlayableWebClient>();
            data = new List<string>();
            client = new PlayableWebClient();
            if (proxy != "None" && proxy != "")
                client.SetProxy(proxy);
            client.SetUA(UA);
            url = Config.PROTOCOL + string.Format("://{0}.ikariam.gameforge.com/index.php", server);

            useCache.Checked = true;
            progress.Minimum = 1;
            progress.Value = 1;
            progress.Step = 1;

            log.Items.Add(lang.Get("Misc", "LoginDetails") + ":");
            log.Items.Add("\t" + lang.Get("Misc", "Username") + ": " + userName);
            log.Items.Add("\t" + lang.Get("Misc", "Password") + ": " + new string('*', password.Length));
            log.Items.Add("\t" + lang.Get("Misc", "Server") + ": " + server);
            log.Items.Add("\t" + lang.Get("Misc", "Proxy") + ": " + proxy);

            allyOption.Checked = option.Equals(ALLY);
            userNameOption.Checked = option.Equals(USER_NAME);
            cityNameOption.Checked = option.Equals(CITY_NAME);
            custom.Checked = option.Equals(CUSTOM);

            wonders = new string[] {
                lang.Get("Misc", "Hephaestus"),
                lang.Get("Misc", "Hades"),
                lang.Get("Misc", "Demeter"),
                lang.Get("Misc", "Athena"),
                lang.Get("Misc", "Hermes"),
                lang.Get("Misc", "Ares"),
                lang.Get("Misc", "Poseidon"),
                lang.Get("Misc", "Colossus")
            };

            goods = new string[] {
                lang.Get("Misc", "Vineyard"),
                lang.Get("Misc", "Quarry"),
                lang.Get("Misc", "Crystal"),
                lang.Get("Misc", "Sulphur")
            };

            Thread speedCalculation = new Thread(delegate ()
            {
                while (true)
                {
                    if (null != islands && 0 != islandsPerTime)
                    {
                        if (InvokeRequired)
                            BeginInvoke(new Action(() => { speed.Text = lang.Get("Misc", "Speed") + ": " + (islandsPerTime - islands.Count).ToString() + " " + lang.Get("Misc", "Islands") + " / 3 " + lang.Get("Misc", "Seconds"); }));
                        else
                            speed.Text = lang.Get("Misc", "Speed") + ": " + (islandsPerTime - islands.Count).ToString() + " " + lang.Get("Misc", "Islands") + " / 3 " + lang.Get("Misc", "Seconds");
                    }
                    else
                    {
                        if (InvokeRequired)
                            BeginInvoke(new Action(() => { speed.Text = lang.Get("Misc", "Speed") + ": -- " + lang.Get("Misc", "Islands") + " / 3 " + lang.Get("Misc", "Seconds"); }));
                        else
                            speed.Text = lang.Get("Misc", "Speed") + ": -- " + lang.Get("Misc", "Islands") + " / 3 " + lang.Get("Misc", "Seconds");
                    }
                    if (null != islands)
                        islandsPerTime = islands.Count;
                    Thread.Sleep(3000);
                }
            });
            speedCalculation.IsBackground = true;
            speedCalculation.Start();
        }

        private void close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void allyOption_CheckedChanged(object sender, EventArgs e)
        {
            if (allyOption.Checked)
            {
                title.Text = lang.Get("Misc", "Ally") + ":";
            }
        }

        private void userNameOption_CheckedChanged(object sender, EventArgs e)
        {
            if (userNameOption.Checked)
            {
                title.Text = lang.Get("Misc", "Username") + ":";
            }
        }

        private void cityNameOption_CheckedChanged(object sender, EventArgs e)
        {
            if (cityNameOption.Checked)
            {
                title.Text = lang.Get("Misc", "CityName") + ":";
            }
        }

        private void custom_CheckedChanged(object sender, EventArgs e)
        {
            if (custom.Checked)
            {
                title.Text = lang.Get("Misc", "Cities") + " / " + lang.Get("Misc", "Islands") + ":";
            }
        }

        private void start_Click(object sender, EventArgs e)
        {
            start.Enabled = false;
            close.Enabled = false;
            if (value.Text.Equals(""))
            {
                log.Items.Add("Please enter ally/user/city name to search");
                start.Enabled = true;
                close.Enabled = true;
                return;
            }

            Thread t = new Thread(delegate ()
            {
                stopWatch = Stopwatch.StartNew();
                AddToLog(lang.Get("Misc", "ReloginTo") + " " + userName + " - " + server);
                bool cache = useCache.Checked;
                string search = value.Text;
                string cityId = "";
                int maxIslands = 0;

                MatchCollection cities_regex = null;
                try
                {
                    string response = string.Empty, parameters = string.Empty;
                    currentStage = "Login";
                    response = client.DownloadString(Config.PROTOCOL + string.Format("://{0}.ikariam.gameforge.com/index.php?view=city&action=loginAvatar&function=login&name={1}&password={2}", server, userName, password));
                    cityId = Regex.Match(response, "cityId=\\s*(.+?)\\s*\"").Groups[1].Value;
                    cities_regex = new Regex("{\\\\\"id\\\\\":(.+?),\\\\\"name\\\\\":\\\\\"(.+?)\\\\\",\\\\\"coords\\\\\":\\\\\"(.+?)\\\\\",\\\\\"tradegood\\\\\":\\\\\"(1|2|3|4)\\\\\",\\\\\"relationship\\\\\":\\\\\"ownCity\\\\\"}").Matches(response.Replace(" ", "").Replace("\n", "").Replace("\r", ""));
                    UpdateActionRequest(response);

                    currentStage = "Search city";
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
                                if (pirateFortress)
                                {
                                    cityId = city.Groups[1].Value;
                                }
                            }
                            catch (Exception exc) { AddToLog(exc.ToString()); }

                        }
                    }
                    AddToLog(lang.Get("Misc", "FetchingWorld"));
                    currentStage = "Read world";
                    if (IsIslandExist("0"))
                    {
                        response = ReadIsland("0");
                    }
                    else
                    {
                        parameters = "action=WorldMap&function=getJSONArea&x_min=-100&x_max=100&&y_min=-100&y_max=100";
                        response = client.DownloadString(url + "?" + parameters);
                        SaveIsland("0", response);
                    }
                    currentStage = "Calculating islands";
                    var world = JObject.Parse(response);
                    islands = new List<short>();
                    foreach (var x in world["data"])
                    {
                        foreach (var y in x.First)
                        {
                            islands.Add(short.Parse(y.First[0].ToString()));
                            maxIslands++;
                        }
                    }
                    islands.Sort();
                    AddToLog(maxIslands.ToString() + " " + lang.Get("Misc", "IslandsToScan"));
                    Invoke(new Action(() => { progress.Maximum = maxIslands; }));

                    for (int i = 0; i < simultaneously.Value; i++)
                    {
                        clients.Add(new PlayableWebClient(client.CookieContainer, proxy, UA));
                    }

                    currentStage = "Creating report";
                    
                    if (!custom.Checked)
                    {
                        CreateReport();
                        foreach (PlayableWebClient user in clients)
                        {
                            Thread searchThread = new Thread(delegate ()
                            {
                                Thread.Sleep(500);
                                string responsePart = string.Empty, param = string.Empty, currentIsland = string.Empty;
                                while (islands.Count > 0)
                                {
                                    try
                                    {
                                        currentIsland = islands[0].ToString();
                                        islands.RemoveAt(0);
                                    //Thread.Sleep(250);
                                    if (InvokeRequired)
                                            Invoke(new Action(() => { progress.PerformStep(); }));
                                        else
                                            progress.PerformStep();
                                        currentStage = "Fetching island";
                                        if (cache && File.Exists("Reports/Islands" + server + "/" + currentIsland + ".js"))//IsIslandExist(currentIsland))
                                    {
                                            responsePart = ReadIsland(currentIsland);
                                        }
                                        else
                                        {
                                            param = "view=updateGlobalData&islandId=" + currentIsland + "&backgroundView=island&currentIslandId=" + currentIsland + "&ajax=1";
                                            responsePart = user.DownloadString(url + "?" + param);
                                            SaveIsland(currentIsland, responsePart);
                                        }

                                        if (responsePart.Equals(string.Empty) || responsePart.Equals(""))
                                        {
                                            param = "view=updateGlobalData&islandId=" + currentIsland + "&backgroundView=island&currentIslandId=" + currentIsland + "&ajax=1";
                                            responsePart = user.DownloadString(url + "?" + param);
                                            SaveIsland(currentIsland, responsePart);
                                        }

                                        currentStage = "Reading island";
                                        JArray island = JArray.Parse(responsePart);
                                        foreach (var city in island[0][1]["backgroundData"]["cities"])
                                        {
                                            currentCity = city.ToString();
                                            currentStage = "If location is city";
                                            if (!city["id"].ToString().Equals("-1") && city["type"].ToString().Equals("city"))
                                            {
                                                currentStage = "Get wonder";
                                                string wonder = GetWonerById(int.Parse(island[0][1]["backgroundData"]["wonder"].ToString())) + " (" + island[0][1]["backgroundData"]["wonderLevel"].ToString() + ")";

                                                currentStage = "Get goods";
                                                string tradeGood = GetGoodById(int.Parse(island[0][1]["backgroundData"]["tradegood"].ToString())) + " (" + island[0][1]["backgroundData"]["tradegoodLevel"].ToString() + ")"; ;

                                                currentStage = "Get ally";
                                                string ally = "None";
                                                if (null != city["ownerAllyTag"] && !city["ownerAllyTag"].ToString().Equals(""))
                                                    ally = city["ownerAllyTag"].ToString();

                                                currentStage = "Condition by user";
                                                bool condition =
                                                    (allyOption.Checked && (
                                                            (!city["ownerAllyId"].ToString().Equals("0") && city["ownerAllyTag"].ToString().ToLower().Contains(search.ToLower())) ||
                                                            (null != city["infos"] && null != city["infos"]["occupation"] && city["infos"]["occupation"]["name"].ToString().ToLower().Contains(search.ToLower())) ||
                                                            (null != city["infos"] && null != city["infos"]["fleetAction"] && city["infos"]["fleetAction"]["name"].ToString().ToLower().Contains(search.ToLower()))
                                                        )
                                                    ) ||
                                                    (cityNameOption.Checked && (
                                                            (city["name"].ToString().ToLower().Contains(search.ToLower())) ||
                                                            (null != city["infos"] && null != city["infos"]["occupation"] && city["infos"]["occupation"]["name"].ToString().ToLower().Contains(search.ToLower())) ||
                                                            (null != city["infos"] && null != city["infos"]["fleetAction"] && city["infos"]["fleetAction"]["name"].ToString().ToLower().Contains(search.ToLower()))
                                                        )
                                                    ) ||
                                                    (userNameOption.Checked && (
                                                            (city["ownerName"].ToString().ToLower().Contains(search.ToLower())) ||
                                                            (null != city["infos"] && null != city["infos"]["occupation"] && city["infos"]["occupation"]["name"].ToString().ToLower().Contains(search.ToLower())) ||
                                                            (null != city["infos"] && null != city["infos"]["fleetAction"] && city["infos"]["fleetAction"]["name"].ToString().ToLower().Contains(search.ToLower()))
                                                        )
                                                    );
                                                if (condition)
                                                {
                                                    currentStage = "Retrive basics";
                                                    string txt = city["ownerName"].ToString() + " (" + (city["ownerAllyId"].ToString().Equals("0") ? "" : city["ownerAllyTag"].ToString()) + ")";
                                                    if (city["state"].ToString().Equals("vacation"))
                                                        txt += " - " + lang.Get("Misc", "Vacation");

                                                    currentStage = "Retrive city";
                                                    if (cache && File.Exists("Reports/Islands" + server + "/c" + city["id"].ToString() + ".js"))//IsIslandExist("c" + city["id"].ToString()))
                                                {
                                                        responsePart = ReadIsland("c" + city["id"].ToString());
                                                    }
                                                    else
                                                    {
                                                        param = "view=noViewChange&action=Options&function=toggleCityBuildingNameOption&cityId=" + city["id"].ToString() + "&currentCityId=" + city["id"].ToString() + "&oldBackgroundView=city&backgroundView=city&ajax=1";
                                                        responsePart = client.DownloadString(url + "?" + param);
                                                        SaveIsland("c" + city["id"].ToString(), responsePart);
                                                    }

                                                    if (responsePart.Equals(string.Empty) || responsePart.Equals(""))
                                                    {
                                                        param = "view=noViewChange&action=Options&function=toggleCityBuildingNameOption&cityId=" + city["id"].ToString() + "&currentCityId=" + city["id"].ToString() + "&oldBackgroundView=city&backgroundView=city&ajax=1";
                                                        responsePart = client.DownloadString(url + "?" + param);
                                                        SaveIsland("c" + city["id"].ToString(), responsePart);
                                                    }

                                                    var cityBuildings = JArray.Parse(responsePart);
                                                    string totalBuildings = "";
                                                    if (null != cityBuildings[0][1]["backgroundData"] && !cityBuildings[0][1]["backgroundData"].ToString().Equals("") && !cityBuildings[0][1]["backgroundData"].ToString().Equals("\"\""))
                                                    {
                                                        var buildings = cityBuildings[0][1]["backgroundData"]["position"];
                                                        if (cityBuildings[0][1]["backgroundData"]["id"].ToString().Equals(city["id"].ToString()))
                                                        {
                                                            foreach (var building in buildings)
                                                            {
                                                                totalBuildings += building["building"] + ":" + building["level"] + ",";
                                                            }
                                                            totalBuildings = totalBuildings.Remove(totalBuildings.Length - 1);
                                                        }
                                                    }


                                                    string[] row = {
                                                    island[0][1]["backgroundData"]["xCoord"].ToString(),
                                                    island[0][1]["backgroundData"]["yCoord"].ToString(),
                                                    island[0][1]["backgroundData"]["name"].ToString(),
                                                    city["name"].ToString(),
                                                    city["ownerName"].ToString(),
                                                    (!city["actions"].ToString().Equals("") && !city["actions"].ToString().Equals("[]") && null != city["actions"]["piracy_raid"] ? lang.Get("Misc", "Yes") : lang.Get("Misc", "No")),
                                                    ally,
                                                    (city["state"].ToString().Equals("vacation") ? lang.Get("Misc", "Yes") : lang.Get("Misc", "No")),
                                                    wonder,
                                                    tradeGood,
                                                    "Saw Mill (" + island[0][1]["backgroundData"]["resourceLevel"] + ")",
                                                    (null != city["infos"] && null != city["infos"]["occupation"] ? lang.Get("Misc", "Yes") : lang.Get("Misc", "No")),
                                                    (null != city["infos"] && null != city["infos"]["fleetAction"] ? lang.Get("Misc", "Yes") : lang.Get("Misc", "No")),
                                                    city["id"].ToString(),
                                                    island[0][1]["backgroundData"]["id"].ToString(),
                                                    totalBuildings
                                                    };
                                                    if (!data.Contains(string.Join(",", row)))
                                                    {
                                                        data.Add(string.Join(",", row));
                                                        AddToLog(txt);
                                                    }
                                                }

                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                    //AddToLog(ex.Message);
                                    islands.Add(short.Parse(currentIsland));
                                    }
                                }
                            });
                            searchThread.IsBackground = true;
                            searchThread.Start();
                        }
                        new Thread(delegate ()
                        {
                            while (islands.Count > 0)
                            {
                                Thread.Sleep(5000);
                            }

                            foreach (string row in data.ToArray())
                            {
                                AddToReport(row);
                            }
                            if (InvokeRequired)
                                BeginInvoke(new Action(() => { start.Enabled = true; close.Enabled = true; }));
                            else
                            {
                                start.Enabled = true; close.Enabled = true;
                            }

                            stopWatch.Stop();
                            MessageBox.Show("Search " + _DateAndTime + " completed\nTotal " + stopWatch.Elapsed.TotalSeconds + " seconds", "Search completed");
                            SaveFileDialog sd = new SaveFileDialog();
                            sd.Filter = Config.PROGRAM_NAME + " (.csv)|*.csv";
                            Invoke((Action)(() =>
                            {
                                if (DialogResult.OK == sd.ShowDialog())
                                {
                                    File.WriteAllText(sd.FileName, File.ReadAllText("Report" + _DateAndTime + ".csv"), System.Text.Encoding.UTF8);
                                    File.Delete("Report" + _DateAndTime + ".csv");
                                }
                                Close();
                            }));

                        }).Start();
                    }
                    else
                    {
                        CreateReport(1);
                        foreach (var x in world["data"])
                        {
                            foreach (var y in x.First)
                            {
                                if (InvokeRequired)
                                    Invoke(new Action(() => { progress.PerformStep(); }));
                                else
                                    progress.PerformStep();
                                try
                                {
                                    short howManyInIslands = 0, howManyInlandsRequested = 0;
                                    short.TryParse(y.First[7].ToString(), out howManyInIslands);
                                    short.TryParse(value.Text, out howManyInlandsRequested);

                                    if (howManyInIslands >= howManyInlandsRequested)
                                    {
                                        AddToLog(x.ToString().Split('"')[1] + ":" + y.ToString().Split('"')[1] + " - " + y.First[1].ToString() + " - " + howManyInIslands.ToString());
                                        string[] row = {
                                                    x.ToString().Split('"')[1],
                                                    y.ToString().Split('"')[1],
                                                    howManyInIslands.ToString(),
                                                    y.First[1].ToString(),
                                                    GetWonerById(int.Parse(y.First[3].ToString())),
                                                    GetGoodById(int.Parse(y.First[2].ToString())),
                                                    y.First[0].ToString()
                                                    };
                                        AddToReport(string.Join(", ", row));
                                    }
                                } catch(Exception ex) { }
                            }
                        }
                        MessageBox.Show("Search " + _DateAndTime + " completed\nTotal " + stopWatch.Elapsed.TotalSeconds + " seconds", "Search completed");
                        SaveFileDialog sd = new SaveFileDialog();
                        sd.Filter = Config.PROGRAM_NAME + " (.csv)|*.csv";
                        Invoke((Action)(() =>
                        {
                            if (DialogResult.OK == sd.ShowDialog())
                            {
                                File.WriteAllText(sd.FileName, File.ReadAllText("Report" + _DateAndTime + ".csv"), System.Text.Encoding.UTF8);
                                File.Delete("Report" + _DateAndTime + ".csv");
                            }
                            Close();
                        }));
                    }
                }
                catch (Exception ex) { MessageBox.Show("Current city: " + currentCity + ", current stage: " + currentStage + "\n\n\n" + ex.ToString()); }

            });
            t.IsBackground = true;
            t.Start();
            
        }

        private string GetWonerById(int id)
        {
            if (id > 0 && id <= wonders.Length)
                return wonders[id - 1];
            else
                return string.Empty;
        }

        private string GetGoodById(int id)
        {
            if (id > 0 && id <= goods.Length)
                return goods[id - 1];
            else
                return string.Empty;
        }

        private void AddToLog(string msg)
        {
            if (InvokeRequired)
                BeginInvoke(new Action(() => { log.Items.Add(msg); }));
            //log.Items.Add(msg);
        }

        public bool IsIslandExist(string islandID)
        {
            bool result = false;
            lock_.EnterReadLock();
            try
            {
                if (!Directory.Exists("Reports/Islands" + server + "/"))
                {
                    Directory.CreateDirectory("Reports/Islands" + server + "/");
                }

                result = File.Exists("Reports/Islands" + server + "/" + islandID + ".js");
            }
            finally
            {
                lock_.ExitReadLock();
            }
            return result;
        }

        public string ReadIsland(string islandID)
        {
            string x = string.Empty;
            lock_.EnterReadLock();
            try
            {
                x = File.ReadAllText("Reports/Islands" + server + "/" + islandID + ".js");
            }
            finally
            {
                lock_.ExitReadLock();
            }
            return x;
        }

        public void SaveIsland(string islandID, string data)
        {
            lock_.EnterWriteLock();
            try
            {
                if (!Directory.Exists("Reports/Islands" + server + "/"))
                {
                    Directory.CreateDirectory("Reports/Islands" + server + "/");
                }

                File.WriteAllText("Reports/Islands" + server + "/" + islandID + ".js", data);
            }
            finally
            {
                lock_.ExitWriteLock();
            }
        }

        public void CreateReport(int mode = 0)
        {
            _DateAndTime = DateTime.Now.ToString("MMddyyyy-HHmm");

            if (!File.Exists("Report" + _DateAndTime + ".csv"))
            {
                string txt = "";
                if (0 == mode)
                {
                    txt = "X, Y, " + lang.Get("Misc", "IslandName") + ", " + lang.Get("Piracy", "CityName") + ", ";
                    txt += lang.Get("Misc", "OwnerName") + ", " + lang.Get("Misc", "Pirate") + ", " + lang.Get("Misc", "Ally") + ", ";
                    txt += lang.Get("Misc", "Vacation") + ", " + lang.Get("Misc", "Wonder") + ", " + lang.Get("Misc", "Goods") + ", ";
                    txt += lang.Get("Misc", "SawMill") + ", " + lang.Get("Misc", "IsCityOccupied") + ", " + lang.Get("Misc", "IsPortOccupied") + ", ";
                    txt += lang.Get("Piracy", "CityID") + ", " + lang.Get("Piracy", "IslandID") + ", " + lang.Get("Misc", "Buildings");
                    for (int i = 0; i < 18; i++)
                        txt += ", " + lang.Get("Misc", "Buildings");
                }
                else if(1 == mode)
                {
                    txt = "X, Y, " + lang.Get("Misc", "Cities") + " / " + lang.Get("Misc", "Islands") + ", " + lang.Get("Misc", "IslandName") + ", " + lang.Get("Misc", "Wonder") + ", " + lang.Get("Misc", "Goods") + ", " + lang.Get("Piracy", "IslandID");
                }
                txt += "\nBy Ika+(https://github.com/Smoxer/)\n";

                File.WriteAllText("Report" + _DateAndTime + ".csv", txt, System.Text.Encoding.UTF8);
            }
        }

        public void AddToReport(string row)
        {
            lock_.EnterWriteLock();
            try
            {
                File.AppendAllText("Report" + _DateAndTime + ".csv", row + "\n", System.Text.Encoding.UTF8);
            }
            finally
            {
                lock_.ExitWriteLock();
            }
            
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

            if (updateUI)
            {
                Match capturePoints = Regex.Match(response, ",\\\\\"capturePoints\\\\\":\\\\\"(.+?)\\\\\",");

                Match crewStrength = new Regex(",\\\\\"completeCrewPoints\\\\\":(.+?),").Match(response);

                return crewStrength.Success && capturePoints.Success;
            }
            return false;
        }
    }
}
