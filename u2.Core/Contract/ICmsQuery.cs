using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface ICmsQuery
    {
        string Query { get; }
    }

    public interface ICmsQuery<T> : ICmsQuery
        where T : class, new()
    {
        Expression<Func<T, bool>> Condition { get; }
    }
}
