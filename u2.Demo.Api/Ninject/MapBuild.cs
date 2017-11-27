using u2.Fixture.Contract;
using u2.Core.Contract;
using u2.Demo.Data;
using u2.Umbraco.DataType;
using u2.Umbraco.DataType.Media;


namespace u2.Demo.Api.Ninject
{
    public class MapBuild : IMapBuild
    {
        public void Setup(IRegistry registry)
        {
            registry.Register<Site>()
                .Map(root => root.Hosts, null, x => x.Split<string>());
            registry.Register<View>()
                .MatchMany(view => view.Blocks);
            registry.Register<Block>()
                .MatchAction<Label>((block, labels) => block.Labels = labels)
                .MatchMany(block => block.Images)
                .MapFunction(block => block.ImageList, mapFunc: x => x.ToNestedContents<ImageList>());
            registry.Register<Media>()
                .Map(media => media.UmbracoFile, mapFunc: x=> x.JsonTo<Image>())
                .MapContent((content, media) =>
                {
                    var entity = content;
                });
            registry.Register<ImageList>()
                .MatchMany(list => list.Images);
        }
    }
}
