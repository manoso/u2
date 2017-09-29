using System;

namespace u2.Core.Contract
{
    public interface IContent
    {
        bool Has(string alias);
        string Alias { get; }
        T Get<T>(string alias);
        object Get(Type type, string alias);
    }
}
