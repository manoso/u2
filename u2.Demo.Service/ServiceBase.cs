using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using u2.Core;
using u2.Core.Contract;
using u2.Demo.Data;

namespace u2.Demo.Service
{
    public abstract class ServiceBase
    {
        [Inject]
        public ICache Cache { get; set; }

        [Inject]
        public ICacheRegistry CacheRegistry { get; set; }

    }
}
