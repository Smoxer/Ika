using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using Library;

namespace IkariamBots.Forms
{
    public partial class ManualCaptchaInput : Form
    {
        public string value;
        private static Languages lang = Languages.Instance;

        public ManualCaptchaInput(byte[] image)
        {
            InitializeComponent();

            #region Translate
            Text = lang.Get("Piracy", "CaptchaDetected");
            send.Text = lang.Get("Piracy", "SendCaptcha");
            #endregion

            captcha.Image = Image.FromStream(new MemoryStream(image));
        }

        private void send_Click(object sender, EventArgs e)
        {
            value = captchaValue.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
