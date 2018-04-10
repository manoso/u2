using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Core.Contract
{
    public interface ISiteCaches
    {
        ICache Default { get; }

        ICache Get(ISite site);
        void Refresh(ISite site = null);
        Task RefreshAsync(ISite site = null);
    }
}