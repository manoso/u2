using System.Collections.Generic;
using System.Threading.Tasks;
using u2.Demo.Common.Ninject;
using u2.Demo.Data;

namespace u2.Demo.Provider
{
    public interface IDataProvider : ISingletonScope
    {
        Task<IEnumerable<Label>> GetLabels();
    }
}