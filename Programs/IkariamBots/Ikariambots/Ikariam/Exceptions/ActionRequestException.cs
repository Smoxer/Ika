using System;

namespace Ikariam.Exceptions
{
    class ActionRequestException : Exception
    {
        public ActionRequestException()
        {

        }

        public ActionRequestException(string message) : base(message)
        {

        }
    }
}
