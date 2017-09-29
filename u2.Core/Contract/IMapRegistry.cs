using System;

namespace u2.Core.Contract
{
    public interface IMapRegistry
    {
        IRoot Root { get; }
        SimpleMap<T> Copy<T>() where T : class, new();
        TypeMap<T> Register<T>() where T : class, new();
        TypeMap<T> For<T>() where T : class, new();
        TypeMap For(Type type);
        Type GetType(string contentType);
        bool Has(Type type);
        TypeMap this[Type type] { get; }
    }
}