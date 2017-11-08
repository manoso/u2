﻿using NSubstitute;
using NUnit.Framework;
using u2.Core.Contract;
using u2.Test;

namespace u2.Umbraco.Test
{
    [TestFixture]
    public class UmbracoQueryFactoryTest
    {
        [Test]
        public void Create_media_success()
        {
            var root = new TestRoot {Id = 1};
            var factory = new UmbracoQueryFactory(root);
            var task = Substitute.For<IMapTask<TestMedia>>();

            var query = factory.Create(task);

            Assert.That(query is MediaQuery<TestMedia>);
        }

        [Test]
        public void Create_content_success()
        {
            var root = new TestRoot { Id = 1 };
            var factory = new UmbracoQueryFactory(root);
            var task = Substitute.For<IMapTask<TestItem>>();

            var query = factory.Create(task);

            Assert.That(query is ContentQuery<TestItem>);
        }
    }
}