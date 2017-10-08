using System.Threading.Tasks;

namespace u2.Core.Contract
{
    public interface IOnceAsync
    {
        Task Run();
    }
}