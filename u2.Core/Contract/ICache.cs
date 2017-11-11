using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface ICache
    {
        IRoot Root { get; }
        IEnumerable<T> Fetch<T>(string key = null);
        ILookup<string, T> Fetch<T>(ICacheLookup<T> lookup);
        Task<IEnumerable<T>> FetchAsync<T>(string key = null);
        Task<ILookup<string, T>> FetchAsync<T>(ICacheLookup<T> lookup);
        Task ReloadAsync<T>(string key = null);
        Task ReloadAsync();
        void Reload<T>(string key = null);
        void Reload();
    }
}