using System.IO;
using IniParser;
using IniParser.Model;

namespace Launcher
{
    public class Languages
    {
        public const string LANGUAGES_LOCATION = "languages/";

        private static Languages _Instance = new Languages();
        private static readonly object _SyncRoot = new object();

        private IniData language, defaultLanguage;

        public static Languages Instance
        {
            get
            {
                lock (_SyncRoot)
                {
                    return _Instance;
                }
            }
        }

        private Languages()
        {
            if (!Directory.Exists(LANGUAGES_LOCATION))
                Directory.CreateDirectory(LANGUAGES_LOCATION);
            var parser = new FileIniDataParser();
            language = parser.ReadFile(LANGUAGES_LOCATION + Settings.Instance.Get("Language", "English") + ".ini", System.Text.Encoding.UTF8);
            defaultLanguage = parser.ReadFile(LANGUAGES_LOCATION + "English.ini", System.Text.Encoding.UTF8);
        }

        public string Get(string section, string key)
        {
            string result = language[section][key];
            if (null == result || result.Equals(string.Empty))
                result = defaultLanguage[section][key];
            return result.Replace("\\n", "\n");
        }
    }
}
