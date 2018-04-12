namespace u2.Core.Contract
{
    /// <summary>
    /// Build a registry of cms content to model type mapping.
    /// Call IRegistry.Register to register cms type mapping in the Setup implementation.
    /// </summary>
    public interface IMapBuild : IBuild<IRegistry>
    {
    }
}
