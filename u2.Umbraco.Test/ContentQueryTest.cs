using NSubstitute;
using NUnit.Framework;
using u2.Core.Contract;
using u2.Test;

namespace u2.Umbraco.Test
{
    [TestFixture]
    public class ContentQueryTest
    {
        [Test]
        public void Query()
        {
            const string rootId = "1";
            var query = new ContentQuery<TestItem> (rootId, "testItem");
            var result = query.Query;

            Assert.That(result, Is.EqualTo(@"+__IndexType:content +__Path:\-1,1* +__NodeTypeAlias:testItem"));
        }

        [Test]
        public void Query_root()
        {
            const string rootId = "1";
            var query = new ContentQuery<TestRoot>(rootId, "testRoot");
            var result = query.Query;

            Assert.That(result, Is.EqualTo(@"+__IndexType:content +__Path:\-1,* +__NodeTypeAlias:testRoot"));
        }
    }
}
