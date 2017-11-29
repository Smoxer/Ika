using System.Windows.Forms;
using System.Collections.Generic;
using Library;

namespace IkariamBots.Forms
{
    public partial class SelectUsers : Form
    {
        public string result;
        public bool repeat;

        private List<string> commands;
        private List<UserItem> allUsers;
        private List<string[]> allCities;
        private static Languages lang = Languages.Instance;
        private string userName, server_address;

        public SelectUsers(List<UserItem> allUsers, List<string[]> allCities, string placeHolder, string userName, string server_address)
        {
            InitializeComponent();

            #region Translate
            Text = lang.Get("Piracy", "RaidList");
            label1.Text = lang.Get("Misc", "AllUsers");
            label2.Text = lang.Get("Piracy", "RaidList");
            label3.Text = lang.Get("Piracy", "ChangeCurrentCity");
            repeatRaid.Text = lang.Get("Misc", "Repeat");
            OK.Text = lang.Get("Misc", "OK");
            Cancel.Text = lang.Get("Misc", "Cancel");
            Clear.Text = lang.Get("Misc", "Clear");
            #endregion

            this.allUsers = allUsers;
            this.allCities = allCities;
            this.userName = userName;
            this.server_address = server_address;
            commands = new List<string>();
            result = "";

            users.Scrollable = true;
            users.View = View.Details;
            users.Columns.Add(lang.Get("Misc", "Username"));
            users.Columns.Add(lang.Get("Misc", "Server"));
            users.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            cities.Scrollable = true;
            cities.View = View.Details;
            cities.Columns.Add(lang.Get("Piracy", "CityName"));
            cities.Columns.Add(lang.Get("WebView", "Location"));
            cities.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            raidList.Scrollable = true;
            raidList.View = View.Details;
            raidList.Columns.Add(lang.Get("Misc", "Action"));
            raidList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

            foreach (UserItem user in allUsers)
            {
                users.Items.Add(new ListViewItem(new string[] { user.userName, user.server }));
            }
            users.Items.Add(new ListViewItem(new string[] { lang.Get("Misc", "Custom"), lang.Get("Misc", "Custom") }));

            foreach (string[] city in allCities)
            {
                if(city[3].Equals(lang.Get("Misc", "Yes")))
                    cities.Items.Add(new ListViewItem(new string[] { city[1], city[2] }));
            }

            foreach(string command in placeHolder.Split(','))
            {
                commands.Add(command);
                if (2 == command.Split('|').Length)
                {
                    raidList.Items.Add(lang.Get("Piracy", "Raid") + " " + command.Split('|')[0]);
                }
                else if (1 == command.Split('|').Length)
                {
                    raidList.Items.Add(lang.Get("Piracy", "ChangeCurrentCity") + " " + command);
                }
            }
        }

        private void users_ItemActivate(object sender, System.EventArgs e)
        {
            if (users.SelectedItems[0].Index == users.Items.Count - 1)
            {
                using (var form = new CityAndIslandID())
                {
                    var result = form.ShowDialog();
                    if (DialogResult.OK == result)
                    {
                        allUsers.Add(new UserItem(lang.Get("Misc", "Custom"), lang.Get("Misc", "Custom"), form.cityID, form.islandID));
                    }
                    else
                        return;
                }
                
            }
            UserItem selected = allUsers[users.SelectedItems[0].Index];

            raidList.Items.Add(lang.Get("Piracy", "Raid") + " " + selected.userName + " (" + selected.server + ")");
            commands.Add(selected.cityID + "|" + selected.islandID);
        }

        private void cities_ItemActivate(object sender, System.EventArgs e)
        {
            int index = 0;
            foreach (string[] city in allCities)
            {
                if (city[3].Equals(lang.Get("Misc", "Yes")))
                {
                    if(index == cities.SelectedItems[0].Index)
                    {
                        raidList.Items.Add(lang.Get("Piracy", "ChangeCurrentCity") + " " + city[0] + " (" + city[1] + ")");
                        commands.Add(city[0]);
                        break;
                    }
                    index++;
                }
                    
            }
            /*string[] city = allCities[cities.SelectedItems[0].Index];
            raidList.Items.Add(lang.Get("Piracy", "ChangeCurrentCity") + " " + city[0] + " (" + city[1] + ")");
            commands.Add(city[0]);*/
        }

        private void raidList_ItemActivate(object sender, System.EventArgs e)
        {
            commands.RemoveAt(raidList.SelectedItems[0].Index);
            raidList.Items.RemoveAt(raidList.SelectedItems[0].Index);
        }

        private void OK_Click(object sender, System.EventArgs e)
        {
            result = string.Join(",", commands);
            repeat = repeatRaid.Checked;
            Settings.Instance.Set("RaidsPlaceHolders" + userName + server_address.Replace("-", ""), result);
            Settings.Instance.Commit();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Cancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Clear_Click(object sender, System.EventArgs e)
        {
            raidList.Items.Clear();
            commands.Clear();
        }
    }

    public class UserItem
    {
        public string userName, server, cityID, islandID;

        public UserItem(string userName, string server, string cityID, string islandID)
        {
            this.userName = userName;
            this.server = server;
            this.cityID = cityID;
            this.islandID = islandID;
        }
    }
}
