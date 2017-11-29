using System;
using System.IO;
using System.Net;
using System.Linq;
using Microsoft.Win32;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using Library;

namespace IkariamBots
{
    internal static class Program
    {
        private static string[] arguments;

        /// <summary>
        /// Run the form
        /// </summary>
        private static void RunProgram()
        {
            MainWindow f = new MainWindow(Config.VERSION);
            f.Text = Config.PROGRAM_NAME;

            string[] locations = { "Reports", "Reports/ResultFight", "Reports/AllFight", "Reports/Fight", "Reports/Spy" };
            foreach (string location in locations)
            {
                if (!Directory.Exists(location + "/"))
                {
                    Directory.CreateDirectory(location + "/");
                }
            }

            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            string proxy = "None";
            if (Settings.Instance.Get("EnableProxy", "false").Equals("true"))
            {
                proxy = Settings.Instance.Get("ProxyAddress", "");
            }
            
            switch (Settings.Instance.Get("ProgramMode", "Login"))
            {
                case "Piracy":
                    Application.Run(new PiracyHide(
                        (arguments.Contains("true") ? true : false) //Auto start
                        ));
                    break;
                case "Register":
                    Application.Run(new AutoRegister(
                        arguments[1],
                        arguments[2],
                        arguments[3],
                        Settings.Instance.Get("UserAgent", ""),
                        (Settings.Instance.Get("EnableProxy", "false").Equals("true") ? Settings.Instance.Get("ProxyAddress", "") : "None"),
                        arguments[4],
                        (arguments.Length > 5 ? int.Parse(arguments[5]) : 1),
                        (arguments.Length > 6 ? int.Parse(arguments[6]) : 1)
                        ));
                    break;
                default:
                    Application.Run(f);
                    break;
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (Settings.Instance.Get("ProgramMode", "Login").Equals("Piracy"))
                File.WriteAllLines("massiveUsers.users", PiracyHide.massiveUsersFile);
            Log.Instance.Error(e.ExceptionObject.ToString());
            Process.Start(Application.ExecutablePath, string.Join(" ", arguments) + " true");
            Application.Exit();
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            if (Settings.Instance.Get("ProgramMode", "Login").Equals("Piracy"))
                File.WriteAllLines("massiveUsers.users", PiracyHide.massiveUsersFile);
            Log.Instance.Error(e.Exception.StackTrace, e.Exception.Source, e.Exception.Message);
            Process.Start(Application.ExecutablePath, string.Join(" ", arguments) + " true");
            Application.Exit();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            arguments = Environment.GetCommandLineArgs();
            Languages lang = Languages.Instance;
            
            try
            {
                RunProgram();

            } catch(Exception e)
            {
                MessageBox.Show(e.ToString());
                if (Settings.Instance.Get("ProgramMode", "Login").Equals("Piracy"))
                    File.WriteAllLines("massiveUsers.users", PiracyHide.massiveUsersFile);
                Log.Instance.Error(e.StackTrace, e.Source, e.Message);
                Process.Start(Application.ExecutablePath, string.Join(" ", Environment.GetCommandLineArgs()) + " true");
                Application.Exit();
            } 
        }
    }
}


/*
 * 
 * Build city
 * string UserAnswer = Interaction.InputBox("Please enter islands coordinates, split by \",\"", "Start colonies", "");
            if (UserAnswer != "")
            {
                browser.ExecuteScriptAsync("var islands = \"" + UserAnswer + "\"; " + server.DownloadFile("ikariam_build_city"));
                workingScripts.Text = "Working scripts - Build city";
            }*/