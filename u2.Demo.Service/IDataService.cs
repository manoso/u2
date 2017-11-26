using System.Collections.Generic;
using System.Threading.Tasks;
using u2.Demo.Common.Ninject;
using u2.Demo.Data;

namespace u2.Demo.Service
{
    public interface IDataService : ITransientScope
    {
        IEnumerable<T> Get<T>();
        Task<IEnumerable<T>> GetAsync<T>();
    }
}