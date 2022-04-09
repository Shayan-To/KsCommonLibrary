using System;

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
