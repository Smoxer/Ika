using System;
using CefSharp;
using System.IO;
using System.Globalization;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Library;

namespace IkariamBots.Forms
{
    public partial class IslandsByTime : Form
    {
        private double gov, poseidonLevel, tritonLevel, draft;
        private int unitSpeed;
        private JObject serverLocations;
        private static Languages lang = Languages.Instance;

        private void result_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = result.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches && null != serverLocations)
            {
                for (int x = 0; x <= 100; x++)
                {
                    for (int y = 0; y <= 100; y++)
                    {
                        if(string.Format("({0}, {1})", x, y).Equals(result.Items[index].ToString()) && null != serverLocations["data"][x.ToString()] && null != serverLocations["data"][x.ToString()][y.ToString()])
                        {
                            MainWindow.browser.ExecuteScriptAsync("location = 'index.php?view=island&islandId=" + serverLocations["data"][x.ToString()][y.ToString()][0].ToString() + "';");
                        }
                    }
                }
            }
            
        }

        public IslandsByTime()
        {
            InitializeComponent();

            #region Translate
            Text = lang.Get("WebView", "IslandsByTime");
            label1.Text = lang.Get("WebView", "Departure");
            label2.Text = lang.Get("WebView", "Arrival");
            label6.Text = lang.Get("WebView", "Oligarchy");
            Oligarchy.Text = lang.Get("WebView", "Oligarchy");
            label5.Text = lang.Get("WebView", "UnitSpeed");
            label4.Text = lang.Get("WebView", "Poseidon");
            label3.Text = lang.Get("WebView", "Cargo");
            label7.Text = lang.Get("WebView", "Triton");
            label8.Text = lang.Get("WebView", "Location");
            #endregion

            if (!File.Exists("Reports/Islands" + MainWindow.server_address + "/0.js"))
            {
                MainWindow.browser.ExecuteScriptAsync("$.ajax({ url: 'index.php',  async: true, cache: false, type: 'get', data:  { action: 'WorldMap', 'function': 'getJSONArea', x_min: -100, x_max: 100, y_min: -100, y_max: 100}, dataType: 'text', success: function(data, textStatus, request){ callbackObj.saveIsland('0', data); } });");
                serverLocations = null;
            }
            else
            {
                serverLocations = JObject.Parse(File.ReadAllText("Reports/Islands" + MainWindow.server_address + "/0.js"));
            }
        }

        private void recalculate(object sender, EventArgs e)
        {
            result.Items.Clear();
            result.Items.Add(lang.Get("WebView", "DoubleClick"));

            gov = 1.0;
            if (Oligarchy.Checked)
                gov += 0.1;

            tritonLevel = decimal.ToInt32(triton.Value) / 100;
            unitSpeed = decimal.ToInt32(speed.Value);

            poseidonLevel = 0;
            switch (decimal.ToInt32(poseidon.Value))
            {
                case 1:
                    poseidonLevel = 0.1;
                    break;
                case 2:
                    poseidonLevel = 0.3;
                    break;
                case 3:
                    poseidonLevel = 0.5;
                    break;
                case 4:
                    poseidonLevel = 0.7;
                    break;
                case 5:
                    poseidonLevel = 1.0;
                    break;
            }

            draft = 0;
            if (decimal.ToInt32(cargo.Value) / 100 > 0 && decimal.ToInt32(cargo.Value) / 100 <= 5)
            {
                switch (decimal.ToInt32(cargo.Value))
                {
                    case 100:
                        draft = 66.7;
                        break;
                    case 200:
                        draft = 50;
                        break;
                    case 300:
                        draft = 33.3;
                        break;
                    case 400:
                        draft = 16.7;
                        break;
                }
            }

            try
            {
                double seconds = Convert.ToDateTime(inTime.Text, new CultureInfo("de-DE")).Subtract(Convert.ToDateTime(outTime.Text, new CultureInfo("de-DE"))).TotalSeconds;
                for (int x = 0; x <= 100; x++)
                {
                    for (int y = 0; y <= 100; y++)
                    {
                        if (seconds == DistanceByTime(decimal.ToInt32(X.Value), x, decimal.ToInt32(Y.Value), y) && null != serverLocations["data"][x.ToString()] && null != serverLocations["data"][x.ToString()][y.ToString()])
                        {
                            result.Items.Add(string.Format("({0}, {1})", x, y));
                            if (result.Items.Count > 50)
                                break;
                        }
                    }
                    if (result.Items.Count > 50)
                        break;
                }
            }
            catch (Exception ex) { }
        }


        private double DistanceByTime(int x1, int x2, int y1, int y2)
        {
            return Math.Ceiling((Math.Sqrt(Math.Pow(x1-x2, 2) + Math.Pow(y1-y2, 2)) * 72000) / ((unitSpeed + (unitSpeed * 0) * gov) * (1 + (poseidonLevel + draft) + tritonLevel))); // Minutes
        }
    }
}
