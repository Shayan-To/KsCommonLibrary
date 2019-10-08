using System.Collections.Generic;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Ks
{
    namespace Common
    {
        public class Dispatcher : IDisposable
        {
            private void RunAction((Action Action, int Id) AI)
            {
                (EventWaitHandle WaitHandle, Exception Exception) WaitingData;
                this.WaitingActions.TryGetValue(AI.Id, out WaitingData);

                try
                {
                    AI.Action.Invoke();
                }
                catch (Exception Ex)
                {
                    if (WaitingData.WaitHandle == null)
                        this.OnExceptionUnhandled(new ExceptionUnhandledEventArgs(Ex));
                    else
                        WaitingData = (WaitingData.WaitHandle, Ex);
                }

                if (WaitingData.WaitHandle != null)
                {
                    this.WaitingActions[AI.Id] = WaitingData;
                    WaitingData.WaitHandle.Set();
                }
            }

            public void Invoke(Action Delegate)
            {
                Verify.False(this.IsShuttingDown, "Cannot add an action when the dispatcher is shutting down.");

                var Id = 0;
                lock (this.LockObject)
                {
                    this.CurrentId += 1;
                    Id = this.CurrentId;
                }

                this.Queue.Add((Delegate, Id));
            }

            public void InvokeAndWait(Action Delegate)
            {
                Verify.False(this.IsShuttingDown, "Cannot add an action when the dispatcher is shutting down.");
                Verify.True(this.IsDispatching, "Cannot wait for invocation when the dispatcher is not running.");

                var Id = 0;
                lock (this.LockObject)
                {
                    this.CurrentId += 1;
                    Id = this.CurrentId;
                }

                var WaitingData = default((EventWaitHandle WaitHandle, Exception Exception));

                using (var WaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset))
                {
                    WaitingData = (WaitHandle, null);
                    this.WaitingActions.Add(Id, WaitingData);

                    this.Queue.Add((Delegate, Id));

                    WaitHandle.WaitOne();
                }

                WaitingData = this.WaitingActions[Id];
                Assert.True(this.WaitingActions.Remove(Id));

                if (WaitingData.Exception != null)
                    throw new InvocationException("Action invocation threw an exception. See InnerException for more details.", WaitingData.Exception);
            }

            public void SetSynchronizationContext()
            {
                if (this.Thread == null)
                    this._Thread = Thread.CurrentThread;

                Verify.True(this.IsSameThread(), $"You can only call `{nameof(this.SetSynchronizationContext)}` from the thread of the dispatcher.");

                SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(this));
            }

            public void DoActions()
            {
                if (this.Thread == null)
                    this._Thread = Thread.CurrentThread;

                Verify.True(this.IsSameThread(), this.IsDispatching ? $"You can only call `{nameof(this.DoActions)}` from the same thread the dispatcher is running on." : "The dispatcher cannot be run from two different threads.");

                var IsRunning = this.IsRunning;
                this._IsRunning = true;

                var List = new List<(Action Action, int Id)>();
                do
                {
                    (Action Action, int Id) AI;
                    if (!this.Queue.TryTake(out AI))
                        break;
                    List.Add(AI);
                }
                while (true);

                foreach (var AI in List)
                    this.RunAction(AI);

                this._IsRunning = IsRunning;
            }

            public void Run()
            {
                Verify.False(this.IsShutDown, "The dispatcher has already been shut down.");
                Verify.False(this.IsRunning, $"The dispatcher is already running something on {(this.IsSameThread() ? "the same" : "another")} thread.");

                if (this.Thread != null)
                    Verify.True(this.IsSameThread(), "The dispatcher cannot be run from two different threads.");

                this._Thread = Thread.CurrentThread;

                this._IsDispatching = true;
                this._IsRunning = true;

                foreach (var AI in this.Queue.GetConsumingEnumerable())
                {
                    this.RunAction(AI);

                    if (this.IsShuttingDown)
                    {
                        this.DoActions();
                        break;
                    }
                }

                this._IsDispatching = false;
                this._IsRunning = false;
                this._IsShutDown = true;

                this.OnDispatcherShutDown();
            }

            public void ShutDown()
            {
                this._IsShuttingDown = true;
            }

            private bool IsSameThread()
            {
                return this.Thread.ManagedThreadId == Thread.CurrentThread.ManagedThreadId;
            }

            protected virtual void Dispose(bool Disposing)
            {
                if (!this._IsDisposed)
                {
                    if (Disposing)
                    {
                        ((IDisposable)this.WaitingActions).Dispose();
                        this.Queue.Dispose();
                    }
                }
                this._IsDisposed = true;
            }

            public void Dispose()
            {
                Dispose(true);
            }

            public event EventHandler<ExceptionUnhandledEventArgs> ExceptionUnhandled;

            protected virtual void OnExceptionUnhandled(ExceptionUnhandledEventArgs E)
            {
                ExceptionUnhandled?.Invoke(this, E);
            }

            public event EventHandler DispatcherShutDown;

            protected virtual void OnDispatcherShutDown()
            {
                DispatcherShutDown?.Invoke(this, EventArgs.Empty);
            }

            private bool _IsDispatching;

            public bool IsDispatching
            {
                get
                {
                    return this._IsDispatching;
                }
            }

            private bool _IsRunning;

            public bool IsRunning
            {
                get
                {
                    return this._IsRunning;
                }
            }

            private bool _IsShutDown;

            public bool IsShutDown
            {
                get
                {
                    return this._IsShutDown;
                }
            }

            private bool _IsShuttingDown;

            public bool IsShuttingDown
            {
                get
                {
                    return this._IsShuttingDown;
                }
            }

            public static Dispatcher Current
            {
                get
                {
                    return (SynchronizationContext.Current as DispatcherSynchronizationContext)?.Dispatcher;
                }
            }

            private Thread _Thread;

            public Thread Thread
            {
                get
                {
                    return this._Thread;
                }
            }

            private bool _IsDisposed;

            public bool IsDisposed
            {
                get
                {
                    return this._IsDisposed;
                }
            }

            private readonly object LockObject = new object();
            private int CurrentId;
            private readonly IDictionary<int, (EventWaitHandle WaitHandle, Exception Exception)> WaitingActions = new ConcurrentDictionary<int, (EventWaitHandle WaitHandle, Exception Exception)>(2, 8);
            private readonly BlockingCollection<(Action Action, int Id)> Queue = new BlockingCollection<(Action Action, int Id)>();

            private class DispatcherSynchronizationContext : SynchronizationContext
            {
                public DispatcherSynchronizationContext(Dispatcher Dispatcher)
                {
                    this.Dispatcher = Dispatcher;
                }

                public override void Post(SendOrPostCallback d, object state)
                {
                    this.Dispatcher.Invoke(() => d.Invoke(state));
                }

                public override void Send(SendOrPostCallback d, object state)
                {
                    this.Dispatcher.InvokeAndWait(() => d.Invoke(state));
                }

                public override SynchronizationContext CreateCopy()
                {
                    return new DispatcherSynchronizationContext(this.Dispatcher);
                }

                public readonly Dispatcher Dispatcher;
            }
        }

        public class ExceptionUnhandledEventArgs : EventArgs
        {
            public ExceptionUnhandledEventArgs(Exception Exception)
            {
                this._Exception = Exception;
            }

            private readonly Exception _Exception;

            public Exception Exception
            {
                get
                {
                    return this._Exception;
                }
            }
        }
    }
}
