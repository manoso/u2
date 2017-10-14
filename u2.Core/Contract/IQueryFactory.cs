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
        ICmsQuery Create(IMapTask mapTask);
        ICmsQuery<T> Create<T>(IMapTask<T> mapTask) where T : class, new();
    }
}
