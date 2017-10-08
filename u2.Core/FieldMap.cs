using System;
using System.Linq.Expressions;
using System.Reflection;
using u2.Core.Contract;

namespace u2.Core
{
    public class FieldMap : IFieldMap
    {
        public IPropertySetter Setter { get; set; }
        public Func<string, object> Converter { get; protected set; }
        public Action<object, object> Defer { get; protected set; }

        public string Alias { get; set; }

        public object Default { get; set; }
        public Type ContentType { get; set; }

        public FieldMap(PropertyInfo info)
        {
            Alias = info.Name.ToLowerInvariant();
            ContentType = info.PropertyType;
            Setter = new PropertySetter(info);
        }

        public FieldMap() { }

        public bool MatchAlias(string alias)
        {
            return string.Equals(Alias, alias, StringComparison.InvariantCultureIgnoreCase);
        }
    }

    public class FieldMap<T, TP> : FieldMap, IFieldMap<T, TP>
    {
        public Func<string, TP> Convert
        {
            set
            {
                if (value != null)
                {
                    Converter = x => value(x) as object;
                    ContentType = typeof(string);
                }
            }
        }

        public Action<T, TP> ActDefer
        {
            set
            {
                if (value != null)
                    Defer = (x, y) => value((T)x, (TP)y);
            }
        }

        public FieldMap(string alias = null, Expression<Func<T, TP>> property = null)
        {
            if (property != null)
            {
                Setter = new PropertySetter<T, TP>(property);
            }

            ContentType = typeof(TP);
            Alias = (string.IsNullOrWhiteSpace(alias) ? Setter?.Name : alias)?.ToLowerInvariant();
        }

    }

    public class FieldMapCopy : FieldMap
    {
    }
}