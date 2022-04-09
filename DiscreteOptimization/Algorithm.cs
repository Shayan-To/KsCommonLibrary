using System;
using System.Diagnostics;

namespace Ks.Common
{
    public abstract class Algorithm
    {
        public Algorithm()
        {
            this.ResetInternal();
        }

        protected abstract bool Step();
        protected virtual void OnStepped()
        {
            this.Stepped?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnStarting() { }
        protected virtual void OnPausing() { }
        protected virtual void OnPaused() { }
        protected virtual void OnResuming() { }
        protected virtual void OnBroken() { }
        protected virtual void OnFinished() { }
        protected virtual void OnReset() { }

        private void Run()
        {
            this.Stopwatch.Start();
            this.State = AlgorithmState.Running;

            try
            {
                while (true)
                {
                    this.StepNumber += 1;

                    bool shouldContinue;
                    try
                    {
                        shouldContinue = this.Step();
                    }
                    catch (Exception ex)
                    {
                        this.Exception = ex;
                        this.State = AlgorithmState.Broken;
                        this.OnBroken();
                        break;
                    }

                    this.OnStepped();

                    if (!shouldContinue)
                    {
                        this.State = AlgorithmState.Finished;
                        this.OnFinished();
                        break;
                    }

                    var terminationStatus = this.TerminationPredicate.Invoke();
                    if (terminationStatus.HasValue)
                    {
                        this.State = AlgorithmState.Finished;
                        this.TerminationStatus = terminationStatus;
                        this.OnFinished();
                        break;
                    }

                    if (this.State == AlgorithmState.Pausing)
                    {
                        this.State = AlgorithmState.Paused;
                        this.OnPaused();
                        break;
                    }
                }
                this.Stopwatch.Stop();
            }
            finally
            {
                if (this.State == AlgorithmState.Running)
                {
                    this.Stopwatch.Stop();
                    this.State = AlgorithmState.Broken;
                    this.Exception = new Exception("Unexpected running status on exit. (Probably because of unhandled exception in handlers.)");
                }
            }
        }

        public void Start()
        {
            Verify.True(this.State == AlgorithmState.Reset, $"Already started. Use {nameof(this.Resume)} instead.");

            this.OnStarting();
            this.Run();
        }

        public void Pause()
        {
            Verify.True(this.State == AlgorithmState.Running, $"Should be running to be able to pause.");
            this.State = AlgorithmState.Pausing;
            this.OnPausing();
        }

        public void Resume()
        {
            Verify.True(this.State == AlgorithmState.Paused, $"Should be paused to be able to resume.");
            this.OnResuming();
            this.Run();
        }

        public void Reset()
        {
            Verify.False((this.State & AlgorithmState.Running) == AlgorithmState.Running, $"Should not be running to be able to reset. Use {nameof(this.Pause)} first.");
            this.ResetInternal();
            this.OnReset();
        }

        private void ResetInternal()
        {
            this.State = AlgorithmState.Reset;
            this.StepNumber = -1;
            this.Exception = null;
            this.Stopwatch.Reset();
        }

        public event EventHandler Stepped;

        public Func<int?> TerminationPredicate { get; set; }
        public AlgorithmState State { get; private set; }
        public Exception Exception { get; private set; }
        public int? TerminationStatus { get; private set; }
        public int StepNumber { get; private set; }
        public TimeSpan ElapsedTime => this.Stopwatch.Elapsed;

        private readonly Stopwatch Stopwatch = new Stopwatch();
    }

    public enum AlgorithmState
    {
        Reset = 0,
        Running = 1,
        Pausing = Running | Paused,
        Paused = 2,
        Broken = 8,
        Finished = 4
    }
}
