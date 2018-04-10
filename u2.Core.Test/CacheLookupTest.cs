using NUnit.Framework;
using u2.Test;

namespace u2.Core.Test
{
    [TestFixture]
    public class CacheLookupTest
    {
        [Test]
        public void CacheKey_get_success()
        {
            var lookup = new CacheLookup<CacheItem>()
                .Add(x => x.LookupKey);

            var key = lookup.CacheKey;
            Assert.That(key, Is.EqualTo("Lookup_CacheItem_LookupKey"));
        }

        [Test]
        public void GetLookupKey_sucess()
        {
            var lookup = new CacheLookup<CacheItem>()
                .Add(x => x.LookupKey)
                .Add(x => x.LookupKeyOther);

            var key = lookup.GetLookupKey(new CacheItem{LookupKey = 1, LookupKeyOther = "other"});
            Assert.That(key, Is.EqualTo("1_other"));
        }

    }
}
