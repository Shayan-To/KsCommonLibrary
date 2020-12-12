using System;

namespace Ks.Common
{
    public class Closable : IDisposable
    {
        public Closable(Action CloseOperation)
        {
            this.CloseOperation = CloseOperation;
        }

        public void Close()
        {
            this.CloseOperation.Invoke();
        }

        void IDisposable.Dispose()
        {
            this.Close();
        }

        private readonly Action CloseOperation;
    }
}
