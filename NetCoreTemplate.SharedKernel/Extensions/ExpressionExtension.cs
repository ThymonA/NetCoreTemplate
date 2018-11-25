namespace NetCoreTemplate.SharedKernel.Extensions
{
    using System;
    using System.Linq.Expressions;

    public static class ExpressionExtension
    {
        public static string GetNameFromExpression<TModel, TProperty>(this Expression<Func<TModel, TProperty>> expression)
        {
            var body = expression.Body as MemberExpression;

            if (body.IsNullOrDefault())
            {
                var unaryBody = (UnaryExpression)expression.Body;
                body = unaryBody.Operand as MemberExpression;
            }

            return body?.Member.Name;
        }

        public static string GetStringRepresentation(this Expression expression)
        {
            var body = expression.ToString();

            if (body.Contains("Convert(") && body.EndsWith(")"))
            {
                body = body.Remove(body.LastIndexOf(')')).Replace("Convert(", string.Empty);
            }

            return body.Substring(body.IndexOf(".", StringComparison.InvariantCulture) + 1).Replace(", Object", string.Empty).Replace("\"", string.Empty);
        }
    }
}
