using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface ICacheFetcher
    { 
        Task<IEnumerable<T>> FetchAsync<T>(string key = null);
        //Task<T> FetchAsync<T>(string key);
        //Task<ILookup<string, T>> FetchLookupAsync<T>(ILookupParameter<T> lookupParameter);
    }
}