using System;
using System.Threading.Tasks;

namespace Ks.Common
{
    public class TaskDeduplicator
    {
        public TaskDeduplicator(Func<Task> Task)
        {
            this.Task = Task;
        }

        public async void Run()
        {
            if (this.IsTaskGoingOn)
            {
                this.IsTaskPending = true;
                return;
            }

            this.IsTaskGoingOn = true;
            do
            {
                this.IsTaskPending = false;
                await this.Task.Invoke();
            } while (this.IsTaskPending);
            this.IsTaskGoingOn = false;

            this.TaskDoneTaskSource?.SetResult(null);
            this.TaskDoneTaskSource = null;
        }

        public Task WaitTillDoneAsync()
        {
            if (!this.IsTaskGoingOn)
            {
                return System.Threading.Tasks.Task.FromResult<Void>(null);
            }

            this.TaskDoneTaskSource = new TaskCompletionSource<Void>();
            return this.TaskDoneTaskSource.Task;
        }

        private TaskCompletionSource<Void> TaskDoneTaskSource;
        private bool IsTaskGoingOn;
        private bool IsTaskPending;
        private readonly Func<Task> Task;
    }
}
