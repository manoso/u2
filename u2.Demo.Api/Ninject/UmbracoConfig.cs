using u2.Umbraco.Contract;

namespace u2.Demo.Api.Ninject
{
    public class UmbracoConfig : IUmbracoConfig
    {
        public string Searcher { get; } = "ExternalSearcher";
    }
}