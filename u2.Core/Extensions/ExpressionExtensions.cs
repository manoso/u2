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
    }
}
