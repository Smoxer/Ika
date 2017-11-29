/**
 */

namespace DeathByCaptcha
{
    /**
     * <summary>Base DBC API exception.</summary>
     */
    public class Exception : System.Exception
    {
        public Exception() : base()
        {}

        public Exception(string message) : base(message)
        {}
    }
}
