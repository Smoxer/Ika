using System;
using IkariamBots;
using System.Threading;
using Captchas;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Library;

namespace Ikariam
{
    public class User
    {
        //Constants
        public static string CANT_FETCH { get { return "Can't fetch"; } }

        private enum WAIT : int {
            ONE_MINUTE = 60000,
            ONE_AND_HALF_MINUTE = 90000,
            FIVE_MINUTES = 300000,
            EIGHT_MINUTES = 480000,
            THREE_HOURS = 10800000
        };
        public enum RESEARCH : int
        {
            SEA_FARING = 0,
            ECONOMY = 1,
            KNOWLEDGE = 2,
            MILITARY = 3
        };

        //Server details
        private Network network;
        private string url;
        private Random random;
        private Connection connection;

        //User details
        private string server, userName, password;

        //Cities details
        private City currentCity;

        /// <summary>
        /// Initialize Ikariam user
        /// <para>server - Ikariam server (eg.: "s1-us")</para>
        /// <para>userName - User's username</para>
        /// <para>password - User's password</para>
        /// <para>connection - New connection object</para>
        /// </summary>
        public User(string server, string userName, string password, Connection connection)
        {
            this.server = server;
            this.userName = userName;
            this.password = password;
            this.connection = connection;
            random = new Random();

            url = Config.PROTOCOL + string.Format("://{0}.ikariam.gameforge.com/index.php", this.server);
            network = new Network(connection, url);
        }

        /// <summary>
        /// Login to the user
        /// </summary>
        public bool Login(string name = "")
        {
            if (string.IsNullOrEmpty(name))
            {
                name = userName;
            }
            Network.NetworkSettings settings = new Network.NetworkSettings("view=city&action=loginAvatar&function=login", null);
            settings.method = "POST";
            settings.data = "name=" + name + "&password=" + password + "&pwat_uid=&pwat_checksum=&startPageShown=1&detectedDevice=1&kid&autoLogin=on";
            settings.checkError = false;
            string login = network.AjaxHandlerRequest(settings);

            try
            {
                settings = new Network.NetworkSettings("view=city&ajax=1", null);
                settings.checkError = true;
                network.AjaxHandlerRequest(settings);
            } catch(Exceptions.ActionRequestException e) { return false;  }

            string currentCityId = Regex.Match(login, "cityId=\\s*(.+?)\\s*\"").Groups[1].Value;
            currentCity = FetchCity(currentCityId);

            return null != currentCity;
        }

        /// <summary>
        /// Relogin to the user
        /// </summary>
        public bool Relogin()
        {
            network = new Network(connection, url);
            return Login();
        }

        /// <summary>
        /// Logout from the user
        /// </summary>
        public void Logout()
        {
            network.AjaxHandlerRequest(string.Format("action=logoutAvatar&function=logout&backgroundView=city&currentCityId={0}&templateView=townHall", currentCity.ID), Relogin);
            network.AjaxHandlerRequest("logout=1", Relogin);
            network = new Network(connection, url);
        }

