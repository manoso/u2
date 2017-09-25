using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace u2.Umbraco
{
    public class ExamineVisitor : ExpressionVisitor
    {
        private StringBuilder _builder;

        public override Expression Visit(Expression node)
        {
            if (node != null && node.NodeType == ExpressionType.Lambda && Query == null)
            {
                var lambda = (LambdaExpression)node;
                if (lambda.Parameters.Count == 1)
                {
                    _builder = new StringBuilder(lambda.Body.ToString());
                }
            }
            return base.Visit(node);
        }

        public string Query => _builder?.ToString();

        #region Overrides

        protected override Expression VisitMember(MemberExpression node)
        {
            var literal = node.ToString();
            string result;

            var member = node.Member;
            var expression = node.Expression as ConstantExpression;
            var property = member as PropertyInfo;

            if (expression != null && member is FieldInfo)
            {
                var value = expression.Value;
                var constant = ((FieldInfo)member).GetValue(value);
                result = constant.ToString();
            }
            else
            {
                result = string.Format(property != null && property.PropertyType == typeof(bool) ? "{0}:(1)" : "{0}", member.Name);
            }

            _builder.Replace(literal, result);

            return base.VisitMember(node);
        }


        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType == ExpressionType.Not)
            {
                var literal = node.ToString();
                _builder.Replace(literal, $"-({node.Operand})");
            }
            return base.VisitUnary(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            const string strTrue = "True";

            var literal = node.ToString();
            var left = node.Left.ToString();
            var right = node.Right.ToString();
            var format = string.Empty;
            switch (node.NodeType)
            {
                case ExpressionType.AndAlso:
                    format = "+({0}) +({1})";
                    break;
                case ExpressionType.OrElse:
                    format = "({0}) ({1})";
                    break;
                case ExpressionType.Equal:
                    format = @"{0}:({1})";
                    break;
            }
            bool isLeft;
            if ((isLeft = left == strTrue) || right == strTrue)
                format = isLeft ? "{1}" : "{0}";

            if (!string.IsNullOrEmpty(format))
                _builder.Replace(literal, string.Format(format, left, right));
            return base.VisitBinary(node);
        }

        #endregion
    }
}
