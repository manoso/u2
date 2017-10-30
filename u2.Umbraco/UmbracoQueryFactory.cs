using u2.Core.Contract;

namespace u2.Umbraco
{
    public class UmbracoQueryFactory : IQueryFactory
    {
        private readonly string _rootId;

        public UmbracoQueryFactory(IRoot root)
        {
            _rootId = root?.Id.ToString();
        }

        public ICmsQuery<T> Create<T>(IMapTask<T> mapTask) where T : class, new()
        {
            var isMedia = typeof(IMedia).IsAssignableFrom(typeof(T));
            return isMedia
                ? new MediaQuery<T>()
                : new ContentQuery<T>(_rootId, mapTask.Alias) as ICmsQuery<T>;
        }
    }
}
