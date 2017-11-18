using System;

namespace u2.Core.Contract
{
    public interface IBinder
    {
        void Add<TContract, TImplement>(bool isSingleton = false, Func<TImplement> func = null)
            where TImplement : TContract;
        T Get<T>() where T : class;
        string Host { get; }
    }
}
