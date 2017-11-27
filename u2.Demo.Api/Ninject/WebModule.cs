using System;
using System.Linq;
using System.Web;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using u2.Core.Contract;
using u2.Demo.Common.Ninject;
using u2.Demo.Data;
using u2.Fixture;

namespace u2.Demo.Api.Ninject
{
    public class WebModule : NinjectModule, IBinder
    {
        public override void Load()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.StartsWith("u2"))
                .ToList();
            Kernel.Bind(x => x.From(assemblies)
                .SelectAllClasses()
                .InheritedFrom<ITransientScope>()
                .BindDefaultInterface()
                .Configure(conf => conf.InTransientScope()));
            Kernel.Bind(x => x.From(assemblies)
                .SelectAllClasses()
                .InheritedFrom<ISingletonScope>()
                .BindDefaultInterface()
                .Configure(conf => conf.InSingletonScope()));
            Kernel.Bind(x => x.From(assemblies)
                .SelectAllClasses()
                .InheritedFrom<IThreadScope>()
                .BindDefaultInterface()
                .Configure(conf => conf.InThreadScope()));

            new BindConfig(this).Config<Site, UmbracoConfig, CacheConfig, MapBuild, CacheBuild>();
        }

        public void Add<TContract, TImplement>(bool isSingleton = false, Func<TImplement> func = null)
            where TImplement: TContract
        {
            var binding = func == null
                ? Bind<TContract>().To<TImplement>()
                : Bind<TContract>().ToMethod(context => func());

            if (isSingleton)
                binding.InSingletonScope();
        }

        public T Get<T>() where T : class
        {
            return Kernel?.Get<T>();
        }

        public string Host => HttpContext.Current.Request.Url.Host;
    }

}