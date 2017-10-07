using System;

namespace u2.Core.Contract
{
    public interface IMapRegistry
    {
        IRoot Root { get; }
        ISimpleMap<T> Copy<T>() where T : class, new();
        ITypeMap<T> Register<T>() where T : class, new();
        ITypeMap For<T>() where T : class, new();
        ITypeMap For(Type type);
        Type GetType(string contentType);
        bool Has(Type type);
        ITypeMap this[Type type] { get; }
    }
}