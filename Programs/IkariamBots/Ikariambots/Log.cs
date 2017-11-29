using Library;
using System;
using System.IO;

namespace IkariamBots
{
    class Log
    {
        private static readonly object _SyncRoot = new object();
        private static Log _Instance = new Log();

        private Log()
        {

        }

        public static Log Instance
        {
            get
            {
                lock (_SyncRoot)
                {
                    return _Instance;
                }
            }
        }

        /// <summary>
        /// Write crash detailes to a log file
        /// </summary>
        public void Error(string StackTrace, string Source = "", string Message = "")
        {
            using (StreamWriter w = File.AppendText(Settings.Instance.Get("LogFile", "Log")))
            {
                w.Write(string.Format("------------------CRASH-----------------\n{4} Windows {5} ({6})\n{0}, {1}\n\n{2}\n\n{3}\n-----------------END CRASH-----------------\n", 
                    DateTime.Now.ToString(Config.DATE_TIME_FORMAT), 
                    StackTrace, 
                    Source, 
                    Message, 
                    (Environment.Is64BitOperatingSystem ? "64 bit": "32 bit"), 
                    Environment.OSVersion, 
                    Config.VERSION)
                );
                //OS version: https://msdn.microsoft.com/en-us/library/windows/desktop/ms724832(v=vs.85).aspx
                w.Flush();
                w.Close();
            }
        }

        /// <summary>
        /// Write info detailes to a log file
        /// </summary>
        public void Info(string User, string Message)
        {
            using (StreamWriter w = File.AppendText(Settings.Instance.Get("LogFile", "Log")))
            {
                w.Write(string.Format("{0}, {1} - {2}\n", DateTime.Now.ToString(Config.DATE_TIME_FORMAT), User, Message));
                w.Flush();
                w.Close();
            }
        }
    }
}
