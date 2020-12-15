using System;
using System.Collections.Generic;
using System.Text;

namespace Ks.Common
{
    public struct LambdaCallDisposable : IDisposable
    {
        public LambdaCallDisposable(Action action)
        {
            this.Action = action;
        }

        public void Dispose()
        {
            this.Action?.Invoke();
        }

        public Action Action { get; }
    }
}
