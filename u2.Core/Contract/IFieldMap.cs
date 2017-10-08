﻿using System;

namespace u2.Core.Contract
{
    public interface IFieldMap
    {
        string Alias { get; set; }
        Type ContentType { get; set; }
        Func<string, object> Converter { get; }
        object Default { get; set; }
        Action<object, object> Defer { get; }
        IPropertySetter Setter { get; set; }

        bool MatchAlias(string alias);
    }

    public interface IFieldMap<out T, TP> : IFieldMap
    {
        Action<T, TP> ActDefer { set; }
        Func<string, TP> Convert { set; }
    }
}