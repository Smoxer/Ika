using IkariamBots.Forms;
using Library;
using System.Windows.Forms;

namespace Captchas
{
    public class ManualCaptcha : CaptchaSolver
    {
        public ManualCaptcha() : base()
        {

        }

        public override string SendCaptcha(out string captchaID, out bool success)
        {
            string captchaValue = null;
            //var imageConverter = new ImageConverter();
            //var imageCaptcha = (Image)imageConverter.ConvertFrom(image);

            using (var form = new ManualCaptchaInput(image))
            {
                form.BringToFront();
                var result = form.ShowDialog();
                success = DialogResult.OK == result;
                captchaID = string.Empty;
                captchaValue = form.value;
            }
            
            return captchaValue;
        }

        public override void CaptchaWorked(string captchaID, bool worked)
        {
            if(!worked)
                MessageBox.Show(Languages.Instance.Get("Piracy", "CaptchaIncorrect"), Languages.Instance.Get("Piracy", "Captcha"));
        }

        public override string GetBalance()
        {
            return "∞";
        }
    }
}
