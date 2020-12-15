using System;

namespace Ks.Common
{
    public sealed class Void
    {
        private Void()
        {
            throw new NotSupportedException();
        }
    }
}
