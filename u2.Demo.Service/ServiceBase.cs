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
        private static readonly OnceToken Once = new OnceToken();

        protected ServiceBase(IRegistry registry)
        {
            Once.Lock(() =>
            {
                registry.Register<View>()
                    .Fit(x => x.Blocks);
                registry.Register<Block>();
            });
        }
    }
}
