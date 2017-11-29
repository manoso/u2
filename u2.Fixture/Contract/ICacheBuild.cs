using u2.Core.Contract;

namespace u2.Fixture.Contract
{
    /// <summary>
    /// Build a registry of caching by types.
    /// Call ICacheRegistry.Add to register types for caching in the Setup implementation.
    /// </summary>
    public interface ICacheBuild : IBuild<ICacheRegistry>
    {
    }
}
