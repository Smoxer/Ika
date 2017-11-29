namespace Ikariam
{
    public class Connection
    {
        public static string EMPTY_PROXY { get { return "None"; } }
        public static string TOR_DEFAULT_PROXY { get { return "socks://127.0.0.1:9150"; } }

        public string UA, proxy;

        /// <summary>
        /// Initialize connection
        /// </summary>
        public Connection(string UA, string proxy)
        {
            this.UA = UA;
            this.proxy = proxy;
        }
    }
}
