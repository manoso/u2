﻿using System;
using System.Collections.Generic;
using System.Linq;
using u2.Core.Contract;

namespace u2.Core
{
    public class ContentType
    {
        public static ContentType<T> For<T>(IMapRegistry registry, IMapper mapper)
            where T : class, new()
        {
            return new ContentType<T>(registry, mapper);
        }
    }

    public class ContentType<T> : ContentType
            where T : class, new()
    {
        private class Case
        {
            public Func<IContent, bool> When { get; set; }
            public Type Type { get; set; }
        }

        private readonly IMapRegistry _registry;
        private readonly IMapper _mapper;
        private readonly List<Case> _cases = new List<Case>();
        private bool _useDefault;

        public ContentType(IMapRegistry registry, IMapper mapper)
        {
            _registry = registry;
            _mapper = mapper;
        }
        public ContentType<T> AddCase<TC>(Func<IContent, bool> when)
            where TC : T
        {
            _cases.Add(new Case { Type = typeof(TC), When = when });
            return this;
        }

        public ContentType<T> Add<TC>(string alias)
            where TC : T
        {
            return AddCase<TC>(n => string.Equals(alias, n.Alias));
        }

        public ContentType<T> UseDefault
        {
            get
            {
                _useDefault = true;
                return this;
            }
        }

        public Type Ask(IContent content)
        {
            var type = _cases.Where(c => c.When(content)).Select(c => c.Type).FirstOrDefault();

            if (type == null && _useDefault)
                type = _registry.GetType(content.Alias);

            return type ?? typeof(T);
        }

        public T Apply(IContent content, T value = null)
        {
            return _mapper.To(content, Ask(content), value) as T;
        }
    }
}
