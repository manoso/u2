using System;
using System.Linq.Expressions;

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
