using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace u2.Core
{
    public abstract class RunOnce<T>
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        protected abstract Func<T, bool> CanRun { get; }
        protected abstract Action<T> Reset { get; }
        protected abstract Func<T, Task<IDictionary<string, object>>> RunTask { get; }

        protected async Task RunAsync(T parameter, Action<IDictionary<string, object>> done = null)
        {
            try
            {
                await _semaphore.WaitAsync().ConfigureAwait(false);

                if (CanRun(parameter))
                {
                    var result = await RunTask(parameter).ConfigureAwait(false);
                    done?.Invoke(result);
                    Reset(parameter);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
