using System.Collections.Generic;


namespace u2.Core.Contract
{
    public interface ICmsFetcher
    {
        IEnumerable<IContent> Fetch(ICmsQuery cmsQuery);
    }
}
