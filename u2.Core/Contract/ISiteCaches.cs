using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface ISiteCaches
    {
        ICache Default { get; }

        ICache this[IRoot root]
        {
            get;
            set;
        }

        void Refresh(IRoot root = null);
        Task RefreshAsync(IRoot root = null);
    }
}