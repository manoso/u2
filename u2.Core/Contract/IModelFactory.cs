using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface IModelFactory
    {
        Task<IEnumerable<T>> Get<T>(Expression<Func<T, bool>> condition = null) where T : class, new();
    }
}
