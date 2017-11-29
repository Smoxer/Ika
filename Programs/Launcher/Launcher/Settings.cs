using System.IO;
using IniParser;
using IniParser.Model;

namespace Launcher
{
    public class Settings
    {
        private static Settings _Instance = new Settings();
        private static readonly object _SyncRoot = new object();

        private IniData settings;
        private FileIniDataParser parser;

        public static Settings Instance
        {
            get
            {
                lock (_SyncRoot)
                {
                    return _Instance;
                }
            }
        }

        private Settings()
        {
            if (!File.Exists("Settings.ini"))
            {
                File.WriteAllText("Settings.ini", "[settings]", System.Text.Encoding.UTF8);
            }
            parser = new FileIniDataParser();
            settings = parser.ReadFile("Settings.ini", System.Text.Encoding.UTF8);
        }

        public string Get(string key, string defaultValue)
        {
            string result = settings["settings"][key];
            if (null == result || result.Equals(string.Empty))
            {
                result = defaultValue;
                Set(key, defaultValue);
                Commit();
            }
            return result;
        }

        public void Set(string key, string value)
        {
            settings["settings"][key] = value;
            Commit();
        }

        public void Commit()
        {
            parser.WriteFile("Settings.ini", settings, System.Text.Encoding.UTF8);
        }
    }
}
