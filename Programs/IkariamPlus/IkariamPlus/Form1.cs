using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Net;

namespace IkariamPlus
{
    public partial class Form1 : Form
    {
        private bool inInstall;
		private const string SERVER = "http://www.ikariamplus.com/";

        public Form1()
        {
            InitializeComponent();
            installFolder.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Ika+\\");
            inInstall = false;
            log.Items.Clear();
        }

        private void installFolder_MouseUp(object sender, MouseEventArgs e)
        {
            if (!inInstall)
            {
                FolderBrowserDialog selectInstall = new FolderBrowserDialog();
                selectInstall.Description = "Select the directory that you want to install Ika+";
                selectInstall.SelectedPath = installFolder.Text;
                selectInstall.ShowNewFolderButton = true;
                DialogResult result = selectInstall.ShowDialog();
                if (!string.IsNullOrWhiteSpace(selectInstall.SelectedPath))
                {
                    if (!selectInstall.SelectedPath.Contains("Ika+"))
                        installFolder.Text = Path.Combine(selectInstall.SelectedPath, "Ika+\\");
                    else
                        installFolder.Text = Path.Combine(selectInstall.SelectedPath, "");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (inInstall)
                return;
            inInstall = true;
            button1.Enabled = false;
            progress.Value = 0;
            log.Items.Add("Unpacking C++ Redistributable 2013");
            if(Environment.Is64BitOperatingSystem)
                File.WriteAllBytes(Path.Combine(Path.GetTempPath(), "vcredist.exe"), Properties.Resources.vcredist_x64);
            else
                File.WriteAllBytes(Path.Combine(Path.GetTempPath(), "vcredist.exe"), Properties.Resources.vcredist_x86);
            progress.Value = 20;
            log.Items.Add("Installing C++ Redistributable 2013");
            var vcredist = Process.Start(Path.Combine(Path.GetTempPath(), "vcredist.exe"), "/install /q /norestart");
            vcredist.WaitForExit();
            progress.Value = 40;
            progress.Value = 60;
            log.Items.Add("Writing files to " + installFolder.Text);
            if (!Directory.Exists(installFolder.Text))
                Directory.CreateDirectory(installFolder.Text);
            progress.Value = 80;
            WebClient client = new WebClient();
            client.DownloadFile(SERVER + "Updater", Path.Combine(installFolder.Text, "Updater.exe"));
            client.DownloadFile(SERVER + "NUnrar.dll", Path.Combine(installFolder.Text, "NUnrar.dll"));
            log.Items.Add("Installing updates");
            progress.Value = 100;
            Process.Start(Path.Combine(installFolder.Text, "Updater.exe"));

            MessageBox.Show("Installation complete!\nIka+ will open when finished downloading updates", "Installation complete!");
            Application.Exit();
            inInstall = false;
        }
    }
}
