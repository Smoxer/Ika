using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace IkariamBots.Forms
{
    public partial class ScriptEditor : Form
    {
        public ScriptEditor(string text = "")
        {
            InitializeComponent();
            richTextBox1.Text = Config.DEFAULT_SCRIPT + "\n\n";

            if(!text.Equals(""))
                richTextBox1.Text += text;
        }

        public void AddText(string text)
        {
            if (!text.Equals(""))
                richTextBox1.Text += "\n" + text + "\n";
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = Config.DEFAULT_SCRIPT;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = Config.PROGRAM_NAME + "|*.ikr|All|*.*";
            if (file.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Text = File.ReadAllText(file.FileName);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = Config.PROGRAM_NAME + " (.ikr)|*.ikr";
            if (sd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sd.FileName, richTextBox1.Text);
            }
        }

        private void runCurrentScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Thread _ScriptLanguage = new Thread(MainWindow.scriptLanguage.RunScript);
            MainWindow.isScriptLanguageCanRun = true;
            _ScriptLanguage.Start(richTextBox1.Text);
        }

        private void sendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new SendGyro())
            {
                var result = form.ShowDialog();
                if (DialogResult.OK == result)
                {
                    int PerRound = form.PerRound;
                    int Rounds = form.Rounds;
                    richTextBox1.Text += "\n# Sending " + Rounds.ToString() + "*" + PerRound.ToString() + "(" + (Rounds*PerRound).ToString() + ") Gyrocopters and " + Rounds.ToString() + " " + form.Unit + " in order to detroy enemy's balloon bombardier";
                    richTextBox1.Text += "\nSet " + form.Unit + " 1";
                    richTextBox1.Text += "\nSet Gyrocopter " + PerRound.ToString();

                    for (int i = 0; i < Rounds; i++)
                    {
                        richTextBox1.Text += "\n" + form.Option;
                        if(i != Rounds-1)
                            richTextBox1.Text += "\nWait 1";
                    }

                    richTextBox1.Text += "\nResetUnits";
                }
            }
        }

        private void clearQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void appendQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text += "\n# Assuming all the cities in the same island";

            
        }

        private void showQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string totalText = String.Empty;
            
            MessageBox.Show(totalText);
        }
    }
}
