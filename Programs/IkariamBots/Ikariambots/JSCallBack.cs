using System;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Threading;
using IkariamBots.Forms;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

namespace IkariamBots
{
    class JSCallBack
    {
        private Thread _ScriptLanguage;
        private ScriptEditor _ScriptEditor;

        public void WriteImage(string response, string type, string alert = "true")
        {
            byte[] bytes = Convert.FromBase64String(response);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            string imageLocation = String.Format("Reports/{0}/{1}.png", type, DateTime.Now.ToString("ddMMyyHHmmss"));
            image.Save(imageLocation);

            if (!alert.Equals("false"))
            {
                Alert("\"" + imageLocation + "\" report saved", "Report saved");
            }
        }

        public void Alert(string text, string title)
        {
            MessageBox.Show(text, title + " - " + Config.PROGRAM_NAME);
        }

        public void HardAlert(string text)
        {
            System.Media.SystemSounds.Asterisk.Play();
            MessageBox.Show(text, "Alert! - " + Config.PROGRAM_NAME);
        }

        /*public void AddCity(string cityID, string cityName)
        {
            if (!cityID.Equals("null") && !cityName.Equals("null"))
            {
                XmlNode cityElement = MainWindow.XMLConfig.CreateElement("City");
                XmlNode cityIDElement = MainWindow.XMLConfig.CreateElement("CityID");
                XmlNode cityNameElement = MainWindow.XMLConfig.CreateElement("CityName");

                cityIDElement.InnerText = cityID;
                cityElement.AppendChild(cityIDElement);

                cityNameElement.InnerText = cityName;
                cityElement.AppendChild(cityNameElement);

                MainWindow.XMLConfig.DocumentElement.SelectSingleNode("/Bot/FastCities").AppendChild(cityElement);
                MainWindow.XMLConfig.Save("config.xml");

                MessageBox.Show("Location saved", "Alert");
            }
        }

        public void PirateList(string cityID, string cityName)
        {
            if (!cityID.Equals("null") && !cityName.Equals("null"))
            {
                if(MainWindow.XMLConfig.DocumentElement.SelectSingleNode("/Bot/PiracyList") == null)
                {
                    XmlNode piracyList = MainWindow.XMLConfig.CreateElement("PiracyList");
                    MainWindow.XMLConfig.DocumentElement.SelectSingleNode("/Bot").AppendChild(piracyList);
                }
                XmlNode cityElement = MainWindow.XMLConfig.CreateElement("City");
                XmlNode cityIDElement = MainWindow.XMLConfig.CreateElement("CityID");
                XmlNode cityNameElement = MainWindow.XMLConfig.CreateElement("CityName");

                cityIDElement.InnerText = cityID;
                cityElement.AppendChild(cityIDElement);

                cityNameElement.InnerText = cityName;
                cityElement.AppendChild(cityNameElement);

                MainWindow.XMLConfig.DocumentElement.SelectSingleNode("/Bot/PiracyList").AppendChild(cityElement);
                MainWindow.XMLConfig.Save("config.xml");

                MessageBox.Show("Location saved", "Alert");
            }
        }*/

        public void StopScriptLanguage()
        {
            MainWindow.isScriptLanguageCanRun = false;
            _ScriptLanguage.Abort();
        }

        public void ScriptedAttacked(string cityIDFromJS)
        {
            MainWindow.FileFight = "AttackedFight.ikr";
            MainWindow.cityID = cityIDFromJS;

            _ScriptLanguage = new Thread(MainWindow.scriptLanguage.RunScript);
            MainWindow.isScriptLanguageCanRun = true;
            _ScriptLanguage.Start(File.ReadAllText(MainWindow.FileFight));
        }

        public void SendMail()
        {
            string command = "https://github.com/Smoxer/";
            Process.Start(command);
        }

        public void OpenExplorer(string location)
        {
            Process.Start("explorer.exe", location.Replace("/", "\\"));
        }

        public void QuickScript(string cityID)
        {
            MainWindow.ActiveForm.BeginInvoke((Action)delegate {
                if (_ScriptEditor == null || _ScriptEditor.IsDisposed)
                    _ScriptEditor = new ScriptEditor("ChangeCityID " + cityID);
                else
                    _ScriptEditor.AddText("ChangeCityID " + cityID);

                _ScriptEditor.Show();
                _ScriptEditor.BringToFront();
            });
        }

        public void AddToQueue(string cityID, string position)
        {
            if (!MainWindow.buildQueue.ContainsKey(cityID))
            {
                List<string> list = new List<string>();
                list.Add(position);
                MainWindow.buildQueue.Add(cityID, list);
            }
            else
            {
                MainWindow.buildQueue[cityID].Add(position);
            }
        }

        public void SaveIsland(string islandID, string data)
        {
            if (!Directory.Exists("Reports/Islands" + MainWindow.server_address + "/"))
            {
                Directory.CreateDirectory("Reports/Islands" + MainWindow.server_address + "/");
            }

            File.WriteAllText("Reports/Islands" + MainWindow.server_address + "/" + islandID + ".js", data);
        }

        public void ResetQueue()
        {
            MainWindow.buildQueue.Clear();
        }

        public void RemoveFromQueue(string cityID)
        {
            MainWindow.buildQueue[cityID].RemoveAt(0);
        }

        public void Exit()
        {
            MainWindow.CloseProgram();
        }

        public void Log(string msg)
        {
            IkariamBots.Log.Instance.Info(MainWindow.UserName, msg);
        }
    }
}
