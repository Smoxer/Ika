/**
 */

namespace DeathByCaptcha
{
    /**
     * <seealso cref="T:DeathByCaptcha.Client.DecodeDelegate"/>
     * <summary>Helper class for asyncronous CAPTCHA decoding.</summary>
     */
    public class PollPayload
    {
        /**
         * <value>Decoding callback.</value>
         */
        public DecodeDelegate Callback;

        /**
         * <value>Uploaded CAPTCHA.</value>
         */
        public Captcha Captcha;

        /**
         * <value>Solving timeout (in seconds).</value>
         */
        public int Timeout;
    }
}
