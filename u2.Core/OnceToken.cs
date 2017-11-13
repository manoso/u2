using System;
using System.Threading;
using System.Threading.Tasks;

namespace u2.Core
{
    public class OnceToken
    {
        private readonly SemaphoreSlim _locker;
        private bool _isRun;

        public OnceToken()
        {
            _locker = new SemaphoreSlim(1, 1);
        }

        public async Task LockAsync(Action action)
        {
            await _locker.WaitAsync().ConfigureAwait(false);
            if (!_isRun)
            {
                action();
                _isRun = true;
            }
            _locker.Release();
        }

        public void Lock(Action action)
        {
            LockAsync(action).Wait();
        }
    }
}