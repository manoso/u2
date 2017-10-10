using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Test
{
    public class TestQueryFactory : IQueryFactory
    {
        public ICmsQuery Create(ITypeMap typeMap)
        {
            throw new NotImplementedException();
        }

        public ICmsQuery<T> Create<T>(ITypeMap<T> typeMap) where T : class, new()
        {
            throw new NotImplementedException();
        }
    }
}
