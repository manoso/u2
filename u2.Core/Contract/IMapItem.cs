using System;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface IMapItem
    {
        string Alias { get; set; }
        Type ContentType { get; set; }
        Func<string, object> Converter { get; }
        object Default { get; set; }
        Func<object, object, Task> Defer { get; }
        IPropertySetter Setter { get; set; }

        bool MatchAlias(string alias);
    }

    public interface IMapItem<out T, TP> : IMapItem
    {
        Func<T, TP, Task> ActDefer { set; }
        Func<string, TP> Convert { set; }
    }
}