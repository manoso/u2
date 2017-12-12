using System;
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
        public void Get_string_value_whitespace_returns_value()
        {
            var fields = new Dictionary<string, string>
            {
                { "Name", " " },
            };

            var content = new UmbracoContent(fields);

            var id = content.Get<string>("name");
            Assert.AreEqual(id, " ");
        }

        [Test]
        public void Get_Guid_value()
        {
            var guid = Guid.NewGuid();
            var fields = new Dictionary<string, string>
            {
                { "Key", guid.ToString() },
            };

            var content = new UmbracoContent(fields);

            var key = content.Get<Guid>("key");
            Assert.That(key, Is.EqualTo(guid));
        }

        [Test]
        public void Get_Guid_wrong_value_returns_default()
        {
            var guid = Guid.NewGuid();
            var fields = new Dictionary<string, string>
            {
                { "Key", guid.ToString().Substring(10) },
            };

            var content = new UmbracoContent(fields);

            var key = content.Get<Guid>("key");
            Assert.That(key, Is.EqualTo(Guid.Empty));
        }

        [Test]
        public void Get_nullable_Guid_value()
        {
            var guid = Guid.NewGuid();
            var fields = new Dictionary<string, string>
            {
                { "Key", guid.ToString() },
            };

            var content = new UmbracoContent(fields);

            var key = content.Get<Guid?>("key");
            Assert.That(key, Is.EqualTo(guid));
        }

        [Test]
        public void Get_nullable_Guid_returns_default()
        {
            var guid = Guid.NewGuid();
            var fields = new Dictionary<string, string>
            {
                { "Key", guid.ToString().Substring(10) },
            };

            var content = new UmbracoContent(fields);

            var key = content.Get<Guid?>("key");
            Assert.That(key, Is.EqualTo(Guid.Empty));
        }

        [Test]
        public void Get_int_value_empty_returns_default()
        {
            var fields = new Dictionary<string, string>
            {
                { "Id", "" },
            };

            var content = new UmbracoContent(fields);

            var id = content.Get<int>("id");
            Assert.That(id, Is.EqualTo(0));
        }

        [Test]
        public void Get_bool_value_returns_true()
        {
            var fields = new Dictionary<string, string>
            {
                { "Yes", "yes" },
                { "One", "1" },
                { "True", "True" },
            };

            var content = new UmbracoContent(fields);

            var yes = content.Get<bool>("yes");
            var one = content.Get<bool>("one");
            var tru = content.Get<bool>("true");
            Assert.That(yes);
            Assert.That(one);
            Assert.That(tru);
        }

        [Test]
        public void Get_bool_value_returns_false()
        {
            var fields = new Dictionary<string, string>
            {
                { "No", "No" },
                { "Zero", "0" },
                { "false", "false" },
            };

            var content = new UmbracoContent(fields);

            var no = content.Get<bool>("no");
            var zero = content.Get<bool>("zero");
            var fals = content.Get<bool>("false");
            Assert.That(!no);
            Assert.That(!zero);
            Assert.That(!fals);
        }

        [Test]
        public void Get_throw_return_default()
        {
            var fields = new Dictionary<string, string>
            {
                { "id", "ab" }
            };

            var content = new UmbracoContent(fields);

            var id = content.Get<int>("id");
            Assert.That(id, Is.EqualTo(0));
        }

        [Test]
        public void Get_Raw()
        {
            var fields = new Dictionary<string, string>
            {
                { "__raw_Id", "10" },
            };

            var content = new UmbracoContent(fields);

            var id = content.Get<int>("id");
            Assert.That(id, Is.EqualTo(10));
        }

        [Test]
        public void Get_Underscore()
        {
            var fields = new Dictionary<string, string>
            {
                { "__flag", "0" },
            };

            var content = new UmbracoContent(fields);

            var flag = content.Get<bool>("flag");
            Assert.That(!flag);
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