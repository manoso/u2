using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace u2.Core
{
    public abstract class RunOnce<T>
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private Task<bool> _task;

        protected abstract Func<bool> CanRun { get; }
        protected abstract Action Reset { get; }
        protected abstract Func<T, Task<IDictionary<string, object>>> RunTask { get; }

        protected async Task RunAsync(T parameter, Action<IDictionary<string, object>> done = null)
        {
            TaskCompletionSource<bool> taskCompletion = null;
            await _semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                if (CanRun())
                {
                    taskCompletion = new TaskCompletionSource<bool>();
                    _task = taskCompletion.Task;
                    Reset();
                }
            }
            finally
            {
                _semaphore.Release();
            }

            if (taskCompletion != null)
            {
                var result = await RunTask(parameter).ConfigureAwait(false);
                done?.Invoke(result);
                taskCompletion.SetResult(true);
            }

            await _task.ConfigureAwait(false);
        }
    }
}
