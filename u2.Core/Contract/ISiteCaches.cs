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

        bool Has(IRoot root);
        void Refresh(IRoot root = null);
        Task RefreshAsync(IRoot root = null);
    }
}