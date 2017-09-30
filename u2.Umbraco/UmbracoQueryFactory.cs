using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using u2.Core;
using u2.Core.Contract;

namespace u2.Umbraco
{
    public class UmbracoQueryFactory : IQueryFactory
    {
        private readonly IRoot _root;

        public UmbracoQueryFactory(IRoot root)
        {
            _root = root;
        }

        public ICmsQuery<T> Create<T>(TypeMap<T> typeMap) where T : class, new()
        {
            var isMedia = typeof(IMedia).IsAssignableFrom(typeof(T));
            return isMedia
                ? new MediaQuery<T>()
                : new ContentQuery<T>
                {
                    Alias = typeMap.Alias,
                    Root = typeof(IRoot).IsAssignableFrom(typeof(T)) ? null : _root
                } as ICmsQuery<T>;
        }
    }
}
