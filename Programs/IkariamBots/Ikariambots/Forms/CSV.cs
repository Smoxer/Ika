using System.Windows.Forms;

namespace IkariamBots.Forms
{
    public partial class CSV : Form
    {
        public CSV()
        {
            InitializeComponent();

            OpenFileDialog file = new OpenFileDialog();
            file.Filter = Config.PROGRAM_NAME + "|*.csv|All|*.*";
            if (DialogResult.OK == file.ShowDialog())
            {
                string[] value = System.IO.File.ReadAllLines(file.FileName);
                for (int i = 0; i < value.Length; i++)
                {
                    if (0 == i)
                    {
                        foreach (string column in value[i].Split(','))
                        {
                            users.Columns.Add(column, 100);
                        }
                    }
                    else
                    {
                        users.Items.Add(new ListViewItem(value[i].Split(',')));
                    }
                }
            }
        }
    }
}
