namespace u2.Core.Contract
{
    public interface ICacheStore
    {
        object Get(string key);
        void Save(string key, object item);
        void Clear(string key);
        bool Has(string key);
    }
}