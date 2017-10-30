using System.Collections.Generic;
using NUnit.Framework;

namespace u2.Umbraco.Test
{
    [TestFixture]
    public class UmbracoContentTest
    {
        [Test]
        public void Get_by_alias()
        {
            var fields = new Dictionary<string, string>
            {
                { "Id", "1" },
                { "key", "item key" },
                { "name", "item name" }
            };

            var content = new UmbracoContent(fields);

            var id = content.Get<int>("id");
            var key = content.Get<string>("key");
            var name = content.Get<string>("name");
            Assert.That(id, Is.EqualTo(1));
            Assert.That(key, Is.EqualTo("item key"));
            Assert.That(name, Is.EqualTo("item name"));
        }

        [Test]
        public void Has_success()
        {
            var fields = new Dictionary<string, string>
            {
                { "Id", "1" },
                { "key", "item key" },
                { "name", "item name" }
            };

            var content = new UmbracoContent(fields);

            Assert.That(content.Has("id"));
            Assert.That(content.Has("key"));
            Assert.That(content.Has("name"));
            Assert.That(!content.Has("fake"));
        }
    }
}