        public List<City> FetchAllCities()
        {
            List<City> allCities = new List<City>();
            string response = network.AjaxHandlerRequest(new Network.NetworkSettings("view=city", Relogin));
            MatchCollection cities_regex = new Regex("{\\\\\"id\\\\\":(.+?),\\\\\"name\\\\\":\\\\\"(.+?)\\\\\",\\\\\"coords\\\\\":\\\\\"(.+?)\\\\\",\\\\\"tradegood\\\\\":\\\\\"(1|2|3|4)\\\\\",\\\\\"relationship\\\\\":\\\\\"ownCity\\\\\"}").Matches(response.Replace(" ", "").Replace("\n", "").Replace("\r", ""));

            foreach (Match city in cities_regex)
            {
                if (city.Success)
                {
                    string cityData = network.AjaxHandlerRequest(string.Format("view=pirateFortress&activeTab=tabBootyQuest&cityId={0}&position=17&backgroundView=city&currentCityId={0}&templateView=pirateFortress", city.Groups[1].Value), Relogin);
                    string islandId = Regex.Match(cityData.Replace(" ", ""), "id=\"js_islandBread\"class=\"yellow\"href=\"\\?view=island&islandId=(.+?)\"").Groups[1].Value;
                    Match capturePoints = new Regex(",\\\\\"capturePoints\\\\\":\\\\\"(.+?)\\\\\",").Match(cityData);
                    Match crewStrength = new Regex(",\\\\\"completeCrewPoints\\\\\":(.+?),").Match(cityData);

                    cityData = network.AjaxHandlerRequest(string.Format("view=pirateFortress&activeTab=tabBootyQuest&cityId={0}&position=17&backgroundView=city&currentCityId={0}&templateView=pirateFortress&ajax=1", city.Groups[1].Value), Relogin);
                    
                    string cityName = Regex.Replace(
                        city.Groups[2].Value.Replace("\\\\", "\\"),
                        @"\\u(?<Value>[a-zA-Z0-9]{4})",
                        m =>
                        {
                            return ((char)int.Parse(m.Groups["Value"].Value, System.Globalization.NumberStyles.HexNumber)).ToString();
                        }
                    );
                   
                    allCities.Add(new City(
                        cityName,
                        city.Groups[1].Value,
                        city.Groups[3].Value,
                        islandId,
                        crewStrength.Success && capturePoints.Success
                    ));
                }
            }

            return allCities;
        }

        public City FetchCity(string cityId)
        {
            string response = network.AjaxHandlerRequest(new Network.NetworkSettings("view=city", Relogin));
            MatchCollection cities_regex = new Regex("{\\\\\"id\\\\\":(.+?),\\\\\"name\\\\\":\\\\\"(.+?)\\\\\",\\\\\"coords\\\\\":\\\\\"(.+?)\\\\\",\\\\\"tradegood\\\\\":\\\\\"(1|2|3|4)\\\\\",\\\\\"relationship\\\\\":\\\\\"ownCity\\\\\"}").Matches(response.Replace(" ", "").Replace("\n", "").Replace("\r", ""));
            City result = currentCity;

            foreach (Match city in cities_regex)
            {
                if (city.Success && city.Groups[1].Value.Equals(cityId))
                {
                    string cityData = network.AjaxHandlerRequest(string.Format("view=pirateFortress&activeTab=tabBootyQuest&cityId={0}&position=17&backgroundView=city&currentCityId={0}&templateView=pirateFortress", city.Groups[1].Value), Relogin);
                    string islandId = Regex.Match(cityData.Replace(" ", ""), "id=\"js_islandBread\"class=\"yellow\"href=\"\\?view=island&islandId=(.+?)\"").Groups[1].Value;
                    Match capturePoints = new Regex(",\\\\\"capturePoints\\\\\":\\\\\"(.+?)\\\\\",").Match(cityData);
                    Match crewStrength = new Regex(",\\\\\"completeCrewPoints\\\\\":(.+?),").Match(cityData);

                    cityData = network.AjaxHandlerRequest(string.Format("view=pirateFortress&activeTab=tabBootyQuest&cityId={0}&position=17&backgroundView=city&currentCityId={0}&templateView=pirateFortress&ajax=1", city.Groups[1].Value), Relogin);

                    string cityName = Regex.Replace(
                        city.Groups[2].Value.Replace("\\\\", "\\"),
                        @"\\u(?<Value>[a-zA-Z0-9]{4})",
                        m =>
                        {
                            return ((char)int.Parse(m.Groups["Value"].Value, System.Globalization.NumberStyles.HexNumber)).ToString();
                        }
                    );

                    result = new City(
                        cityName,
                        city.Groups[1].Value,
                        city.Groups[3].Value,
                        islandId,
                        crewStrength.Success && capturePoints.Success
                    );
                }
            }

            return result;
        }

