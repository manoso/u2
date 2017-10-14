namespace u2.Core.Contract
{
    public interface IRegistry
    {
        IMapTask<T> Register<T>(string key = null) where T : class, new();
    }
}