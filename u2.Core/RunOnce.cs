using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace u2.Core
{
    public abstract class RunOnce<T>
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        protected abstract Task<bool> GetTask(T parameter);
        protected abstract void SetTask(T parameter, Task<bool> task);
        protected abstract Func<T, bool> CanRun { get; }
        protected abstract Action<T> Reset { get; }
        protected abstract Func<T, Task<IDictionary<string, object>>> RunTask { get; }

        protected async Task RunAsync(T parameter, Action<IDictionary<string, object>> done = null)
        {
            TaskCompletionSource<bool> taskCompletion = null;
            await _semaphore.WaitAsync().ConfigureAwait(false);
            try
            {
                if (CanRun(parameter))
                {
                    taskCompletion = new TaskCompletionSource<bool>();
                    SetTask(parameter, taskCompletion.Task);
                    Reset(parameter);
                }

                if (taskCompletion != null)
                {
                    var result = await RunTask(parameter).ConfigureAwait(false);
                    done?.Invoke(result);
                    taskCompletion.SetResult(true);
                }
            }
            finally
            {
                _semaphore.Release();
            }
            await GetTask(parameter).ConfigureAwait(false);
        }
    }
}
