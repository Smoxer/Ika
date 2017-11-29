using System;
using System.IO;
using System.Net;
using NUnrar.Common;
using NUnrar.Archive;
using System.Reflection;
using System.Diagnostics;
using IWshRuntimeLibrary;

namespace Updater
{
    class Program
    {
		private const string SERVER = "http://www.ikariamplus.com/";
		
        static void Main(string[] args)
        {
            try
            {
                string location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                WebClient web = new WebClient();
                Console.WriteLine("Downloading file . . .");
                string fileName = "IkariamPlusx86.rar";
                if (Environment.Is64BitOperatingSystem)
                {
                    fileName = "IkariamPlus.rar";
                }
                //
                web.DownloadFile(SERVER + fileName, Path.Combine(location, fileName));
                foreach (var process in Process.GetProcessesByName("Launcher"))
                {
                    process.Kill();
                }
                foreach (var process in Process.GetProcessesByName("IkariamPlus"))
                {
                    process.Kill();
                }
                Console.WriteLine("Unpacking " + fileName + " . . .");
                RarArchive archive = RarArchive.Open(Path.Combine(location,fileName));
                foreach (RarArchiveEntry entry in archive.Entries)
                {
                    Console.WriteLine("Unpacking " + Path.GetFileName(entry.FilePath));
                    string fileName2 = Path.GetFileName(entry.FilePath);
                    string rootToFile = Path.Combine(location, entry.FilePath.Replace(fileName2, ""));
                    if (fileName2 != "NUnrar.dll" && fileName2 != "Updater.exe")
                    {

                        if (!Directory.Exists(rootToFile))
                        {
                            Directory.CreateDirectory(rootToFile);
                        }

                        entry.WriteToFile(Path.Combine(rootToFile, fileName2), ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                    }

                }

                object shDesktop = (object)"Desktop";
                WshShell shell = new WshShell();
                string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\Ika+.lnk";
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                shortcut.Description = "Shortcut for Ika+";
                shortcut.WorkingDirectory = location;
                shortcut.TargetPath = Path.Combine(location, "Launcher.exe");
                shortcut.Save();

            
            
                if (System.IO.File.Exists(Path.Combine(location, "Launcher.exe")))
                    Process.Start(Path.Combine(location, "Launcher.exe"));
                //Console.WriteLine("Clearing . . .");
                //if (System.IO.File.Exists(Path.Combine(location, fileName)))
                    //System.IO.File.Delete(Path.Combine(location, fileName));
            } catch(Exception e) { }
        }
    }
}
