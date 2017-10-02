using u2.Core.Contract;

namespace u2.Umbraco
{
    public class MediaQuery : UmbracoQuery
    {
        private const string MediaFormat = "+__IndexType:media";

        public override string Query => MediaFormat;
    }

    public class MediaQuery<T> : UmbracoQuery<T>
        where T : class, new()
    {
        private const string MediaFormat = "+__IndexType:media{0}";

        public override string Query => string.Format(MediaFormat, RawQuery);
    }
}