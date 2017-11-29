/**
 */

namespace DeathByCaptcha
{
    /**
     * <summary>Exception indicating the CAPTCHA image was rejected due to being empty, or too big, or not a valid image at all.</summary>
     */
    public class InvalidCaptchaException : Exception
    {
        public InvalidCaptchaException() : base()
        {}

        public InvalidCaptchaException(string message) : base(message)
        {}
    }
}
