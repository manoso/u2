namespace u2.Umbraco
{
    public class ContentQuery : UmbracoQuery
    {
        private const string ContentFormat = @"+__IndexType:content +__Path:\-1,{0}* +__NodeTypeAlias:{1}";

        public override string Query => string.Format(ContentFormat, Root?.Id.ToString() ?? string.Empty, Alias);
    }

    public class ContentQuery<T> : UmbracoQuery<T>
        where T : class, new()
    {
        private const string ContentFormat = @"+__IndexType:content +__Path:\-1,{0}* +__NodeTypeAlias:{1}{2}";

        public override string Query
        {
            get
            {
                var raw = RawQuery;
                return string.Format(ContentFormat,
                    Root?.Id.ToString() ?? string.Empty,
                    Alias,
                    raw == null ? string.Empty : $" +({raw})");
            }
        }
    }
}