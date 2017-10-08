using System;

namespace u2.Core.Contract
{
    public interface IPropertySetter
    {
        string Name { get; set; }
        Action<object, object> Set { get; set; }
    }
}