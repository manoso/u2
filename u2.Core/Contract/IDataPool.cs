using System.Collections.Generic;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface IDataPool
    {
        Task<IEnumerable<T>> GetAsync<T>(string key = null) where T : class, new();
        //Task<ILookup<string, T>> GetLookupAsync<T>(ICacheLookup<T> lookupParameter) where T : class, new();
    }
}
