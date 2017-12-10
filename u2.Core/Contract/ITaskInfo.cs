using System;

namespace u2.Core.Contract
{
    /// <summary>
    /// Metadata type to define a cache task status.
    /// </summary>
    public interface ITaskInfo
    {
        /// <summary>
        /// Get and set the action to save to cache.
        /// </summary>
        Action<string, object> Save { get; set; }

        /// <summary>
        /// Get and set the timestamp.
        /// </summary>
        DateTime Timestamp { get; set; }
    }
}