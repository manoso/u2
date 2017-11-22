using u2.Config.Contract;
using u2.Core.Contract;
using u2.Umbraco;
using u2.Demo.Data;
using u2.Umbraco.DataType;


namespace u2.Demo.Api.Ninject
{
    public class MapConfig : IMapConfig
    {
        public void Config(IRegistry registry)
        {
            registry.Register<Site>()
                .Map(root => root.Hosts, null, x => x.Split<string>());
            registry.Register<View>()
                .MatchMany(view => view.Blocks);
            registry.Register<Block>()
                .MatchAction<Label>((block, labels) => block.Labels = labels)
                .MatchMany(block => block.Images)
                .MapFunction(block => block.ImageList, mapFunc: x => x.NestedContents<ImageList>());
            registry.Register<Media>();
            registry.Register<ImageList>()
                .MatchMany(list => list.Images);
        }
    }
}
