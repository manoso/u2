using System;
using System.Linq.Expressions;
using System.Reflection;

namespace u2.Core.Extensions
{
    public static class ExpressionExtensions
    {
        public static PropertyInfo ToInfo(this Expression property)
        {
            var expLambda = property as LambdaExpression;

            var expMem = expLambda?.Body as MemberExpression;

            return expMem?.Member as PropertyInfo;
        }

        public static Action<object, object> ToSetter(this PropertyInfo info)
        {
            var type = info.DeclaringType;
            var parameter = Expression.Parameter(typeof(object));
            var typedParameter = Expression.Convert(parameter, type);

            var property = Expression.Property(typedParameter, info);

            var value = Expression.Parameter(typeof(object));
            var typedValue = Expression.Convert(value, info.PropertyType);

            var assign = Expression.Assign(property, typedValue);

            var actionType = typeof(Action<,>).MakeGenericType(typeof(object), typeof(object));
            var setter = Expression.Lambda(actionType, assign, parameter, value).Compile();

            return setter as Action<object, object>;
        }

        public static Action<T, TP> ToSetter<T, TP>(this Expression<Func<T, TP>> property)
        {
            var parameter = (ParameterExpression)((MemberExpression)property.Body).Expression;
            var value = Expression.Parameter(typeof(TP));
            var assign = Expression.Assign(property.Body, value);
            return  Expression.Lambda<Action<T, TP>>(assign, parameter, value).Compile();
        }
    }

}
