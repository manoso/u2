using u2.Umbraco.Contract;

namespace u2.Test
{
    public class TestUmbracoConfig : IUmbracoConfig
    {
        public string ExamineSearcher { get; } = "ExternalSearcher";
    }
}