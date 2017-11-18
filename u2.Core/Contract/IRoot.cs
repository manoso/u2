using System.Collections.Generic;

namespace u2.Core.Contract
{
    public interface IRoot : ICmsModel<int>
    {
        IEnumerable<string> Hosts { get; set; }
    }
}