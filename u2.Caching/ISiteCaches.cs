using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Caching
{
    public interface ISiteCaches
    {
        ICache Default { get; }

        ICache Get(IRoot root);
        void Refresh(IRoot root = null);
        Task RefreshAsync(IRoot root = null);
    }
}