        public bool IsEMailVerified()
        {
            string response = network.AjaxHandlerRequest(new Network.NetworkSettings(string.Format("view=options&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin));

            return !response.Contains("emailInvalidWarning");
        }

        public bool IsUnderAttack()
        {
            string response = network.AjaxHandlerRequest(string.Format("view=pirateFortress&activeTab=tabBootyQuest&cityId={0}&position=17&backgroundView=city&currentCityId={0}&templateView=pirateFortress&ajax=1", currentCity.ID), Relogin).Replace(" ", "").Replace("\n", "").Replace("\r", "");

            return response.Contains("\"advisors\":{\"military\":{\"link\":\"?view=militaryAdvisor&oldView=pirateFortress&cityId=" + currentCity.ID + "&position=17\",\"cssclass\":\"normalalert\",\"premiumlink\":\"?view=premiumDetails&oldView=pirateFortress&cityId=" + currentCity.ID + "&position=17\"},");
        }

        public string GetCapturePoints()
        {
            string cityData = network.AjaxHandlerRequest(string.Format("view=pirateFortress&activeTab=tabBootyQuest&cityId={0}&position=17&backgroundView=city&currentCityId={0}&templateView=pirateFortress", currentCity.ID), Relogin);
            Match capturePoints = new Regex(",\\\\\"capturePoints\\\\\":\\\\\"(.+?)\\\\\",").Match(cityData);

            return capturePoints.Groups[1].Value;
        }

        public string GetCrewStrength()
        {
            string cityData = network.AjaxHandlerRequest(string.Format("view=pirateFortress&activeTab=tabBootyQuest&cityId={0}&position=17&backgroundView=city&currentCityId={0}&templateView=pirateFortress", currentCity.ID), Relogin);
            Match crewStrength = new Regex(",\\\\\"completeCrewPoints\\\\\":(.+?),").Match(cityData);

            return crewStrength.Groups[1].Value;
        }

        public int GetCapturePointsToday()
        {
            int capturePointsTodayInt = -1;

            string response = network.AjaxHandlerRequest(new Network.NetworkSettings(string.Format("view=dailyTasks&oldBackgroundView=city&cityWorldviewScale=1&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin));
            response = response.Replace(" ", "").Replace("\n", "").Replace("\r", "");

            GroupCollection capturePointsToday = Regex.Match(response, Regex.Escape("<\\/td>\\n\\n<tdclass=\\\"rightsmallright\\\">") + "([0-9,]+)" + Regex.Escape("<\\/td><tdclass=\\\"centersmall\\\">&nbsp;\\/&nbsp;<\\/td><tdclass=\\\"leftsmall\\\">1,500<")).Groups;
            GroupCollection capturePointsTodayLowOption = Regex.Match(response, Regex.Escape("<\\/td>\\n\\n<tdclass=\\\"rightsmallright\\\">") + "([0-9,]+)" + Regex.Escape("<\\/td><tdclass=\\\"centersmall\\\">&nbsp;\\/&nbsp;<\\/td><tdclass=\\\"leftsmall\\\">500<\\/td><tdclass=\\\"leftsmallleft\\\"><imgsrc=\\\"skin\\/resources\\/capturePoints_small.png")).Groups;
            if (capturePointsToday.Count > 0 && !capturePointsToday[1].Value.Equals(""))
            {
                capturePointsTodayInt = int.Parse(capturePointsToday[1].Value.Replace(",", ""));
            }
            else if (capturePointsTodayLowOption.Count > 0 && !capturePointsTodayLowOption[1].Value.Equals(""))
            {
                capturePointsTodayInt = int.Parse(capturePointsTodayLowOption[1].Value.Replace(",", ""));
            }
            return capturePointsTodayInt;
        }

        public void PirateMission(string missionLevel, CaptchaSolver captchaSolver)
        {
            string response = network.AjaxHandlerRequest(string.Format("action=PiracyScreen&function=capture&buildingLevel={1}&view=pirateFortress&cityId={0}&position=17&activeTab=tabBootyQuest&backgroundView=city&currentCityId={0}&templateView=pirateFortress", currentCity.ID, missionLevel), Relogin);

            if (response.Contains("captchaNeeded=1"))
            {
                byte[] captchaData = null;
                try
                {
                    captchaData = network.DownloadData(url + "?action=Options&function=createCaptcha&rand=" + random.Next(0, 99999).ToString());
                }
                catch (Exception exc) { }

                //Captcha solve
                captchaSolver.image = captchaData;

                string captchaID;
                bool success;
                string captchaValue = captchaSolver.SendCaptcha(out captchaID, out success);
                if (!success)
                {
                    captchaValue = "None";
                }

                network.AjaxHandlerRequest(string.Format("action=PiracyScreen&function=capture&cityId={0}&position=17&captchaNeeded=1&activeTab=tabBootyQuest&backgroundView=city&templateView=pirateFortress&currentCityId={0}&buildingLevel={1}&ajax=1&captcha={2}", currentCity.ID, missionLevel, captchaValue), Relogin);

                if (success)
                    captchaSolver.CaptchaWorked(captchaID, !response.Contains("captchaNeeded=1"));
            }
        }

        public void PirateMission(int missionLevel, CaptchaSolver captchaSolver)
        {
            PirateMission(missionLevel.ToString(), captchaSolver);
        }

        public void ConvertToCrew(int crew)
        {
            network.AjaxHandlerRequest(new Network.NetworkSettings(string.Format("action=PiracyScreen&function=convert&view=pirateFortress&crewPoints={1}&cityId={0}&position=17&activeTab=tabCrew&backgroundView=city&currentCityId={0}&templateView=pirateFortress&ajax=1", currentCity.ID, crew.ToString()), Relogin));
        }

        public void SendPM(string to, string message)
        {
            network.AjaxHandlerRequest(string.Format("action=Messages&function=send&receiverId={0}&closeView=1&msgType=50&content={1}&isMission=0&allyId=0&backgroundView=city&currentCityId={2}&templateView=sendIKMessage&ajax=1", to, Uri.EscapeDataString(message), currentCity.ID), Relogin);
        }

        public void SendCulturalAssetTreaty(string to)
        {
            network.AjaxHandlerRequest(string.Format("action=Messages&function=send&receiverId={0}&msgType=77&content=&isMission=0&closeView=0&allyId=0&backgroundView=city&currentCityId={1}&templateView=sendIKMessage&ajax=1", to, currentCity.ID), Relogin);
        }

        public bool IsHaveCulturalAssetTreaty(string userId)
        {
            string response = network.AjaxHandlerRequest(string.Format("view=sendIKMessage&receiverId={0}&backgroundView=city&currentCityId={1}&templateView=highscore&ajax=1", userId, currentCity.ID), Relogin);
            
            return !response.Replace(" ", "").Contains("<optionvalue=\\\"77\\\">");
        }

        public void RaidCity(string raidCityId, string raidIslandId)
        {
            network.AjaxHandlerRequest(string.Format("action=PiracyScreen&function=raid&destinationIslandId={2}&destinationCityId={1}&backgroundView=city&currentCityId={0}&templateView=piracyRaid&ajax=1", currentCity.ID, raidCityId, raidIslandId), Relogin);
        }

        public void SetCurrentCity(string currentCityId)
        {
            currentCity = FetchCity(currentCityId);
        }

        public void SetCurrentCity(City currentCity)
        {
            this.currentCity = currentCity;
        }

        public City GetCurrentCity()
        {
            return currentCity;
        }

        public void BuildBuilding(City.BUILDINGS building, City.POSITIONS position)
        {
            network.AjaxHandlerRequest(string.Format("action=CityScreen&function=build&cityId={0}&position={2}&building={1}&backgroundView=city&currentCityId={0}&templateView=buildingGround&ajax=1", currentCity.ID, building.ToString(), position.ToString()), Relogin);
        }

        public void UpgradeBuilding(City.POSITIONS position)
        {
            network.AjaxHandlerRequest(string.Format("action=CityScreen&function=upgradeBuilding&cityId={0}&position={1}&activeTab=tabBootyQuest&backgroundView=city&currentCityId={0}&templateView=pirateFortress&ajax=1", currentCity.ID, position.ToString()), Relogin);
        }

        public void BuildingSpeedUp(City.POSITIONS position)
        {
            network.AjaxHandlerRequest(string.Format("action=Premium&function=buildingSpeedup&cityId={0}&position={1}&level=0&backgroundView=city&currentCityId={0}&templateView=pirateFortress&currentTab=tabBootyQuest&ajax=1", currentCity.ID, position.ToString()), Relogin);
        }

        public List<string> GetHighScoreTable(int page)
        {
            List<string> users = new List<string>();

            string response = network.AjaxHandlerRequest(string.Format("highscoreType=score&offset={1}&view=highscore&searchUser=&currentCityId={0}&templateView=highscore&ajax=1", currentCity.ID, ((page-1) * 50).ToString()), Relogin);

            MatchCollection players_regex = new Regex("\"\\?view=avatarProfile&avatarId=(.+?)\"").Matches(response.Replace(" ", "").Replace("\n", "").Replace("\r", ""));
            MatchCollection ally_regex = new Regex("\\?view=allyPage&oldView=highscore&allyId=([0-9]+?)\\\\\">([A-Za-z\\-\\\\]+?)<\\\\/a>").Matches(response.Replace(" ", "").Replace("\n", "").Replace("\r", ""));

            for (int i = 0; i < players_regex.Count; i++)
            {
                if (players_regex[i].Success)
                {
                    string playerId = players_regex[i].Groups[1].Value.Replace("\\", "");
                    users.Add(playerId);
                }
            }
            foreach (Match player in ally_regex)
            {
                if (player.Success)
                {
                    string id = player.Groups[1].Value.Replace("\\", "");
                }
            }

            return users;
        }

        public List<string> GetHighScoreTable()
        {
            return GetHighScoreTable(1);
        }

        public List<PiracyTopUser> GetPiracyTop()
        {
            List<PiracyTopUser> top = new List<PiracyTopUser>();

            string response = network.AjaxHandlerRequest(string.Format("view=pirateFortress&activeTab=tabBootyQuest&cityId={0}&position=17&backgroundView=city&currentCityId={0}&templateView=pirateFortress&ajax=1", currentCity.ID), Relogin);

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

                top.Add(new PiracyTopUser(result[1].Value, result[2].Value, result[3].Value, result[4].Value, name, result[7].Value, result[8].Value, result[9].Value, server));
            }

            return top;
        }

        public DateTime GetPiracyTimeEnd()
        {
            string response = network.AjaxHandlerRequest(string.Format("view=pirateFortress&activeTab=tabBootyQuest&cityId={0}&position=17&backgroundView=city&currentCityId={0}&templateView=pirateFortress&ajax=1", currentCity.ID), Relogin);

            Match endOfRotation = Regex.Match(response, "\\\"enddate\\\":(.+?),");
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            if (endOfRotation.Success)
            {
                dtDateTime = dtDateTime.AddSeconds(long.Parse(endOfRotation.Groups[1].Value)).ToLocalTime();
            }
            return dtDateTime;
        }

        public bool IsCrewOnGoingMission()
        {
            string response = network.AjaxHandlerRequest(string.Format("view=pirateFortress&activeTab=tabBootyQuest&cityId={0}&position=17&backgroundView=city&currentCityId={0}&templateView=pirateFortress&ajax=1", currentCity.ID), Relogin);

            return new Regex(",\\\\\"hasOngoingMission\\\\\":true,").Match(response).Success;
        }

        public void Research(RESEARCH research)
        {
            string[] r = new string[] { "seafaring", "economy", "knowledge", "military" };
            network.AjaxHandlerRequest(string.Format("action=Advisor&function=doResearch&type={1}&backgroundView=city&currentCityId={0}&templateView=researchAdvisor&ajax=1", currentCity.ID, r[(int)research]), Relogin);
        }

        public void SetCurrentCityName(string name)
        {
            network.AjaxHandlerRequest(
                string.Format("action=CityScreen&function=rename&cityId={0}&position=0&name={1}&backgroundView=city&currentCityId={0}&templateView=townHall&ajax=1", 
                    currentCity.ID,
                    Uri.EscapeDataString(name)
                ), Relogin
           );
        }

        public string GetOnGoingMissionType()
        {
            string mission = "", response = network.AjaxHandlerRequest(string.Format("view=pirateFortress&activeTab=tabBootyQuest&cityId={0}&position=17&backgroundView=city&currentCityId={0}&templateView=pirateFortress&ajax=1", currentCity.ID), Relogin);

            Match missionType = new Regex(",\\\\\"ongoingMissionType\\\\\":\\\\\"(.+?)\\\\\",").Match(response);
            if (missionType.Success)
                mission = missionType.Groups[1].Value;

            return mission;
        }

        public string GetFriendId(string inviteURL)
        {
            string friendId = Regex.Match(network.DownloadString(inviteURL).Replace(" ", ""), "name=\"friendId\"value=\"(.+?)\"").Groups[1].Value;

            if(null == friendId)
                return friendId;
            return string.Empty;
        }

        public static string GetFriendHash(string inviteURL)
        {
            return inviteURL.Split('=')[1];
        }

        public bool Register(string EMail, CaptchaSolver captchaSolver, string friendId="", string friendHash="")
        {
            EMail = EMail.Replace("+", "%2B");
            string response = network.DownloadString(url + string.Format("?action=newPlayer&function=createAvatar&fh={0}&friendId={3}&list=0&kid=&gclid=&password={1}&email={2}&agb=on", friendHash, password, EMail, friendId));

            while (response.Replace(" ", "").Contains("captchaForm"))
            {
                byte[] captchaData = network.DownloadData(url + "?action=NoUser&function=createCaptcha&rand=" + random.Next(0, 99999).ToString());
                captchaSolver.image = captchaData;

                string captchaID;
                bool success;
                string captchaValue = captchaSolver.SendCaptcha(out captchaID, out success);
                if (success && captchaValue != null)
                {
                    response = network.DownloadString(url + string.Format("?registrationCaptcha={0}&action=newPlayer&function=createAvatar&friend=0&checksum&password={1}&name={2}&email={3}&agb=on&kid&friendId={4}&list=0&ssoMode=0&oiUrl&fh={5}&state&altNames&errors[0]=39", captchaValue, password, userName, EMail, friendId, friendHash));
                    captchaSolver.CaptchaWorked(captchaID, !response.Replace(" ", "").Contains("captchaForm"));
                }
            }

            network = new Network(connection, url);
            bool result = Login(EMail);
            if (result)
            {
                response = network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&avatarName={0}&email={1}&agb=on&cityId={2}&signUp=1&backgroundView=city&currentCityId={2}&ajax=1", userName, EMail, currentCity.ID), Relogin);
            }
            return result;
        }

        public void DoTutorial(Action<string> callback)
        {
            Languages lang = Languages.Instance;

            //Mission 1 - set 10 to wood
            callback(lang.Get("WebView", "StartingMission") + " 1");
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=20&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=IslandScreen&function=workerPlan&screen=TownHall&type&cityId={0}&wood=10&luxury=0&scientists=0&priests=0&backgroundView=city&currentCityId={0}&templateView=townHall&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&templateView=townHall&ajax=1", currentCity.ID), Relogin);
            callback(lang.Get("WebView", "MissionCompleted"));

            //Mission 2 - build academy
            callback(lang.Get("WebView", "StartingMission") + " 2");
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=30&backgroundView=city&templateView=null&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=CityScreen&function=build&cityId={0}&position=9&building=4&backgroundView=city&currentCityId={0}&templateView=buildingGround&ajax=1", currentCity.ID), Relogin);
            Thread.Sleep((int)WAIT.ONE_MINUTE + random.Next(10000, 20000));
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            callback(lang.Get("WebView", "MissionCompleted"));

            //Mission 3 - research
            callback(lang.Get("WebView", "StartingMission") + " 3");
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=50&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=Advisor&function=doResearch&type=economy&backgroundView=city&currentCityId={0}&templateView=researchAdvisor&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&templateView=townHall&ajax=1", currentCity.ID), Relogin);
            callback(lang.Get("WebView", "MissionCompleted"));

            //Mission 4 - Set 12 scientists
            callback(lang.Get("WebView", "StartingMission") + " 4");
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=40&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=IslandScreen&function=workerPlan&screen=TownHall&type&cityId={0}&wood=10&luxury=0&scientists=12&priests=0&backgroundView=city&currentCityId={0}&templateView=townHall&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&templateView=townHall&ajax=1", currentCity.ID), Relogin);
            callback(lang.Get("WebView", "MissionCompleted"));

            //Mission 5 - Build warehouse
            callback(lang.Get("WebView", "StartingMission") + " 5");
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=60&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&templateView=townHall&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=CityScreen&function=build&cityId={0}&position=10&building=7&backgroundView=city&currentCityId={0}&templateView=buildingGround&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            Thread.Sleep((int)WAIT.ONE_MINUTE + random.Next(20000, 60000));
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            callback(lang.Get("WebView", "MissionCompleted"));

            //Mission 6 - build barracks
            callback(lang.Get("WebView", "StartingMission") + " 6");
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=70&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=CityScreen&function=build&cityId={0}&position=6&building=6&backgroundView=city&currentCityId={0}&templateView=buildingGround&ajax=1", currentCity.ID), Relogin);
            Thread.Sleep((int)WAIT.ONE_MINUTE + random.Next(20000, 60000));
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            callback(lang.Get("WebView", "MissionCompleted"));

            //Mission 7 - build 3 spearmen and wall
            callback(lang.Get("WebView", "StartingMission") + " 7");
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=80&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("301=0&302=0&303=0&304=0&305=0&306=0&307=0&308=0&309=0&310=0&311=0&312=0&313=0&315=3&action=CityScreen&function=buildUnits&cityId={0}&position=6&backgroundView=city&currentCityId={0}&templateView=barracks&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=CityScreen&function=build&cityId={0}&position=14&building=8&backgroundView=city&currentCityId={0}&templateView=buildingGround&ajax=1", currentCity.ID), Relogin);
            Thread.Sleep((int)WAIT.ONE_AND_HALF_MINUTE + random.Next(20000, 60000));
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&templateView=townHall&ajax=1", currentCity.ID), Relogin);
            callback(lang.Get("WebView", "MissionCompleted"));

            //Mission 8 - build port
            callback(lang.Get("WebView", "StartingMission") + " 8");
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=90&backgroundView=city&templateView=null&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=CityScreen&function=build&cityId={0}&position=1&building=3&backgroundView=city&currentCityId={0}&templateView=buildingGround&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=91&backgroundView=city&templateView=null&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            Thread.Sleep((int)WAIT.EIGHT_MINUTES + random.Next(30000, 60000));
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            callback(lang.Get("WebView", "MissionCompleted"));

            //Mission 9 - buy ship
            callback(lang.Get("WebView", "StartingMission") + " 9");
            Thread.Sleep((int)WAIT.ONE_AND_HALF_MINUTE + random.Next(30000, 60000));
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=100&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            while (true)
            {
                network.AjaxHandlerRequest(string.Format("view=port&cityId={0}&position=1&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
                network.AjaxHandlerRequest(string.Format("view=port&activeTab=tabBuyTransporter&cityId={0}&position=1&backgroundView=city&currentCityId={0}&templateView=port&ajax=1", currentCity.ID), Relogin);
                network.AjaxHandlerRequest(string.Format("action=CityScreen&function=increaseTransporter&cityId={0}&position=1&activeTab=tabBuyTransporter&backgroundView=city&currentCityId={0}&templateView=port&ajax=1", currentCity.ID), Relogin);
                GroupCollection transporter = Regex.Match(network.AjaxHandlerRequest(string.Format("view=updateGlobalData&oldBackgroundView=city&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin), "\"maxTransporters\":\"(\\d+?)\"").Groups;
                if (transporter.Count > 0 && !transporter[1].Value.Equals("") && "0" != transporter[1].Value)
                    break;
                Thread.Sleep(5000);
            }
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=rewardShown&position=1&activeTab=tabBuyTransporter&backgroundView=city&currentCityId={0}&templateView=port&ajax=1", currentCity.ID), Relogin);
            callback(lang.Get("WebView", "MissionCompleted"));

            //Mission 10 - upgrade warehouse
            callback(lang.Get("WebView", "StartingMission") + " 10");
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=110&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=CityScreen&function=upgradeBuilding&cityId={0}&position=10&level=2&backgroundView=city&currentCityId={0}&templateView=warehouse&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&templateView=townHall&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=111&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=city&currentCityId={0}&ajax=1", currentCity.ID), Relogin);
            callback(lang.Get("WebView", "MissionCompleted"));

            //Change to island view
            callback(lang.Get("Piracy", "IslandID") + ": " + currentCity.IslandId);

            //Mission 11 - Raid barbarians
            callback(lang.Get("WebView", "StartingMission") + " 11");
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=120&backgroundView=island&islandId={0}&currentIslandId={0}&ajax=1", currentCity.IslandId), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&backgroundView=island&currentIslandId={0}&ajax=1", currentCity.IslandId), Relogin);
            network.AjaxHandlerRequest(string.Format("view=barbarianVillage&destinationIslandId={0}&backgroundView=island&currentIslandId={0}&ajax=1", currentCity.IslandId), Relogin);
            network.AjaxHandlerRequest(string.Format("view=plunder&destinationCityId=0&barbarianVillage=1&islandId={0}&destinationIslandId={0}&backgroundView=island&currentIslandId={0}&templateView=barbarianVillage&ajax=1", currentCity.IslandId), Relogin);

            network.AjaxHandlerRequest(string.Format("action=transportOperations&function=attackBarbarianVillage&islandId={0}&destinationCityId=0&cargo_army_315_upkeep=1&cargo_army_315=3&transporter=2&barbarianVillage=1&backgroundView=island&currentIslandId={0}&templateView=plunder&ajax=1", currentCity.IslandId), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=rewardShown&destinationIslandId={0}&backgroundView=island&currentIslandId={0}&templateView=barbarianVillage&ajax=1", currentCity.IslandId), Relogin);
            callback(lang.Get("WebView", "MissionCompleted"));

            //Mission 12 - Ambrosia
            callback(lang.Get("WebView", "StartingMission") + " 12");
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=messageWasDisplayed&quest=130&backgroundView=island&templateView=barbarianVillage&destinationIslandId={0}&currentIslandId={0}&ajax=1", currentCity.IslandId), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=updateTutorialData&disableTutorial=false&destinationIslandId={0}&backgroundView=island&currentIslandId={0}&templateView=barbarianVillage&ajax=1", currentCity.IslandId), Relogin);
            network.AjaxHandlerRequest(string.Format("view=premium&linkType=1&destinationIslandId={0}&backgroundView=island&currentIslandId={0}&templateView=barbarianVillage&ajax=1", currentCity.IslandId), Relogin);
            network.AjaxHandlerRequest(string.Format("action=TutorialOperations&function=rewardShown&backgroundView=island&currentIslandId={0}&templateView=premium&ajax=1", currentCity.IslandId), Relogin);
            callback(lang.Get("WebView", "MissionCompleted"));

            //Move to city view
            network.AjaxHandlerRequest(string.Format("view=city"), Relogin);
            network.AjaxHandlerRequest(string.Format("action=IslandScreen&function=workerPlan&screen=TownHall&type&cityId={0}&wood=10&luxury=0&scientists=12&priests=0&backgroundView=city&currentCityId={0}&templateView=townHall&ajax=1", currentCity.ID), Relogin);

            callback(lang.Get("WebView", "Optinal"));
            //Wait until got all points
            Thread.Sleep((int)WAIT.THREE_HOURS);

            //Research
            network.AjaxHandlerRequest(string.Format("action=Advisor&function=doResearch&type=seafaring&backgroundView=city&currentCityId={0}&templateView=researchAdvisor&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=Advisor&function=doResearch&type=military&backgroundView=city&currentCityId={0}&templateView=researchAdvisor&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=Advisor&function=doResearch&type=seafaring&backgroundView=city&currentCityId={0}&templateView=researchAdvisor&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=Advisor&function=doResearch&type=seafaring&backgroundView=city&currentCityId={0}&templateView=researchAdvisor&ajax=1", currentCity.ID), Relogin);

            network.AjaxHandlerRequest(string.Format("action=CityScreen&function=build&cityId={0}&position=17&building=30&backgroundView=city&currentCityId={0}&templateView=buildingGround&ajax=1", currentCity.ID), Relogin);
            Thread.Sleep((int)WAIT.ONE_AND_HALF_MINUTE);
            network.AjaxHandlerRequest(string.Format("view=pirateFortress&cityId={0}&position=17&backgroundView=city&currentCityId={0}&currentTab=tabBootyQuest&ajax=1", currentCity.ID), Relogin);
            network.AjaxHandlerRequest(string.Format("action=Achievements&function=clearPopups&position=17&activeTab=tabBootyQuest&backgroundView=city&currentCityId={0}&templateView=pirateFortress&ajax=1", currentCity.ID), Relogin);
            callback(lang.Get("WebView", "PirateBuilt"));
        }

    }
}
