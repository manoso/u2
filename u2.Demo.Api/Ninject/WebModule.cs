using System;
using System.Linq;
using System.Web;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using u2.Caching;
using u2.Core;
using u2.Core.Contract;
using u2.Core.Extensions;
using u2.Demo.Common.Ninject;
using u2.Demo.Data;
using u2.Umbraco;

namespace u2.Demo.Api.Ninject
{
    public class WebModule : NinjectModule
    {
        public override void Load()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => x.FullName.StartsWith("u2"))
                .ToArray();
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

            Bind<IMapRegistry>().To<MapRegistry>().InSingletonScope();
            Bind<IMapper>().To<Mapper>().InSingletonScope();
            Bind<ICacheRegistry>().To<CacheRegistry>().InSingletonScope();
            Bind<IQueryFactory>().To<UmbracoQueryFactory>().InSingletonScope();
            Bind<ICmsFetcher>().To<UmbracoFetcher>().InSingletonScope();
            Bind<IRegistry>().To<Registry>().InSingletonScope();
            Bind<ICacheStore>().To<CacheStore>();
            Bind<ICache>().To<Cache>().Named("default")
                .WithConstructorArgument("root", x => null);
            Bind<ISiteCaches>().To<SiteCaches>().InSingletonScope()
                .WithConstructorArgument("cache", x => x.Kernel.Get<ICache>("default"));

            var rego = Kernel?.Get<IRegistry>();
            rego?.Register<Site>()
                .Map(site => site.Hosts, null, x => x.Split<string>());

            Bind<IRoot>().ToMethod(context =>
            {
                var host = HttpContext.Current.Request.Url.Host;
                var cache = context.Kernel.Get<ISiteCaches>().Default;
                var sites = cache.Fetch<Site>().AsList();
                return sites.FirstOrDefault(site => site.Hosts.Contains(host));
            });
            Bind<ICache>().ToMethod(context =>
            {
                var caches = context.Kernel.Get<ISiteCaches>();
                var root = context.Kernel.Get<IRoot>();

                return caches[root];
            });

            //Bind<IMappingEngine>().ToMethod(x => AutoMapperInstance.Current);
        }

        
    }
}