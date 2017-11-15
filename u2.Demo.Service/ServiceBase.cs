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

        [Inject]
        public IRegistry Registry { get; set; }

        protected ServiceBase()
        {
            Once.Lock(() =>
            {
                Registry.Register<View>()
                    .Fit(x => x.Blocks);
                Registry.Register<Block>();
            });
        }
    }
}
