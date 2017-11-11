using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface ISiteCaches
    {
        void Refresh(string site = null);
        Task RefreshAsync(string site = null);
    }
}