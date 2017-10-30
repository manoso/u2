using u2.Core.Contract;

namespace u2.Umbraco
{
    public class ContentQuery<T> : UmbracoQuery<T>
        where T : class, new()
    {
        private const string ContentFormat = @"+__IndexType:content +__Path:\-1,{0}* +__NodeTypeAlias:{1}{2}";
        private readonly string _rootId;

        public override string Query
        {
            get
            {
                var raw = RawQuery;
                return string.Format(ContentFormat, 
                    _rootId,
                    Alias,
                    raw == null ? string.Empty : $" +({raw})");
            }
        }

        public ContentQuery(string rootId, string alias)
        {
            Alias = alias;
            _rootId = typeof(IRoot).IsAssignableFrom(typeof(T)) ? string.Empty : rootId;
        }
    }
}