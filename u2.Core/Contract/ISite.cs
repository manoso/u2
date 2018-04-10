using System.Collections.Generic;

namespace u2.Core.Contract
{
    /// <summary>
    /// CMS root model type
    /// </summary>
    public interface ISite : ICmsModel<int>
    {
        /// <summary>
        /// Get and set the site name. 
        /// Note: Each u2 site has its own cache and must have a unique site name.
        /// </summary>
        string SiteName { get; set; }

        /// <summary>
        /// Get and set a collection of host names fall under the root in CMS.
        /// Host names are like: www.google.com, www.microsoft.com etc.
        /// </summary>
        IEnumerable<string> Hosts { get; set; }
    }
}