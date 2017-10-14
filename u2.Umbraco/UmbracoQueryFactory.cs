using u2.Core.Contract;

namespace u2.Umbraco
{
    public class UmbracoQueryFactory : IQueryFactory
    {
        private readonly IRoot _root;

        public UmbracoQueryFactory(IRoot root)
        {
            _root = root;
        }

        public ICmsQuery<T> Create<T>(IMapTask<T> mapTask) where T : class, new()
        {
            var isMedia = typeof(IMedia).IsAssignableFrom(typeof(T));
            return isMedia
                ? new MediaQuery<T>()
                : new ContentQuery<T>
                {
                    Alias = mapTask.Alias,
                    Root = typeof(IRoot).IsAssignableFrom(typeof(T)) ? null : _root
                } as ICmsQuery<T>;
        }

        public ICmsQuery Create(IMapTask mapTask)
        {
            var isMedia = typeof(IMedia).IsAssignableFrom(mapTask.EntityType);
            return isMedia
                ? new MediaQuery()
                : new ContentQuery
                {
                    Alias = mapTask.Alias,
                    Root = typeof(IRoot).IsAssignableFrom(mapTask.EntityType) ? null : _root
                } as ICmsQuery;
        }
    }
}
