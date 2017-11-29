using DeathByCaptcha;

namespace Captchas
{
    public class DeathByCaptcha : CaptchaSolver
    {
        private new Client client;

        public DeathByCaptcha() : base()
        {
            
        }

        public override void CaptchaWorked(string captchaID, bool worked)
        {
            if (!worked)
                client.Report(int.Parse(captchaID));
        }

        public override string GetBalance()
        {
            if (null == client)
            {
                client = new HttpClient(APIKeyOrName, APISourceOrPassword);
            }

            return client.Balance.ToString();
        }

        public override string SendCaptcha(out string captchaID, out bool success)
        {
            if(null == client)
            {
                client = new HttpClient(APIKeyOrName, APISourceOrPassword);
            }

            try
            {
                Captcha captcha = client.Decode(image, Client.DefaultTimeout);
                if (null != captcha)
                {
                    captchaID = captcha.Id.ToString();
                    success = true;
                    return captcha.Text;
                }
            }catch(Exception e) { }
            captchaID = "";
            success = false;
            return "None";
        }
    }
}
