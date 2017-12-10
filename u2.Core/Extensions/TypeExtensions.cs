namespace u2.Core.Extensions
{
    /// <summary>
    /// Extension methods related to Type.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Cast the source object to type T.
        /// If source is null, default T is returned.
        /// </summary>
        /// <typeparam name="T">The type to cast to.</typeparam>
        /// <param name="source">The source object.</param>
        /// <returns></returns>
        public static T To<T>(this object source) => source == null ? default(T) : (T) source;
    }
}
