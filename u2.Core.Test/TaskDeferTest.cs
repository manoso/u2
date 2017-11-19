
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using u2.Test;

namespace u2.Core.Test
{
    [TestFixture]
    public class TaskDeferTest
    {
        [Test]
        public void Attach_success()
        {
            var defer = new TaskDefer();

            var result = defer.Attach("test", async (x, s) =>
            {
                await Task.Run(() => {});
            });

            Assert.That(defer, Is.EqualTo(result));
            Assert.That(defer.Maps.Count, Is.EqualTo(1));
            Assert.That(defer.Maps.First() is MapItem<object, string>);
        }

        [Test]
        public void Attach_type_success()
        {
            var defer = new TaskDefer<TestItem>();

            var result = defer.Attach<int>("test", async (x, s) =>
            {
                await Task.Run(() => { x.ItemId = s; });
            });

            Assert.That(defer, Is.EqualTo(result));
            Assert.That(defer.Maps.Count, Is.EqualTo(1));
            Assert.That(defer.Maps.First() is MapItem<TestItem, int>);
        }
    }
}
