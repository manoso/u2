using System;
using System.Linq.Expressions;
using System.Reflection;
using u2.Core.Contract;
using u2.Core.Extensions;

namespace u2.Core
{
    public class PropertySetter : IPropertySetter
    {
        public Action<object, object> Set { get; set; }

        public string Name { get; set; }

        public PropertySetter() { }

        public PropertySetter(PropertyInfo info)
        {
            if (info != null)
            {
                var type = info.DeclaringType;
                if (type != null)
                {
                    Name = info.Name;
                    Set = info.ToSetter();
                }
            }
        }
    }

    public class PropertySetter<T, TP> : PropertySetter
    { 
        public PropertySetter(Expression<Func<T, TP>> property)
        {
            var info = property.ToInfo();
            if (property != null && info != null)
            {
                Name = info.Name;
                Set = (x, y) => property.ToSetter()((T) x, (TP) y);
            }
        }
    }
}
