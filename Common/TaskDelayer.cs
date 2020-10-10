#define WriteDebugInfo

using System;
using System.Diagnostics;

namespace Ks
{
    namespace Common
    {
        public class TaskDelayer : IDisposable
        {

            /// <param name="Task">The task that should be invoked. This call will be taken place on some background thread.</param>
            public TaskDelayer(Action Task, TimeSpan MinDelay, TimeSpan MaxDelay = default, TimeSpan InactivityTime = default)
            {
                this.StopWatch.Start();

                this._Task = Task;
                this._MinDelay = MinDelay;
                this._MaxDelay = (MaxDelay == TimeSpan.Zero) ? TimeSpan.MaxValue : MaxDelay;
                this._InactivityTime = InactivityTime;

                this.TaskThread = new System.Threading.Thread(this.TaskThreadProcedure) { IsBackground = true, Name = nameof(TaskDelayer) + " thread - " + this.GetHashCode().ToString() };
                this.TaskThread.Start();
            }

            public void RunTask(TaskDelayerRunningMode RunningMode)
            {
#if WriteDebugInfo
                Console.WriteLine("{0}: Started. Mode: {1}", nameof(RunTask), RunningMode);
#endif
                TimeSpan Now;
                // First Check that IsTaskPending is REALLY true. We have to lock to make that sure.
                if (this.IsTaskPending)
                {
                    lock (this.WaitLockObject)
                    {
                        Now = this.StopWatch.Elapsed;
                        if (this.IsTaskPending)
                        {
#if WriteDebugInfo
                            Console.WriteLine("{0}: Task was pending.", nameof(RunTask));
#endif
                            if (RunningMode == TaskDelayerRunningMode.Instant)
                            {
                                this.IsInstantSet = true;
                                this.DelayWaitHandle.Set();
#if WriteDebugInfo
                                Console.WriteLine("{0}: {1} was set.", nameof(RunTask), nameof(this.DelayWaitHandle));
#endif
                            }
                            else
// As setting LastActivityTime can at most delay the run of a task, it is only important to have it at the time of deciding whether to run the task.
if (this.LastActivityTime > Now)
                            {
                                // This usually means that the system's time has changed.
                                // The simplest thing to do is to set instant.
                                this.IsInstantSet = true;
                                this.DelayWaitHandle.Set();
#if WriteDebugInfo
                                Console.WriteLine("{0}: {1} was set.", nameof(RunTask), nameof(this.DelayWaitHandle));
#endif
                            }
                            else
                                this.LastActivityTime = Now;
                            return;
                        }
                    }
                }
                else
                    // We can skip locking as the other thread is locked at TaskWaitHandle. See the comment below.
                    Now = this.StopWatch.Elapsed;

                // If IsTaskPending is REALLY false, then the other thread is for sure locked at TaskWaitHandle.
                // So we safely can set it to true.
                this.IsTaskPending = true;
                this.FirstActivityTime = Now;
                this.LastActivityTime = this.FirstActivityTime;

                if (RunningMode == TaskDelayerRunningMode.Instant)
                {
                    this.IsInstantSet = true;
                    // We set the wait handle before waiting on it. The wait will then immediately return.
                    this.DelayWaitHandle.Set();
#if WriteDebugInfo
                    Console.WriteLine("{0}: {1} was set.", nameof(RunTask), nameof(this.DelayWaitHandle));
#endif
                }
                this.TaskWaitHandle.Set();
#if WriteDebugInfo
                Console.WriteLine("{0}: {1} was set.", nameof(RunTask), nameof(this.TaskWaitHandle));
#endif
            }

