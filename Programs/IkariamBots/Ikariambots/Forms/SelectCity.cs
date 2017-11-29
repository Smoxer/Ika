using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using Library;

namespace IkariamBots.Forms
{
    public partial class SelectCity : Form
    {
        private List<string[]> cities;
        public string cityID;
        private static Languages lang = Languages.Instance;

        public SelectCity(List<string[]> cities)
        {
            InitializeComponent();

            #region Translate
            Text = lang.Get("Piracy", "SelectCity");
            #endregion

            this.cities = cities;
        }

        private void SelectCity_Load(object sender, EventArgs e)
        {
            citiesList.Scrollable = true;
            citiesList.View = View.Details;
            citiesList.Columns.Add(lang.Get("Piracy", "CityID"), 100);
            citiesList.Columns.Add(lang.Get("Piracy", "CityName"), 100);
            citiesList.Columns.Add(lang.Get("WebView", "Location"), 100);
            citiesList.Columns.Add(lang.Get("Piracy", "HasPirate"), 100);

            foreach (string[] city in cities)
            {
                citiesList.Items.Add(new ListViewItem(city));
            }
        }

        private void citiesList_ItemActivate(object sender, EventArgs e)
        {
            cityID = citiesList.SelectedItems[0].Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void citiesList_MouseClick(object sender, MouseEventArgs e)
        {
            bool match = false;

            if (MouseButtons.Right == e.Button)
            {
                foreach (ListViewItem item in citiesList.Items)
                {

                    if (item.Bounds.Contains(new Point(e.X, e.Y)))
                    {
                        ToolStripLabel title = new ToolStripLabel(item.SubItems[1].Text);
                        title.Enabled = false;

                        ToolStripItem[] menuItems = null;

                        if (item.SubItems[3].Text.Equals(lang.Get("Misc", "No")))
                        {
                            ToolStripMenuItem build = new ToolStripMenuItem(lang.Get("Piracy", "BuildPirate"));
                            build.Click += BuildPirate;

                            menuItems = new ToolStripItem[] { title, new ToolStripSeparator(), build };
                        }
                        else
                        {
                            ToolStripMenuItem upgrade = new ToolStripMenuItem(lang.Get("Piracy", "UpgradePirate"));
                            upgrade.Click += UpgradePirate;

                            menuItems = new ToolStripItem[] { title, new ToolStripSeparator(), upgrade };
                        }

                        ContextMenuStrip menu = new ContextMenuStrip();
                        menu.Items.AddRange(menuItems);
                        citiesList.ContextMenuStrip = menu;
                        match = true;

                        break;
                    }
                }
                if (match)
                {
                    citiesList.ContextMenuStrip.Show(citiesList, new Point(e.X, e.Y));
                }
            }
        }

        private void UpgradePirate(object sender, EventArgs e)
        {
            cityID = citiesList.SelectedItems[0].Text;
            DialogResult = DialogResult.Retry;
            Close();
        }

        private void BuildPirate(object sender, EventArgs e)
        {
            cityID = citiesList.SelectedItems[0].Text;
            DialogResult = DialogResult.No;
            Close();
        }
    }
}
