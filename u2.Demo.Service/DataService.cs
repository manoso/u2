using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using u2.Core.Contract;
using u2.Demo.Data;

namespace u2.Demo.Service
{
    public class DataService : ServiceBase, IDataService
    {
        [Inject, Named("site")]
        public ICache Cache { get; set; }

        public DataService(IRegistry registry)
            : base(registry)
        {
        }

        public async Task<IEnumerable<T>> Get<T>()
        {
            return await Cache.FetchAsync<T>();
        }
    }
}
