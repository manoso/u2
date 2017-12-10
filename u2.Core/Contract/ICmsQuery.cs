using System;
using System.Linq.Expressions;

namespace u2.Core.Contract
{
    /// <summary>
    /// A base type for Lucene query.
    /// </summary>
    public interface ICmsQuery
    {
        /// <summary>
        /// Get the Lucene search query.
        /// </summary>
        string Query { get; }
    }

    /// <summary>
    /// Using lambda expression to define a Lucene query for a given CMS model type.
    /// </summary>
    /// <typeparam name="T">The model type the Lucene query is for.</typeparam>
    public interface ICmsQuery<T> : ICmsQuery
        where T : class, new()
    {
        /// <summary>
        /// The search criteria in lambda expression.
        /// </summary>
        Expression<Func<T, bool>> Condition { get; }
    }
}
