using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface IQueryFactory
    {
        ICmsQuery Create(TypeMap typeMap);
        ICmsQuery<T> Create<T>(TypeMap<T> typeMap) where T : class, new();
    }
}
