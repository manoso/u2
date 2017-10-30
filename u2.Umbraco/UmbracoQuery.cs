using System;
using System.Linq.Expressions;
using u2.Core.Contract;

namespace u2.Umbraco
{
    public abstract class UmbracoQuery : ICmsQuery
    {
        public string Alias { get; protected set; }

        public abstract string Query { get; }
    }

    public abstract class UmbracoQuery<T> : UmbracoQuery, ICmsQuery<T>
        where T : class, new()
    {

        public Expression<Func<T, bool>> Condition { get; set; }

        protected virtual string RawQuery
        {
            get
            {
                if (Condition != null)
                {
                    var visitor = new ExamineVisitor();
                    visitor.Visit(Condition);
                    return  visitor.Query;
                }
                return null;
            }
        }
    }
}