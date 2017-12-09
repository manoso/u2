namespace u2.Fixture.Contract
{
    /// <summary>
    /// Base type to build up a registry.
    /// </summary>
    /// <typeparam name="T">Indicates the registry type.</typeparam>
    public interface IBuild<in T>
    {
        /// <summary>
        /// Build up the passed in registry. 
        /// </summary>
        /// <param name="registry">The registry instance to be built.</param>
        void Setup(T registry);
    }
}
