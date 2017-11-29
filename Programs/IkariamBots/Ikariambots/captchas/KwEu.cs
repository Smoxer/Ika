using System;
using System.Web;
using System.Threading;
using Library;

namespace Captchas
{
    public class KwEu : CaptchaSolver
    {
        public KwEu() : base()
        {

        }

        public override string SendCaptcha(out string captchaID, out bool success)
        {
            string parameters = "action=usercaptchaupload&file-upload-01=" + HttpUtility.UrlEncode(Convert.ToBase64String(image)) + "&nomd5=1&maxtimeout=900&base64=1&apikey=" + APIKeyOrName + "&source=" + APISourceOrPassword + "&time=" + (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString();
            string captchaSolveID = client.UploadString(hostname, parameters);
            captchaID = captchaSolveID;

            //Thread.Sleep(75000);

            parameters = "action=usercaptchacorrectdata&id=" + captchaSolveID + "&apikey=" + APIKeyOrName + "&source=" + APISourceOrPassword;
            string captchaValue = "";
            success = true;
            while (captchaValue.Equals(""))
            {
                int secs = 5;
                int.TryParse(Settings.Instance.Get("KWSecondsInterval", "5"), out secs);
                Thread.Sleep(secs * 1000);
                try
                {
                    captchaValue = client.DownloadString(hostname + "?" + parameters);
                    success = true;
                }
                catch (Exception e)
                {
                    captchaValue = e.ToString();
                    success = false;
                }
            }

            return captchaValue;
        }

        public override void CaptchaWorked(string captchaID, bool worked)
        {
            client.DownloadString(hostname + "?action=usercaptchacorrectback&id=" + captchaID + "&apikey=" + APIKeyOrName + "&source=" + APISourceOrPassword + "&correct=" + (worked ? "1" : "2"));
        }

        public override string GetBalance()
        {
            return client.DownloadString(hostname + "?action=usercaptchaguthaben&apikey=" + APIKeyOrName);
        }
    }
}
