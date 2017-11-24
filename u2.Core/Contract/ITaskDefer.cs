using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface ITaskDefer
    {
        IList<IMapItem> Maps { get; }
    }
}