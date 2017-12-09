using System;

namespace u2.Core.Contract
{
    /// <summary>
    /// All CMS types need to inherit from this base type to have a unique CMS key.
    /// </summary>
    public interface ICmsKey
    {
        /// <summary>
        /// Get the unique CMS key.
        /// </summary>
        Guid Key { get; }
    }
}