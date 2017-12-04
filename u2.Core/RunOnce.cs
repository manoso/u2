using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace u2.Core
{
    public abstract class RunOnce<T>
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly IDictionary<T, Task<bool>> _tasks = new Dictionary<T, Task<bool>>();

        protected abstract Func<T, Task<IDictionary<string, object>>> RunTask { get; }

        protected async Task RunAsync(T parameter, Action<IDictionary<string, object>> done = null)
        {
            TaskCompletionSource<bool> taskCompletion = null;
            Task<bool> task;
            try
            {
                await _semaphore.WaitAsync().ConfigureAwait(false);
                if (!_tasks.TryGetValue(parameter, out task))
                {
                    taskCompletion = new TaskCompletionSource<bool>();
                    task = _tasks[parameter] = taskCompletion.Task;
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

            await task.ConfigureAwait(false);
        }
    }
}
