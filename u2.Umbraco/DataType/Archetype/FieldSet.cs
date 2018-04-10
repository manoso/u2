using System.Collections.Generic;

namespace u2.Umbraco.DataType.Archetype
{
    /// <summary>
    /// Archetype fieldset
    /// </summary>
    public class FieldSet
    {
        /// <summary>
        /// Get or set the list of properties.
        /// </summary>
        public IList<Property> Properties { get; set; }
    }
}