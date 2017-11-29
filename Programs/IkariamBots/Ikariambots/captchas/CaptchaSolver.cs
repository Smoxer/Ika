using System.Net;

namespace Captchas
{
    public abstract class CaptchaSolver
    {
        //Define vars
        protected WebClient client;
        public string APIKeyOrName, APISourceOrPassword, hostname, provider;
        public uint port;
        public byte[] image;

        //Define methods
        public abstract string SendCaptcha(out string captchaID, out bool success);
        public abstract void CaptchaWorked(string captchaID, bool worked);
        public abstract string GetBalance();

        protected CaptchaSolver()
        {
            client = new WebClient();
        }
    }
}
