using System.Collections.Generic;

namespace u2.Core.Contract
{
    /// <summary>
    /// CMS root model type
    /// </summary>
    public interface IRoot : ICmsModel<int>
    {
        /// <summary>
        /// Get and set the cache name. 
        /// Note: Each CMS root has its own cache and must have a unique cache name.
        /// </summary>
        string CacheName { get; set; }

        /// <summary>
        /// Get and set a collection of host names fall under the root in CMS.
        /// Host names are like: www.google.com, www.microsoft.com etc.
        /// </summary>
        IEnumerable<string> Hosts { get; set; }
    }
}