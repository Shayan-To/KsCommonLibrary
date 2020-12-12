using System;

namespace Ks.Common
{
    public class AssertionException : Exception
    {
        public AssertionException()
        {
        }

        public AssertionException(string Message) : base(Message)
        {
        }

        public AssertionException(string Message, Exception InnerException) : base(Message, InnerException)
        {
        }
    }
}
