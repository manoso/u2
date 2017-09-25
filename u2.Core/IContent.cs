using System;

namespace u2.Core
{
    public interface IContent
    {
        bool Has(string alias);
        string Alias { get; }
        T Get<T>(string alias);
        object Get(Type type, string alias);
    }
}
