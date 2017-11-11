using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using u2.Caching;
using u2.Core;
using u2.Core.Contract;
using u2.Demo.Common.Ninject;
using u2.Umbraco;
using Mapper = AutoMapper.Mapper;

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
            Bind<IMapper>().To<Core.Mapper>().InSingletonScope();
            Bind<ICacheRegistry>().To<CacheRegistry>().InSingletonScope();
            Bind<IQueryFactory>().To<UmbracoQueryFactory>().InSingletonScope();
            Bind<ICmsFetcher>().To<UmbracoFetcher>().InSingletonScope();
            Bind<IRegistry>().To<Registry>().InSingletonScope();
            Bind<ISiteCaches>().To<SiteCaches>().InSingletonScope();

            Bind<ICacheStore>().To<CacheStore>();
            Bind<IRoot>().ToMethod(context => null);
            Bind<ICache>().ToMethod(context =>
            {

                var caches = context.Kernel.Get<ISiteCaches>();

                return null;
            });

            //Bind<IMappingEngine>().ToMethod(x => AutoMapperInstance.Current);
        }

        
    }
}