            private void TaskThreadProcedure()
            {
#if WriteDebugInfo
                Console.WriteLine("{0}: Started.", nameof(TaskThreadProcedure));
#endif
                // ToDo Prove that we need two wait handles. (Have done it once.)
                while (true)
                {
#if WriteDebugInfo
                    Console.WriteLine("{0}: {1}, getting into wait.", nameof(TaskThreadProcedure), nameof(this.TaskWaitHandle));
#endif
                    this.TaskWaitHandle.WaitOne();
#if WriteDebugInfo
                    Console.WriteLine("{0}: {1}, out of wait.", nameof(TaskThreadProcedure), nameof(this.TaskWaitHandle));
#endif
                    if (!this.IsTaskPending)
                    {
#if WriteDebugInfo
                        Console.WriteLine("{0}: Exiting...", nameof(TaskThreadProcedure));
#endif
                        Assert.True(this.IsDisposed);
                        break;
                    }

                    while (true)
                    {
                        var WaitTime = TimeSpan.Zero;
                        var ShouldRunTask = false;

                        // Here we will decide whether to run the task or not.
                        // So as LastActivityTime can set that to a later time, we have to take the last LastActivityTime into account.
                        // Every call to RunTask has to have an effect, either to start a pending task or to set an activity time (or set instant).
                        // So if RunTask has determined that IsTaskPending is true, it must set its activity time as it cannot set out a new pending task.
                        lock (this.WaitLockObject)
                        {
                            ShouldRunTask = this.IsInstantSet;
                            var Now = this.StopWatch.Elapsed;

                            {
                                if (ShouldRunTask)
                                {
#if WriteDebugInfo
                                    Console.WriteLine("{0}: Wait -> Instant.", nameof(TaskThreadProcedure));
#endif
                                    break;
                                }

                                // These " - Now"s were at the end. To avoid overflows, moved them a step back.

                                WaitTime = (this.FirstActivityTime - Now) + this.MinDelay;
                                if (WaitTime > TimeEpsilon)
                                {
#if WriteDebugInfo
                                    Console.WriteLine("{0}: Wait -> Min delay.", nameof(TaskThreadProcedure));
#endif
                                    break;
                                }

                                var MaxWaitTime = (this.FirstActivityTime - Now) + this.MaxDelay;
                                WaitTime = (this.LastActivityTime - Now) + this.InactivityTime;
                                if ((WaitTime <= TimeEpsilon) | (MaxWaitTime <= TimeEpsilon))
                                {
#if WriteDebugInfo
                                    Console.WriteLine("{0}: Wait -> No wait.", nameof(TaskThreadProcedure));
#endif
                                    ShouldRunTask = true;
                                    break;
                                }

                                if (WaitTime >= MaxWaitTime)
                                {
#if WriteDebugInfo
                                    Console.WriteLine("{0}: Wait -> Reaching max wait.", nameof(TaskThreadProcedure));
#endif
                                    // We know that next time the task must surely be done, so we set instant to avoid unnecessary next-time calculations.
                                    this.IsInstantSet = true;
                                    WaitTime = MaxWaitTime;
                                    break;
                                }

#if WriteDebugInfo
                                Console.WriteLine("{0}: Wait -> Regular wait.", nameof(TaskThreadProcedure));
#endif
                            }

                            if (ShouldRunTask)
                            {
                                // ToDo Isn't DelayWaitHandle always reset at this point?
                                this.DelayWaitHandle.Reset();
                                this.IsTaskPending = false;
                                this.IsInstantSet = false;
                            }
                        }

                        if (ShouldRunTask)
                        {
#if WriteDebugInfo
                            Console.WriteLine("{0}: Running task...", nameof(TaskThreadProcedure));
#endif
                            this.Task.Invoke();
                            break;
                        }
                        else
                        {
#if WriteDebugInfo
                            Console.WriteLine("{0}: {1}, getting into wait. WaitTime: {2}", nameof(TaskThreadProcedure), nameof(this.DelayWaitHandle), WaitTime);
#endif
                            this.DelayWaitHandle.WaitOne(WaitTime);
#if WriteDebugInfo
                            Console.WriteLine("{0}: {1}, out of wait.", nameof(TaskThreadProcedure), nameof(this.DelayWaitHandle));
#endif
                        }
                    }
                }
            }

            protected virtual void Dispose(bool Disposing)
            {
                if (!this.IsDisposed)
                {
                    this._IsDisposed = true;

                    if (Disposing)
                    {
                    }

                    // Free unmanaged resources (unmanaged objects) and override Finalize() below.

                    // The other thread is necessarily waiting in one of the wait handles (or soon will be).

                    // ... If it is waiting in DelayWaitHandle, a task is pending and must be done, so we set instant.
                    if (this.IsTaskPending)
                    {
                        // We have to ensure that when a task is set to be done, it is always necessarily done.
                        this.IsInstantSet = true;
                        this.DelayWaitHandle.Set();
                    }

                    // ... Otherwise, we set the TaskWaitHandle without setting IsTaskPending, so the thread understands that object is being disposed.
                    this.TaskWaitHandle.Set();
                    // ... And the important part, joining the thread so that everything is done before Dispose method returns.
                    this.TaskThread.Join();

                    this.TaskWaitHandle.Dispose();
                    this.DelayWaitHandle.Dispose();
                }
            }

            ~TaskDelayer()
            {
                this.Dispose(false);
            }

            public void Dispose()
            {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            private bool _IsDisposed;

            public bool IsDisposed
            {
                get
                {
                    return this._IsDisposed;
                }
            }

            private readonly Action _Task;

            public Action Task
            {
                get
                {
                    return this._Task;
                }
            }

            private readonly TimeSpan _MinDelay;

            public TimeSpan MinDelay
            {
                get
                {
                    return this._MinDelay;
                }
            }

            private readonly TimeSpan _MaxDelay;

            public TimeSpan MaxDelay
            {
                get
                {
                    return this._MaxDelay;
                }
            }

            private readonly TimeSpan _InactivityTime;

            public TimeSpan InactivityTime
            {
                get
                {
                    return this._InactivityTime;
                }
            }

            private bool IsTaskPending = false;
            private bool IsInstantSet = false;
            private TimeSpan FirstActivityTime;
            private TimeSpan LastActivityTime;

            private static readonly TimeSpan TimeEpsilon = TimeSpan.FromMilliseconds(30);

            private readonly Stopwatch StopWatch = new Stopwatch();
            private readonly object WaitLockObject = new object();
            private readonly System.Threading.EventWaitHandle TaskWaitHandle = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset);
            private readonly System.Threading.EventWaitHandle DelayWaitHandle = new System.Threading.EventWaitHandle(false, System.Threading.EventResetMode.AutoReset);
            private readonly System.Threading.Thread TaskThread;
        }

        public enum TaskDelayerRunningMode
        {
            Instant,
            Delayed
        }
    }
}
