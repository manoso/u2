using u2.Config.Contract;
using u2.Core.Contract;
using u2.Umbraco;
using u2.Demo.Data;


namespace u2.Demo.Api.Ninject
{
    public class MapConfig : IMapConfig
    {
        public void Config(IRegistry registry)
        {
            registry.Register<Site>()
                .Map(root => root.Hosts, null, x => x.Split<string>());
            registry.Register<View>()
                .Fit(x => x.Blocks);
            registry.Register<Block>();
        }
    }
}
