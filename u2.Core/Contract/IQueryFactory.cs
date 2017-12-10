namespace u2.Core.Contract
{
    /// <summary>
    /// Factory type to create CMS queries.
    /// </summary>
    public interface IQueryFactory
    {
        /// <summary>
        /// To create a CMS query.
        /// </summary>
        /// <typeparam name="T">The CMS model type.</typeparam>
        /// <param name="root">The root instance used to create the query.</param>
        /// <param name="mapTask">The map task used to create the query.</param>
        /// <returns></returns>
        ICmsQuery<T> Create<T>(IRoot root, IMapTask<T> mapTask) where T : class, new();
    }
}
