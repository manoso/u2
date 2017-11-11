using u2.Core.Contract;

namespace u2.Umbraco
{
    public class UmbracoQueryFactory : IQueryFactory
    {
        public ICmsQuery<T> Create<T>(IRoot root, IMapTask<T> mapTask) where T : class, new()
        {
            var isMedia = typeof(IMedia).IsAssignableFrom(typeof(T));
            return isMedia
                ? new MediaQuery<T>()
                : new ContentQuery<T>(root?.Id.ToString(), mapTask.Alias) as ICmsQuery<T>;
        }
    }
}
