using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface IDataPool
    {
        Task<IEnumerable<T>> GetAsync<T>() where T : class, new();
        Task<T> GetAsync<T>(string key);
        Task<ILookup<string, T>> GetLookupAsync<T>(ILookupParameter<T> lookupParameter) where T : class, new();
    }
}
