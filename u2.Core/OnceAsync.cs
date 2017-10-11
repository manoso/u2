﻿using System;
using System.Threading;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Core
{
    public abstract class OnceAsync
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private Task<bool> _task;

        protected abstract Func<bool> NeedRun { get; }
        protected abstract Action Reset { get; }
        protected abstract Task RunAsync { get; }

        protected async Task Run(Action done)
        {
            TaskCompletionSource<bool> taskCompletion = null;
            await _semaphore.WaitAsync();
            if (NeedRun())
            {
                taskCompletion = new TaskCompletionSource<bool>();
                _task = taskCompletion.Task;
                Reset();
            }
            _semaphore.Release();

            if (taskCompletion != null)
            {
                await RunAsync;
                done();
                taskCompletion.SetResult(true);
            }

            await _task;
        }
    }
}