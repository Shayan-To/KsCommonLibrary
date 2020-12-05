﻿using System.Threading.Tasks;
using System;

namespace Ks
{
    namespace Common
    {
        public class Timer
        {
            public Timer(Action Callback, TimeSpan Interval)
            {
                this.Callback = Callback;
                this.Interval = Interval;
            }

            public Timer(Action Callback, int IntervalMillis) : this(Callback, TimeSpan.FromMilliseconds(IntervalMillis))
            {
            }

            public async void Start()
            {
                Verify.False(this.IsRunning, "Cannot start an already started timer.");
                this._IsRunning = true;

                if (this.RunAtStart)
                    this.Callback.Invoke();

                do
                {
                    await Task.Delay(this.Interval);
                    if (!this.IsRunning)
                        break;
                    this.Callback.Invoke();
                }
                while (true);
            }

            public void Stop()
            {
                this._IsRunning = false;
            }

            private TimeSpan _Interval;

            public TimeSpan Interval
            {
                get
                {
                    return this._Interval;
                }
                set
                {
                    this._Interval = value;
                }
            }

            private bool _IsRunning;

            public bool IsRunning
            {
                get
                {
                    return this._IsRunning;
                }
                set
                {
                    if (value != this._IsRunning)
                    {
                        if (value)
                            this.Start();
                        else
                            this.Stop();
                    }
                }
            }

            private bool _RunAtStart;

            public bool RunAtStart
            {
                get
                {
                    return this._RunAtStart;
                }
                set
                {
                    this._RunAtStart = value;
                }
            }

            private readonly Action Callback;
        }
    }
}
