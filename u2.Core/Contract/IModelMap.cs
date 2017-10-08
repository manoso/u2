using System;
using System.Collections.Generic;

namespace u2.Core.Contract
{
    public interface IModelMap
    {
        string Alias { get; }
        Func<object, string> GetKey { get; set; }
        bool IsMany { get; }
        Type ModelType { get; }
        Action<object, object> SetModel { get; set; }

        void Match(object target, IEnumerable<string> keys, IEnumerable<object> source);
        void Match(object target, string key, IEnumerable<object> source);
    }
}