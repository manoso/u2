using System;
using System.Collections.Generic;
using System.Linq;

namespace u2.Core
{
    public class ContentType
    {
        public static ContentType<T> For<T>(IMap map)
            where T : class, new()
        {
            return new ContentType<T>(map);
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

        private readonly IMap _map;
        private readonly List<Case> _cases = new List<Case>();
        private bool _useDefault;

        public ContentType(IMap map)
        {
            _map = map;
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
                type = _map.GetType(content.Alias);

            return type ?? typeof(T);
        }

        public T Apply(IContent content, T value = null)
        {
            return _map.To(content, Ask(content), value) as T;
        }
    }

}
