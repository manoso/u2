using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace u2.Core.Extensions
{
    public static class TypeExtensions
    {
        public static TP To<TP>(this object source) => source == null ? default(TP) : (TP) source;
    }
}
