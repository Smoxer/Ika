using CefSharp;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace IkariamBots
{
    public class ScriptLanguage
    {
        private int lineIndex = 0;

        public ScriptLanguage()
        {

        }

        public void RunScript(object script)
        {
            string[] lines = script.ToString().Split('\n')
           .Where(x => !string.IsNullOrEmpty(x))
           .ToArray();
            MainWindow.browser.ExecuteScriptAsync("$('body').append('<div style=\\\'width: 100%; height: 100%; position: fixed; background: rgba(0,0,0,0.5); z-index: 999999; text-align: center; color: #FFFFFF; direction: ltr;\\\' id=\\\'canceluserInput\\\'>A script is running<br /><a href=\\\'javascript: void(0);\\\' onclick=\\\'callbackObj.stopScriptLanguage(); $(\\\\\'#canceluserInput\\\\\\').hide();\\\'>Abort script</a></div>');");
            MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />Log: ');");
            while (lineIndex < lines.Length && MainWindow.isScriptLanguageCanRun)
            {
                if (lineIndex >= lines.Length)
                {
                    break;
                }
                string command = lines[lineIndex].Trim();
                if (lines[lineIndex].Length == 0 || command[0].Equals("#") || command[0].Equals(";"))
                    continue;
                string option = command.Split(' ')[0];
                if (option.Equals("Set", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (command.Split(' ').Length != 3)
                    {
                        MessageBox.Show("Error in file \"ManualFight.ikr\" line " + (lineIndex).ToString() + "Unknown command format\nPlease use:\nSet *UnitName* *HowMuch*", "Error");
                        lineIndex = lines.Length + 2;
                        continue;
                    }
                    string unit = command.Split(' ')[1];
                    int value;
                    bool result = Int32.TryParse(command.Split(' ')[2], out value);
                    if (!result)
                    {
                        MessageBox.Show("Error in file \"ManualFight.ikr\" line " + (lineIndex).ToString() + "Unknown command format\nPlease use:\nSet *UnitName* *HowMuch*", "Error");
                        lineIndex = lines.Length + 2;
                        continue;
                    }
                    switch (unit.ToLower())
                    {
                        case "hoplite":
                            MainWindow.Hoplite = value;
                            MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Hoplite = " + value + "');");
                            break;
                        case "steamgiant":
                            MainWindow.SteamGiant = value;
                            MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Steam giant = " + value + "');");
                            break;
                        case "spearman":
                            MainWindow.Spearman = value;
                            MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Spearman = " + value + "');");
                            break;
                        case "slinger":
                            MainWindow.Slinger = value;
                            MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Slinger = " + value + "');");
                            break;
                        case "swordman":
                            MainWindow.Swordsman = value;
                            MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Swordsman = " + value + "');");
                            break;
                        case "archer":
                            MainWindow.Archer = value;
                            MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Archer = " + value + "');");
                            break;
                        case "sulphurcarabineer":
                            MainWindow.SulphurCarabineer = value;
                            MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Sulphur carabineer = " + value + "');");
                            break;
                        case "ram":
                            MainWindow.Ram = value;
                            MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Ram = " + value + "');");
                            break;
                        case "catapult":
                            MainWindow.Catapult = value;
                            MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Catapult = " + value + "');");
                            break;
                        case "mortar":
                            MainWindow.Mortar = value;
                            MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Mortar = " + value + "');");
                            break;
                        case "gyrocopter":
                            MainWindow.Gyrocopter = value;
                            MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Gyrocopter = " + value + "');");
                            break;
                        case "balloonbombardier":
                            MainWindow.BalloonBombardier = value;
                            MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Balloon bombardier = " + value + "');");
                            break;
                        case "cook":
                            MainWindow.Cook = value;
                            MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Cook = " + value + "');");
                            break;
                        case "doctor":
                            MainWindow.Doctor = value;
                            MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Doctor = " + value + "');");
                            break;
                        case "transporterships":
                            MainWindow.TransporterShips = value;
                            MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Transporter ships = " + value + "');");
                            break;
                    }
                }
                else if (option.Equals("ResetUnits", StringComparison.InvariantCultureIgnoreCase))
                {
                    MainWindow.Hoplite = 0;
                    MainWindow.SteamGiant = 0;
                    MainWindow.Spearman = 0;
                    MainWindow.Slinger = 0;
                    MainWindow.Swordsman = 0;
                    MainWindow.Archer = 0;
                    MainWindow.SulphurCarabineer = 0;
                    MainWindow.Ram = 0;
                    MainWindow.Catapult = 0;
                    MainWindow.Mortar = 0;
                    MainWindow.Gyrocopter = 0;
                    MainWindow.BalloonBombardier = 0;
                    MainWindow.Cook = 0;
                    MainWindow.Doctor = 0;
                    MainWindow.TransporterShips = 0;
                    MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* All units are now set to 0');");
                }
                else if (option.Equals("RefreshData", StringComparison.InvariantCultureIgnoreCase))
                {
                    MainWindow.browser.ExecuteScriptAsync("ajaxHandlerCall('?view=updateGlobalData');");
                    MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Data refreshed');");
                    Thread.Sleep(4000);
                }
                else if (option.Equals("JS", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (command.Split(' ').Length != 2)
                    {
                        MessageBox.Show("Error in file \"ManualFight.ikr\" line " + (lineIndex).ToString() + "Unknown command format\nPlease use:\nJS *Command*", "Error");
                        lineIndex = lines.Length + 2;
                        continue;
                    }
                    MainWindow.browser.ExecuteScriptAsync(command.Split(' ')[1]);
                    MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Run JavaScript command');");
                }
                else if (option.Equals("ChangeView", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (command.Split(' ').Length != 2)
                    {
                        MessageBox.Show("Error in file \"ManualFight.ikr\" line " + (lineIndex).ToString() + "Unknown command format\nPlease use:\nChangeView *View*", "Error");
                        lineIndex = lines.Length + 2;
                        continue;
                    }
                    if(command.Split(' ')[1].Equals("Island", StringComparison.InvariantCultureIgnoreCase))
                    {
                        MainWindow.browser.ExecuteScriptAsync("window.location = 'index.php?view=island';");
                        MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Change view to: Island');");
                    }
                    else if (command.Split(' ')[1].Equals("City", StringComparison.InvariantCultureIgnoreCase))
                    {
                        MainWindow.browser.ExecuteScriptAsync("window.location = 'index.php?view=city';");
                        MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Change view to: City');");
                    }
                    else
                    {
                        MainWindow.browser.ExecuteScriptAsync("window.location = 'index.php?view=worldmap_iso';");
                        MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Change view to: World');");
                    }
                    
                    Thread.Sleep(15000);
                }
                else if (option.Equals("StartPiracy", StringComparison.InvariantCultureIgnoreCase))
                {
                    MainWindow.browser.ExecuteScriptAsync("timer_is_on = true; startPiracy();");
                }
                else if (option.Equals("StopPiracy", StringComparison.InvariantCultureIgnoreCase))
                {
                    MainWindow.browser.ExecuteScriptAsync("timer_is_on = false;");
                }
                else if (option.Equals("Wait", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (command.Split(' ').Length != 2)
                    {
                        MessageBox.Show("Error in file \"ManualFight.ikr\" line " + (lineIndex).ToString() + "Unknown command format\nPlease use:\nWait *Minutes*", "Error");
                        lineIndex = lines.Length + 2;
                        continue;
                    }
                    int value;
                    bool result = Int32.TryParse(command.Split(' ')[1], out value);
                    if (!result)
                    {
                        MessageBox.Show("Error in file \"ManualFight.ikr\" line " + (lineIndex).ToString() + "Unknown command format\nPlease use:\nWait *Minutes*", "Error");
                        lineIndex = lines.Length + 2;
                        continue;
                    }
                    MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Wait " + value + " minutes');");
                    Thread.Sleep(value * 60000);
                }
                else if (option.Equals("UpgradeBuilding", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (command.Split(' ').Length != 2)
                    {
                        MessageBox.Show("Error in file \"ManualFight.ikr\" line " + (lineIndex).ToString() + "Unknown command format\nPlease use:\nUpgradeBuilding *PositionID*", "Error");
                        lineIndex = lines.Length + 2;
                        continue;
                    }
                    int value;
                    bool result = Int32.TryParse(command.Split(' ')[1], out value);
                    if (!result)
                    {
                        MessageBox.Show("Error in file \"ManualFight.ikr\" line " + (lineIndex).ToString() + "Unknown command format\nPlease use:\nUpgradeBuilding *PositionID*", "Error");
                        lineIndex = lines.Length + 2;
                        continue;
                    }
                    MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* upgrade at position " + value + "');");
                    MainWindow.browser.ExecuteScriptAsync("$('#js_CityPosition" + value.ToString() + "Link').click();");
                    Thread.Sleep(4000);
                    MainWindow.browser.ExecuteScriptAsync("$('#js_buildingUpgradeButton').click();");
                }
                else if (option.Equals("ChangeCityID", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (command.Split(' ').Length != 2)
                    {
                        MessageBox.Show("Error in file \"ManualFight.ikr\" line " + (lineIndex).ToString() + "Unknown command format\nPlease use:\nChangeCityID *CityID*", "Error");
                        lineIndex = lines.Length + 2;
                        continue;
                    }
                    int value;
                    bool result = Int32.TryParse(command.Split(' ')[1], out value);
                    if (!result)
                    {
                        MessageBox.Show("Error in file \"ManualFight.ikr\" line " + (lineIndex).ToString() + "Unknown command format\nPlease use:\nChangeCityID *CityID*", "Error");
                        lineIndex = lines.Length + 2;
                        continue;
                    }
                    MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* CityID =  " + value + "');");
                    MainWindow.cityID = value.ToString();
                }
                else if (option.Equals("ChangeCurrentCityID", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (command.Split(' ').Length != 2)
                    {
                        MessageBox.Show("Error in file \"ManualFight.ikr\" line " + (lineIndex).ToString() + "Unknown command format\nPlease use:\nChangeCurrentCityID *CityID*", "Error");
                        lineIndex = lines.Length + 2;
                        continue;
                    }
                    int value;
                    bool result = Int32.TryParse(command.Split(' ')[1], out value);
                    if (!result)
                    {
                        MessageBox.Show("Error in file \"ManualFight.ikr\" line " + (lineIndex).ToString() + "Unknown command format\nPlease use:\nChangeCurrentCityID *CityID*", "Error");
                        lineIndex = lines.Length + 2;
                        continue;
                    }
                    MainWindow.browser.ExecuteScriptAsync("if ($(\"#dropDown_js_citySelectContainer\").css(\"display\") == \"none\"){ $(\"#js_citySelectContainer span\").click(); setTimeout(function() { $(\"#dropDown_js_citySelectContainer li[selectvalue='" + value + "']\").click(); }, 500); } else $(\"#dropDown_js_citySelectContainer li[selectvalue='" + value + "']\").click();");
                    MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* CityID =  " + value + "');");
                    Thread.Sleep(2000);
                }
                else if (option.Equals("GoTo", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (command.Split(' ').Length != 2)
                    {
                        MessageBox.Show("Error in file \"ManualFight.ikr\" line " + (lineIndex).ToString() + "Unknown command format\nPlease use:\nGoTo *Line*", "Error");
                        lineIndex = lines.Length + 2;
                        continue;
                    }
                    int value;
                    bool result = Int32.TryParse(command.Split(' ')[1], out value);
                    if (!result)
                    {
                        MessageBox.Show("Error in file \"ManualFight.ikr\" line " + (lineIndex).ToString() + "Unknown command format\nPlease use:\nGoTo *Line*", "Error");
                        lineIndex = lines.Length + 2;
                        continue;
                    }
                    if (lineIndex >= lines.Length)
                        value = lines.Length;
                    lineIndex = value - 1;
                    MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Go to line #" + value + "');");
                }
                else if (option.Equals("PiracyRaid", StringComparison.InvariantCultureIgnoreCase))
                {
                    MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* Piracy raid');");
                    MainWindow.browser.ExecuteScriptAsync("ajaxHandlerCall(\"index.php?view=piracyRaid&isMission=1&destinationCityId=" + MainWindow.cityID + "\");");
                    Thread.Sleep(4000);
                    MainWindow.browser.ExecuteScriptAsync("$(\"#actionLink\").click();");
                }
                else if (option.Equals("Raid", StringComparison.InvariantCultureIgnoreCase) || option.Equals("DeployUnits", StringComparison.InvariantCultureIgnoreCase) || option.Equals("Occupy", StringComparison.InvariantCultureIgnoreCase))
                {
                    MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").append('<br />* " + option + "');");
                    if (option.Equals("Raid", StringComparison.InvariantCultureIgnoreCase))
                        MainWindow.browser.ExecuteScriptAsync("ajaxHandlerCall(\"index.php?view=plunder&isMission=1&destinationCityId=" + MainWindow.cityID + "\");");
                    else if (option.Equals("DeployUnits", StringComparison.InvariantCultureIgnoreCase))
                        MainWindow.browser.ExecuteScriptAsync("ajaxHandlerCall(\"index.php?view=deployment&isMission=1&deploymentType=army&destinationCityId=" + MainWindow.cityID + "\");");
                    else if (option.Equals("Occupy", StringComparison.InvariantCultureIgnoreCase))
                        MainWindow.browser.ExecuteScriptAsync("ajaxHandlerCall(\"index.php?view=occupy&isMission=1&destinationCityId=" + MainWindow.cityID + "\");");
                    Thread.Sleep(4000);
                    MainWindow.browser.ExecuteScriptAsync("Hoplite = " + MainWindow.Hoplite.ToString() + "; SteamGiant = " + MainWindow.SteamGiant.ToString() + "; Spearman = " + MainWindow.Spearman.ToString() + "; Slinger = " + MainWindow.Slinger.ToString() + "; Swordsman = " + MainWindow.Swordsman.ToString() + "; Archer = " + MainWindow.Archer.ToString() + "; SulphurCarabineer = " + MainWindow.SulphurCarabineer.ToString() + "; Ram = " + MainWindow.Ram.ToString() + "; Catapult = " + MainWindow.Catapult.ToString() + "; Mortar = " + MainWindow.Mortar.ToString() + "; Gyrocopter = " + MainWindow.Gyrocopter.ToString() + "; BalloonBombardier = " + MainWindow.BalloonBombardier.ToString() + "; Cook = " + MainWindow.Cook.ToString() + "; Doctor = " + MainWindow.Doctor.ToString() + "; TransporterShips = " + MainWindow.TransporterShips.ToString() + ";");
                    string totalArmy = "$(\"#cargo_army_301\").val(Slinger);\n";
                    totalArmy += "$(\"#cargo_army_302\").val(Swordsman);\n";
                    totalArmy += "$(\"#cargo_army_303\").val(Hoplite);\n";
                    totalArmy += "$(\"#cargo_army_304\").val(SulphurCarabineer);\n";
                    totalArmy += "$(\"#cargo_army_305\").val(Mortar);\n";
                    totalArmy += "$(\"#cargo_army_306\").val(Catapult);\n";
                    totalArmy += "$(\"#cargo_army_307\").val(Ram);\n";
                    totalArmy += "$(\"#cargo_army_308\").val(SteamGiant);\n";
                    totalArmy += "$(\"#cargo_army_309\").val(BalloonBombardier);\n";
                    totalArmy += "$(\"#cargo_army_310\").val(Cook);\n";
                    totalArmy += "$(\"#cargo_army_311\").val(Doctor);\n";
                    totalArmy += "$(\"#cargo_army_312\").val(Gyrocopter);\n";
                    totalArmy += "$(\"#cargo_army_313\").val(Archer);\n";
                    totalArmy += "$(\"#cargo_army_315\").val(Spearman);\n";
                    totalArmy += "$(\"#cargo_army_301\").click();\n";
                    totalArmy += "$(\"#cargo_army_302\").click();\n";
                    totalArmy += "$(\"#cargo_army_303\").click();\n";
                    totalArmy += "$(\"#cargo_army_304\").click();\n";
                    totalArmy += "$(\"#cargo_army_305\").click();\n";
                    totalArmy += "$(\"#cargo_army_306\").click();\n";
                    totalArmy += "$(\"#cargo_army_307\").click();\n";
                    totalArmy += "$(\"#cargo_army_308\").click();\n";
                    totalArmy += "$(\"#cargo_army_309\").click();\n";
                    totalArmy += "$(\"#cargo_army_310\").click();\n";
                    totalArmy += "$(\"#cargo_army_311\").click();\n";
                    totalArmy += "$(\"#cargo_army_312\").click();\n";
                    totalArmy += "$(\"#cargo_army_313\").click();\n";
                    totalArmy += "$(\"#cargo_army_314\").click();\n";
                    totalArmy += "$(\"#cargo_army_315\").click();\n";
                    if (option.Equals("Raid", StringComparison.InvariantCultureIgnoreCase))
                    {
                        totalArmy += "$(\"#plusminus #extraTransporter\").val(TransporterShips);\n";
                        totalArmy += "$(\"#plusminus #extraTransporter\").click();\n";
                        totalArmy += "$(\"#plunderForm\").submit();\n";
                    }
                    else if (option.Equals("DeployUnits", StringComparison.InvariantCultureIgnoreCase))
                    {
                        totalArmy += "$(\"#armyDeploymentForm\").submit();\n";
                    }
                    else if (option.Equals("Occupy", StringComparison.InvariantCultureIgnoreCase))
                    {
                        totalArmy += "$(\"#plunderForm\").submit();\n";
                    }
                    MainWindow.browser.ExecuteScriptAsync(totalArmy);
                }
                lineIndex++;
            }
            MainWindow.browser.ExecuteScriptAsync("$(\"#canceluserInput\").remove();");
        }
    }
}
