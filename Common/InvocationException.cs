using System;

namespace Ks.Common
{
        public class InvocationException : Exception
        {
            public InvocationException() : base()
            {
            }

            public InvocationException(string message) : base(message)
            {
            }

            public InvocationException(string message, Exception innerException) : base(message, innerException)
            {
            }
        }
    }
