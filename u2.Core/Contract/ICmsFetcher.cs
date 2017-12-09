using System.Collections.Generic;


namespace u2.Core.Contract
{
    /// <summary>
    /// A type to fetch content from CMS.
    /// </summary>
    public interface ICmsFetcher
    {
        /// <summary>
        /// To fetch all the contents that match the criteria specified in the cms query.
        /// </summary>
        /// <param name="cmsQuery">The cms query.</param>
        /// <returns></returns>
        IEnumerable<IContent> Fetch(ICmsQuery cmsQuery);
    }
}
