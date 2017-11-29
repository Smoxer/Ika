using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Captchas
{
    public class TwoCaptcha : CaptchaSolver
    {
        public TwoCaptcha() : base()
        {

        }

        public override void CaptchaWorked(string captchaID, bool worked)
        {
            throw new NotImplementedException();
        }

        public override string GetBalance()
        {
            throw new NotImplementedException();
        }

        public override string SendCaptcha(out string captchaID, out bool success)
        {
            throw new NotImplementedException();
        }
    }
}
