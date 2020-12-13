using System;

namespace Ks
{
    namespace Common
    {
        public class InvalidImplementationException : Exception
        {
            public InvalidImplementationException()
            {
            }

            public InvalidImplementationException(string Message) : base(Message)
            {
            }

            public InvalidImplementationException(string Message, Exception InnerException) : base(Message, InnerException)
            {
            }
        }
    }
}
