using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Windows.Forms;
using com.LandonKey.SocksWebProxy.Proxy;

namespace IkariamBots
{
    public class PlayableWebClient : WebClient
    {

        public PlayableWebClient()
                : this(new CookieContainer())
            { }
        public PlayableWebClient(CookieContainer c)
        {
            CookieContainer = c;

            Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            Encoding = System.Text.Encoding.UTF8;
        }

        public PlayableWebClient(CookieContainer c, string proxy, string UA)
        {
            CookieContainer = c;

            Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            Encoding = System.Text.Encoding.UTF8;

            if (proxy != "None" && proxy != "")
                SetProxy(proxy);
            SetUA(UA);
        }
        public CookieContainer CookieContainer { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);

            var castRequest = request as HttpWebRequest;
            if (castRequest != null)
            {
                castRequest.CookieContainer = this.CookieContainer;
            }

            return request;
        }

        public void SetProxy(string proxy)
        {
            if (proxy.ToLower().Contains("socks"))
            {
                proxy = proxy.Replace("socks://127.0.0.1:", "");
                TcpListener l = new TcpListener(IPAddress.Loopback, 0);
                l.Start();
                int port = ((IPEndPoint)l.LocalEndpoint).Port;
                l.Stop();

                Thread.Sleep(1000);

                var proxy_socks = new com.LandonKey.SocksWebProxy.SocksWebProxy(new ProxyConfig(IPAddress.Parse("127.0.0.1"), port, IPAddress.Parse("127.0.0.1"), int.Parse(proxy), ProxyConfig.SocksVersion.Five));
                if (!proxy_socks.IsActive())
                {
                    Thread.Sleep(5000);
                    proxy_socks = new com.LandonKey.SocksWebProxy.SocksWebProxy(new ProxyConfig(IPAddress.Parse("127.0.0.1"), port, IPAddress.Parse("127.0.0.1"), int.Parse(proxy), ProxyConfig.SocksVersion.Five));
                    if (!proxy_socks.IsActive())
                    {
                        MessageBox.Show("Can't establish proxy connection on port " + proxy + "\nDoes port " + proxy + " is open to use?");
                        Application.Exit();
                    }
                }
                Proxy = proxy_socks;
            }
            else
            {
                WebProxy wp = new WebProxy(proxy);
                Proxy = wp;
            }
        }

        public void SetUA(string UA)
        {
            Headers.Add("user-agent", UA);
        }
    }
}
