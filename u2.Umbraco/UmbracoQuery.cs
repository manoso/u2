using System;
using System.Linq.Expressions;
using u2.Core.Contract;

namespace u2.Umbraco
{
    public abstract class UmbracoQuery<T> : ICmsQuery<T>
        where T : class, new()
    {

        public IRoot Root { get; set; }
        public string Alias { get; set; }

        public Expression<Func<T, bool>> Condition { get; set; }

        public abstract string Query { get; }

        protected string RawQuery
        {
            get
            {
                string query = null;
                if (Condition != null)
                {
                    var visitor = new ExamineVisitor();
                    visitor.Visit(Condition);
                    query = visitor.Query;
                }
                return query;
            }
        }
    }
}