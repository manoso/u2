using System.Collections.Generic;

namespace u2.Core.Contract
{
    /// <summary>
    /// Metadata type to define model type mappings that use instances of other model types.
    /// </summary>
    public interface ITaskDefer
    {
        /// <summary>
        /// Get the list of map items defined for the task defer.
        /// </summary>
        IList<IMapItem> Maps { get; }
    }
}