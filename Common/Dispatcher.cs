using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Ks.Common
{
    public class Dispatcher : IDisposable
    {
        private void RunAction((Action Action, int Id) AI)
        {
            this.WaitingActions.TryGetValue(AI.Id, out var WaitingData);

            try
            {
                AI.Action.Invoke();
            }
            catch (Exception Ex)
            {
                if (WaitingData.WaitHandle == null)
                {
                    this.OnExceptionUnhandled(new ExceptionUnhandledEventArgs(Ex));
                }
                else
                {
                    WaitingData = (WaitingData.WaitHandle, Ex);
                }
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
            {
                throw new InvocationException("Action invocation threw an exception. See InnerException for more details.", WaitingData.Exception);
            }
        }

        public void SetSynchronizationContext()
        {
            if (this.Thread == null)
            {
                this.Thread = Thread.CurrentThread;
            }

            Verify.True(this.IsSameThread(), $"You can only call `{nameof(this.SetSynchronizationContext)}` from the thread of the dispatcher.");

            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(this));
        }

        public void DoActions()
        {
            if (this.Thread == null)
            {
                this.Thread = Thread.CurrentThread;
            }

            Verify.True(this.IsSameThread(), this.IsDispatching ? $"You can only call `{nameof(this.DoActions)}` from the same thread the dispatcher is running on." : "The dispatcher cannot be run from two different threads.");

            var IsRunning = this.IsRunning;
            this.IsRunning = true;

            var List = new List<(Action Action, int Id)>();
            while (true)
            {
                if (!this.Queue.TryTake(out var AI))
                {
                    break;
                }

                List.Add(AI);
            }

            foreach (var AI in List)
            {
                this.RunAction(AI);
            }

            this.IsRunning = IsRunning;
        }

        public void Run()
        {
            Verify.False(this.IsShutDown, "The dispatcher has already been shut down.");
            Verify.False(this.IsRunning, $"The dispatcher is already running something on {(this.IsSameThread() ? "the same" : "another")} thread.");

            if (this.Thread != null)
            {
                Verify.True(this.IsSameThread(), "The dispatcher cannot be run from two different threads.");
            }

            this.Thread = Thread.CurrentThread;

            this.IsDispatching = true;
            this.IsRunning = true;

            foreach (var AI in this.Queue.GetConsumingEnumerable())
            {
                this.RunAction(AI);

                if (this.IsShuttingDown)
                {
                    this.DoActions();
                    break;
                }
            }

            this.IsDispatching = false;
            this.IsRunning = false;
            this.IsShutDown = true;

            this.OnDispatcherShutDown();
        }

        public void ShutDown()
        {
            this.IsShuttingDown = true;
        }

        private bool IsSameThread()
        {
            return this.Thread.ManagedThreadId == Thread.CurrentThread.ManagedThreadId;
        }

        protected virtual void Dispose(bool Disposing)
        {
            if (!this.IsDisposed)
            {
                if (Disposing)
                {
                    ((IDisposable) this.WaitingActions).Dispose();
                    this.Queue.Dispose();
                }
            }
            this.IsDisposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
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

        public bool IsDispatching { get; private set; }

        public bool IsRunning { get; private set; }

        public bool IsShutDown { get; private set; }

        public bool IsShuttingDown { get; private set; }

        public static Dispatcher Current
        {
            get
            {
                return (SynchronizationContext.Current as DispatcherSynchronizationContext)?.Dispatcher;
            }
        }

        public Thread Thread { get; private set; }

        public bool IsDisposed { get; private set; }

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
            this.Exception = Exception;
        }

        public Exception Exception { get; }
    }
}
