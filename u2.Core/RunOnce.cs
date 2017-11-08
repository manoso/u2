﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace u2.Core
{
    public abstract class RunOnce
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private Task<bool> _task;

        protected abstract Func<bool> CanRun { get; }
        protected abstract Action Reset { get; }
        protected abstract Task RunTask { get; }

        protected async Task RunAsync(Action done = null)
        {
            TaskCompletionSource<bool> taskCompletion = null;
            await _semaphore.WaitAsync().ConfigureAwait(false);
            if (CanRun())
            {
                taskCompletion = new TaskCompletionSource<bool>();
                _task = taskCompletion.Task;
                Reset();
            }
            _semaphore.Release();

            if (taskCompletion != null)
            {
                await RunTask.ConfigureAwait(false);
                done?.Invoke();
                taskCompletion.SetResult(true);
            }

            await _task.ConfigureAwait(false);
        }
    }
}