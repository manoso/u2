using System;

namespace u2.Core.Contract
{
    public interface IMapRegistry
    {
        IBaseTask<T> Copy<T>() where T : class, new();
        IMapTask<T> Register<T>() where T : class, new();
        IMapTask For<T>() where T : class, new();
        IMapTask For(Type type);
        Type GetType(string contentType);
        bool Has(Type type);
        IMapTask this[Type type] { get; }
    }
}