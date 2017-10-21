using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using u2.Core.Contract;
using u2.Test;

namespace u2.Core.Test
{
    [TestFixture]
    public class RegistryTest
    {
        [Test]
        public void Register_no_key_received()
        {
            var mapRegistry = Substitute.For<IMapRegistry>();
            var mapper = Substitute.For<IMapper>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();
            var cache = Substitute.For<ICache>();
            var queryFactory = Substitute.For<IQueryFactory>();
            var cmsFetcher = Substitute.For<ICmsFetcher>();

            var registry = new Registry(mapRegistry, mapper, cacheRegistry, cache, queryFactory, cmsFetcher);

            registry.Register<TestItem>();

            mapRegistry.Register<TestItem>().Received(1);
            cacheRegistry.Add(Arg.Any<Func<Task<IEnumerable<TestItem>>>>(), typeof(TestItem).FullName).Received(1);
        }

        [Test]
        public void Register_key_received()
        {
            var mapRegistry = Substitute.For<IMapRegistry>();
            var mapper = Substitute.For<IMapper>();
            var cacheRegistry = Substitute.For<ICacheRegistry>();
            var cache = Substitute.For<ICache>();
            var queryFactory = Substitute.For<IQueryFactory>();
            var cmsFetcher = Substitute.For<ICmsFetcher>();

            var registry = new Registry(mapRegistry, mapper, cacheRegistry, cache, queryFactory, cmsFetcher);

            registry.Register<TestItem>("test");

            mapRegistry.Register<TestItem>().Received(1);
            cacheRegistry.Add(Arg.Any<Func<Task<IEnumerable<TestItem>>>>(), "test").Received(1);
        }
    }
}
