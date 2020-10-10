using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Ks.Common
{
    [Obsolete("Use " + nameof(TaskCompletionSource<Void>) + " instead.", true)]
    public class EventBasedTask : INotifyCompletion
    {
        public EventBasedTask GetAwaiter()
        {
            return this;
        }

        public void GetResult()
        {
            Verify.True(this.IsCompleted);
        }

        public void SetComplete()
        {
            if (!this.IsCompleted)
            {
                this.IsCompleted = true;
                this.CompletedAction?.Invoke();
            }
        }

        public void OnCompleted(Action continuation)
        {
            Verify.True(this.CompletedAction == null);
            this.CompletedAction = continuation;
        }

        public bool IsCompleted { get; private set; }

        private Action CompletedAction;
    }
}
