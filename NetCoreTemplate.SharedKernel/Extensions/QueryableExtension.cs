namespace NetCoreTemplate.SharedKernel.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NetCoreTemplate.SharedKernel.Enums;
    using NetCoreTemplate.SharedKernel.Expressions;

    using MoreLinq;

    public static class QueryableExtension
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, List<SortExpression<T>> sortExpressions)
            where T : class
        {
            return OrderList(source, sortExpressions);
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string field, string dir = "asc")
        {
            var parameter = Expression.Parameter(typeof(TSource), "r");
            var expression = Expression.Property(parameter, field);

            try
            {
                var lambda = Expression.Lambda<Func<TSource, string>>(expression, parameter);

                return string.Equals(dir, "desc", StringComparison.InvariantCultureIgnoreCase)
                           ? source.OrderByDescending(lambda)
                           : source.OrderBy(lambda);
            }
            catch (Exception)
            {
                // Empty exception
            }

            try
            {
                var lambda = Expression.Lambda<Func<TSource, int>>(expression, parameter);

                return string.Equals(dir, "desc", StringComparison.InvariantCultureIgnoreCase)
                   ? source.OrderByDescending(lambda)
                   : source.OrderBy(lambda);
            }
            catch (Exception)
            {
                // Empty exception
            }

            try
            {
                var lambda = Expression.Lambda<Func<TSource, bool>>(expression, parameter);

                return string.Equals(dir, "desc", StringComparison.InvariantCultureIgnoreCase)
                   ? source.OrderByDescending(lambda)
                   : source.OrderBy(lambda);
            }
            catch (Exception)
            {
                // Empty exception
            }

            try
            {
                var lambda = Expression.Lambda<Func<TSource, DateTimeOffset>>(expression, parameter);

                return string.Equals(dir, "desc", StringComparison.InvariantCultureIgnoreCase)
                   ? source.OrderByDescending(lambda)
                   : source.OrderBy(lambda);
            }
            catch (Exception)
            {
                // Empty exception
            }

            try
            {
                var lambda = Expression.Lambda<Func<TSource, DateTime>>(expression, parameter);

                return string.Equals(dir, "desc", StringComparison.InvariantCultureIgnoreCase)
                   ? source.OrderByDescending(lambda)
                   : source.OrderBy(lambda);
            }
            catch (Exception)
            {
                // Empty exception
            }

            try
            {
                var type = expression.Type;
                var isEnum = type.IsEnum;

                if (isEnum)
                {
                    var lambda = Expression.Lambda<Func<TSource, int>>(expression, parameter);

                    return string.Equals(dir, "desc", StringComparison.InvariantCultureIgnoreCase)
                               ? source.OrderByDescending(lambda)
                               : source.OrderBy(lambda);
                }
            }
            catch (Exception)
            {
                // Empty exception
            }

            try
            {
                var lambda = Expression.Lambda<Func<TSource, decimal>>(expression, parameter);

                return string.Equals(dir, "desc", StringComparison.InvariantCultureIgnoreCase)
                   ? source.OrderByDescending(lambda)
                   : source.OrderBy(lambda);
            }
            catch (Exception)
            {
                // Empty exception
            }

            try
            {
                var lambda = Expression.Lambda<Func<TSource, double>>(expression, parameter);

                return string.Equals(dir, "desc", StringComparison.InvariantCultureIgnoreCase)
                           ? source.OrderByDescending(lambda)
                           : source.OrderBy(lambda);
            }
            catch (Exception)
            {
                // Empty exception
            }

            try
            {
                var lambda = Expression.Lambda<Func<TSource, object>>(expression, parameter);

                return string.Equals(dir, "desc", StringComparison.InvariantCultureIgnoreCase)
                           ? source.OrderByDescending(lambda)
                           : source.OrderBy(lambda);
            }
            catch (Exception)
            {
                // Empty exception
            }

            throw new Exception();
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, List<SortExpression<T>> sortExpressions)
            where T : class
        {
            var sortExpressionsDescending = sortExpressions.Select(x => x.Flip()).ToList();

            return OrderList(source, sortExpressionsDescending);
        }

        private static IOrderedQueryable<T> OrderList<T>(IQueryable<T> source, List<SortExpression<T>> sortExpressions)
            where T : class
        {
            if (!sortExpressions.Any())
            {
                throw new ArgumentOutOfRangeException($"{typeof(List<SortExpression<T>>)}");
            }

            var orderedList = sortExpressions.First().SortType == SortType.Descending
                                  ? source.OrderByDescending(sortExpressions.First().Expression)
                                  : source.OrderBy(sortExpressions.First().Expression);

            sortExpressions
                .Skip(1)
                .ForEach(
                    x => orderedList = x.SortType == SortType.Descending
                    ? orderedList.ThenByDescending(x.Expression)
                    : orderedList.ThenBy(x.Expression));

            return orderedList;
        }
    }
}
