using System;

namespace Ks
{
    namespace Common
    {
        public sealed class Void
        {
            private Void()
            {
                throw new NotSupportedException();
            }
        }
    }
}
