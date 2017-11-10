using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Ninject.Extensions.Conventions;
using Ninject.Modules;
using u2.Demo.Common;

namespace u2.Demo.Api.Ninject
{
    public class WebModule : NinjectModule
    {
        public override void Load()
        {
            var assemblies =
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(x => x.FullName.StartsWith("Cinema"))
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

            //Bind<IMappingEngine>().ToMethod(x => AutoMapperInstance.Current);
        }
    }
}