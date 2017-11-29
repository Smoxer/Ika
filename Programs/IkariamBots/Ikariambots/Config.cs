using Library;
using System.Management;

namespace IkariamBots
{
    /// <summary>
    /// The configured class for the program.
    /// </summary>
    public static class Config
    {
        public static string VERSION
        {
            get { return System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion; } // The program current version
        }

        public const string PROGRAM_NAME = "Ika+"; // The program name
        public const string DATE_TIME_FORMAT = "dd/MM/yyyy HH:mm:ss"; // Date and time format
        public const string TIME_FORMAT = "HH:mm:ss"; //Time format
        public const string CACHE_PATH = "data/"; // Relative cache path
        public const string PROTOCOL = "https";

        public static string DEFAULT_SCRIPT { get { return "; " + Languages.Instance.Get("Misc", "SeeIn"); } }
    }
}
