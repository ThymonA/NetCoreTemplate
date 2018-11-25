namespace NetCoreTemplate.SharedKernel.Expressions
{
    using System;
    using System.Linq.Expressions;

    using NetCoreTemplate.SharedKernel.Enums;

    public class SortExpression<T>
        where T : class
    {
        public Expression<Func<T, object>> Expression { get; }

        public SortType SortType { get; }

        public SortExpression(Expression<Func<T, object>> expression) : this(expression, SortType.Ascending)
        {
        }

        public SortExpression(Expression<Func<T, object>> expression, SortType sortType)
        {
            Expression = expression;
            SortType = sortType;
        }

        public SortExpression<T> Flip()
        {
            return new SortExpression<T>(Expression, SortType == SortType.Ascending ? SortType.Descending : SortType.Ascending);
        }
    }
}
