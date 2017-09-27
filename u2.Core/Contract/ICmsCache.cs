using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface ICmsCache
    {
        void Register<T>(Func<Task<IEnumerable<T>>> func, int cacheInMins = 0);
    }
}
