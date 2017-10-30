using NUnit.Framework;
using u2.Test;

namespace u2.Umbraco.Test
{
    [TestFixture]
    public class MediaQueryTest
    {
        [Test]
        public void Query()
        {
            var query = new MediaQuery();
            var result = query.Query;

            Assert.That(result, Is.EqualTo(@"+__IndexType:media"));
        }

        [Test]
        public void Query_type()
        {
            var query = new MediaQuery<TestMedia>();
            var result = query.Query;

            Assert.That(result, Is.EqualTo(@"+__IndexType:media"));
        }
    }
}
