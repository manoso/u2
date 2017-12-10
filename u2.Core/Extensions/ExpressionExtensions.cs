using System;
using System.Linq.Expressions;
using System.Reflection;

namespace u2.Core.Extensions
{
    /// <summary>
    /// Expression related extenstion methods.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Return PropertyInfo from a lambda property expression.
        /// </summary>
        /// <param name="property">The lambda property expression.</param>
        /// <returns></returns>
        public static PropertyInfo ToInfo(this Expression property)
        {
            var expLambda = property as LambdaExpression;

            var expMem = expLambda?.Body as MemberExpression;

            return expMem?.Member as PropertyInfo;
        }

        /// <summary>
        /// Create a setter action using a given PropertyInfo.
        /// </summary>
        /// <param name="info">The given PropertyInfo.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Create a setter action from a lambda property expression.
        /// </summary>
        /// <typeparam name="T">The property declaring type.</typeparam>
        /// <typeparam name="TP">The property type.</typeparam>
        /// <param name="property">The lambda property expression.</param>
        /// <returns></returns>
        public static Action<T, TP> ToSetter<T, TP>(this Expression<Func<T, TP>> property)
        {
            var parameter = (ParameterExpression)((MemberExpression)property.Body).Expression;
            var value = Expression.Parameter(typeof(TP));
            var assign = Expression.Assign(property.Body, value);
            return  Expression.Lambda<Action<T, TP>>(assign, parameter, value).Compile();
        }
    }

}
