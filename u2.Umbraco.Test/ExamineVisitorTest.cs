using System;
using System.Linq.Expressions;
using NUnit.Framework;
using u2.Test;

namespace u2.Umbraco.Test
{
    /// <summary>
    /// Summary description for ExamineVisitorTest
    /// </summary>
    [TestFixture]
    public class ExamineVisitorTest
    {
        [Test]
        public void Visit_int_Sucess()
        {
            Expression<Func<TestItem, bool>> condition = x => x.ItemId == 1;
            var visitor = new ExamineVisitor();
            visitor.Visit(condition);
            var query = visitor.Query;

            Assert.That(query, Is.EqualTo(@"ItemId:(1)"));
        }

        [Test]
        public void Visit_bool_Sucess()
        {
            Expression<Func<TestItem, bool>> condition = x => x.OnSale;
            var visitor = new ExamineVisitor();
            visitor.Visit(condition);
            var query = visitor.Query;

            Assert.That(query, Is.EqualTo(@"OnSale:(1)"));
        }

        [Test]
        public void Visit_And_Sucess()
        {
            Expression<Func<TestItem, bool>> condition = x => !x.OnSale && x.ItemId == 1;
            var visitor = new ExamineVisitor();
            visitor.Visit(condition);
            var query = visitor.Query;

            Assert.That(query, Is.EqualTo(@"+(-(OnSale:(1))) +(ItemId:(1))"));
        }

        [Test]
        public void Visit_Or_Sucess()
        {
            Expression<Func<TestItem, bool>> condition = x => x.OnSale || x.ItemId == 1;
            var visitor = new ExamineVisitor();
            visitor.Visit(condition);
            var query = visitor.Query;

            Assert.That(query, Is.EqualTo(@"(OnSale:(1)) (ItemId:(1))"));
        }
    }
}
