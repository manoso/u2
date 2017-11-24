using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using u2.Core.Contract;

namespace u2.Core
{
    public class MapItem : IMapItem
    {
        public IPropertySetter Setter { get; set; }
        public Func<string, object> Converter { get; protected set; }
        public Func<ICache, object, object, Task> Defer { get; protected set; }
        public Func<string, Func<IMapper, ICache, Task<object>>> Mapper { get; protected set; }

        public string Alias { get; set; }

        public object Default { get; set; }
        public Type ContentType { get; set; }

        public MapItem(PropertyInfo info)
        {
            Alias = info.Name.ToLowerInvariant();
            ContentType = info.PropertyType;
            Setter = new PropertySetter(info);
        }

        public MapItem() { }

        public bool MatchAlias(string alias)
        {
            return string.Equals(Alias, alias, StringComparison.InvariantCultureIgnoreCase);
        }
    }

    public class MapItem<T, TP> : MapItem, IMapItem<T, TP>
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

        public Func<string, Func<IMapper, ICache, Task<object>>> Map
        {
            set
            {
                if (value != null)
                {
                    Mapper = value;
                    ContentType = typeof(string);
                }
            }
        }

        public Func<ICache, T, TP, Task> ActDefer
        {
            set
            {
                if (value != null)
                    Defer = async (x, y, z) => await value(x, (T)y, (TP)z).ConfigureAwait(false);
            }
        }

        public MapItem(string alias = null, Expression<Func<T, TP>> property = null)
        {

            if (property != null)
            {
                Setter = new PropertySetter<T, TP>(property);
            }

            ContentType = typeof(TP);
            Alias = (string.IsNullOrWhiteSpace(alias) ? Setter?.Name : alias)?.ToLowerInvariant();
        }
    }

    public class MapItemCopy : MapItem
    {
    }
}