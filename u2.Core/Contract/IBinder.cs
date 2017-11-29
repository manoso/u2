using System;

namespace u2.Core.Contract
{
    /// <summary>
    /// To bind interface to concrete type using the underlying IoC.
    /// Don't impletment if IoC is not used.
    /// </summary>
    public interface IBinder
    {
        /// <summary>
        /// Add an interface to type binding to the underlying IoC.
        /// </summary>
        /// <typeparam name="TContract"></typeparam>
        /// <typeparam name="TImplement"></typeparam>
        /// <param name="isSingleton"></param>
        /// <param name="func"></param>
        void Add<TContract, TImplement>(bool isSingleton = false, Func<TImplement> func = null)
            where TContract : class
            where TImplement : TContract;

        T Get<T>() where T : class;
        string Host { get; }
    }
}
