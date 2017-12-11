using System.Collections.Generic;

namespace u2.Umbraco.DataType.Archetype
{
    /// <summary>
    /// Archetype model
    /// </summary>
    public class Model
    { 
        /// <summary>
        /// Get or set the list of fieldsets.
        /// </summary>
        public IList<FieldSet> FieldSets { get; set; }
    }
}