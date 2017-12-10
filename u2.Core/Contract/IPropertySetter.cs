using System;

namespace u2.Core.Contract
{
    /// <summary>
    /// Metadata type to define a property setter.
    /// </summary>
    public interface IPropertySetter
    {
        /// <summary>
        /// Get the property name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get the setter action.
        /// </summary>
        Action<object, object> Set { get; }
    }
}