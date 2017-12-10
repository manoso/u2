using System;
using System.Collections.Generic;

namespace u2.Core.Contract
{
    /// <summary>
    /// Metadata type to define a property mapping match a CMS content field to cached instances of another model type. 
    /// </summary>
    public interface IModelMap
    {
        /// <summary>
        /// Get the CMS field alias
        /// </summary>
        string Alias { get; }

        /// <summary>
        /// Get and set the field string value to model instance matching function.
        /// </summary>
        Func<object, string, bool> IsMatch { get; set; }

        /// <summary>
        /// Get if the model type is IEnumerable or not.
        /// </summary>
        bool IsMany { get; }

        /// <summary>
        /// Get the model type.
        /// </summary>
        Type ModelType { get; }

        /// <summary>
        /// Get and set the action of setting the model property.
        /// </summary>
        Action<object, object> SetModel { get; set; }

        /// <summary>
        /// To match instances from source with string value from keys using the IsMatch func and set it to the property this model map is defined for.
        /// </summary>
        /// <param name="target">The instance of the model type which this model map is defined for.</param>
        /// <param name="keys">A collectin of field string values.</param>
        /// <param name="source">A collection of cached model instances to match from.</param>
        void Match(object target, IEnumerable<string> keys, IEnumerable<object> source);

        /// <summary>
        /// To match a instance from source with the string value from key using the IsMatch func and set it to the property this model map is defined for.
        /// </summary>
        /// <param name="target">The instance of the model type which this model map is defined for.</param>
        /// <param name="key">The field string value.</param>
        /// <param name="source">A collection of cached model instances to match from.</param>
        void Match(object target, string key, IEnumerable<object> source);

        /// <summary>
        /// To match a instance from source with the string value from key using the default IsMatch func and set it to the property this model map is defined for.
        /// </summary>
        /// <param name="target">The instance of the model type which this model map is defined for.</param>
        /// <param name="source">A collection of cached model instances to match from.</param>
        void Match(object target, IEnumerable<object> source);
    }
}