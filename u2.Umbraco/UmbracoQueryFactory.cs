﻿using u2.Core.Contract;

namespace u2.Umbraco
{
    public class UmbracoQueryFactory : IQueryFactory
    {
        private readonly IRoot _root;

        public UmbracoQueryFactory(IRoot root)
        {
            _root = root;
        }

        public ICmsQuery<T> Create<T>(ITypeMap<T> typeMap) where T : class, new()
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

        public ICmsQuery Create(ITypeMap typeMap)
        {
            var isMedia = typeof(IMedia).IsAssignableFrom(typeMap.EntityType);
            return isMedia
                ? new MediaQuery()
                : new ContentQuery
                {
                    Alias = typeMap.Alias,
                    Root = typeof(IRoot).IsAssignableFrom(typeMap.EntityType) ? null : _root
                } as ICmsQuery;
        }
    }
}