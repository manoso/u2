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
        /// <typeparam name="TContract">Interface to bind.</typeparam>
        /// <typeparam name="TImplement">Concrete type to bind to.</typeparam>
        /// <param name="isSingleton">Indicate whether the object instance should be created as singleton.</param>
        /// <param name="func">Function to return the TImplement object.</param>
        void Add<TContract, TImplement>(bool isSingleton = false, Func<TImplement> func = null)
            where TContract : class
            where TImplement : TContract;

        /// <summary>
        /// Get an instance of T from the underlying IoC.
        /// </summary>
        /// <typeparam name="T">The interface or class type.</typeparam>
        /// <returns></returns>
        T Get<T>() where T : class;

        /// <summary>
        /// The host segment of the request url.
        /// </summary>
        string Host { get; }
    }
}
