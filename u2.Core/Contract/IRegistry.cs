namespace u2.Core.Contract
{
    public interface IRegistry
    {
        ITypeMap<T> Register<T>(string key = null) where T : class, new();
    }
}