using System.Collections.Generic;

namespace u2.Core.Contract
{
    public interface IRoot : ICmsModel<int>
    {
        string CacheName { get; set; }
        IEnumerable<string> Hosts { get; set; }
    }
}