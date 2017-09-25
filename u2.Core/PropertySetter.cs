using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using u2.Core.Extensions;

namespace u2.Core
{
    public class PropertySetter
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
                    var parameter = Expression.Parameter(typeof(object));
                    var typedParameter = Expression.Convert(parameter, type);

                    var property = Expression.Property(typedParameter, info);

                    var value = Expression.Parameter(typeof(object));
                    var typedValue = Expression.Convert(value, info.PropertyType);

                    var assign = Expression.Assign(property, typedValue);

                    var actionType = typeof(Action<,>).MakeGenericType(typeof(object), typeof(object));
                    var setter = Expression.Lambda(actionType, assign, parameter, value).Compile();

                    Name = info.Name;
                    Set = setter as Action<object, object>;
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
                var parameter = (ParameterExpression)((MemberExpression)property.Body).Expression;
                var value = Expression.Parameter(typeof(TP));
                var assign = Expression.Assign(property.Body, value);
                var setter = Expression.Lambda<Action<T, TP>>(assign, parameter, value).Compile();

                Name = info.Name;
                Set = (x, y) => setter((T) x, (TP) y);
            }
        }



    }
}
