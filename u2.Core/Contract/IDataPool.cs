using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface IDataPool
    {
        Task<IEnumerable<T>> GetAsync<T>() where T : class, new();
        Task<T> GethAsync<T>(string key) where T : class, new();
        Task<ILookup<string, T>> GetLookupAsync<T>(ILookupParameter<T> lookupParameter) where T : class, new();
    }
}
