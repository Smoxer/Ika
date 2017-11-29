namespace Captchas
{
    public class CaptchaFactory
    {
        public static CaptchaSolver Captcha(string captchaName, string apiKey, string apiSource)
        {
            CaptchaSolver captchaSolver;
            if (captchaName.Equals("9KW") && apiKey != "" && apiSource != "")
            {
                captchaSolver = new KwEu();
                captchaSolver.hostname = "https://www.9kw.eu/index.cgi";
                captchaSolver.APIKeyOrName = apiKey;
                captchaSolver.APISourceOrPassword = apiSource;
            }
            else if (captchaName.Equals("DeCaptcher") && apiKey != "" && apiSource != "")
            {
                captchaSolver = new Decaptcher();
                captchaSolver.hostname = "http://poster.de-captcher.com/"; //api.de-captcher.com
                captchaSolver.port = 3500;
                captchaSolver.APIKeyOrName = apiKey;
                captchaSolver.APISourceOrPassword = apiSource;
            }
            else if (captchaName.Equals("DeathByCaptcha") && apiKey != "" && apiSource != "")
            {
                captchaSolver = new DeathByCaptcha();
                captchaSolver.APIKeyOrName = apiKey;
                captchaSolver.APISourceOrPassword = apiSource;
            }
            else
            {
                captchaSolver = new ManualCaptcha();
            }
            captchaSolver.provider = captchaName;
            return captchaSolver;
        }
    }
}
