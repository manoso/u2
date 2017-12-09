using System;
using System.Collections.Generic;

namespace u2.Core.Contract
{
    /// <summary>
    /// Metadata type to define an action to run after fields with the given aliases are read from the underlying CMS content.
    /// </summary>
    public interface IGroupAction
    {
        /// <summary>
        /// Get and set the action to run.
        /// </summary>
        Action<object, IList<object>> Action { get; set; }

        /// <summary>
        /// Get and set the list of aliases of the fields.
        /// </summary>
        IList<string> Aliases { get; set; }
    }
}