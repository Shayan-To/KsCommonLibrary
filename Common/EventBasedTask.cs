using System.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;

namespace Ks
{
    namespace Common
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
                if (!this._IsCompleted)
                {
                    this._IsCompleted = true;
                    this.CompletedAction?.Invoke();
                }
            }

            public void OnCompleted(Action continuation)
            {
                Verify.True(this.CompletedAction == null);
                this.CompletedAction = continuation;
            }

            private bool _IsCompleted;

            public bool IsCompleted
            {
                get
                {
                    return this._IsCompleted;
                }
            }

            private Action CompletedAction;
        }
    }
}
