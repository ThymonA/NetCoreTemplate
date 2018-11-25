namespace NetCoreTemplate.DAL.Extensions
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    using NetCoreTemplate.DAL.Attributes;

    using Microsoft.EntityFrameworkCore;

    public static class AttributedIndexBuilderExtension
    {
        private class IndexParam
        {
            public string IndexName { get; }

            public bool IsUnique { get; }

            public bool IsClustered { get; }

            public string[] PropertyNames { get; }

            public IndexParam(IndexAttribute indexAttr, params PropertyInfo[] properties)
            {
                IndexName = indexAttr.Name;
                IsUnique = indexAttr.IsUnique;
                IsClustered = indexAttr.IsClustered;
                PropertyNames = properties.Select(prop => prop.Name).ToArray();
            }
        }

        public static void BuildIndexesFromAnnotations(this ModelBuilder modelBuilder)
        {
            var entityTypes = modelBuilder.Model.GetEntityTypes();

            Parallel.ForEach(
                entityTypes,
                entityType =>
                {
                    var items = entityType.ClrType
                    .GetProperties()
                    .SelectMany(prop =>
                        Attribute.GetCustomAttributes(prop, typeof(IndexAttribute))
                            .Cast<IndexAttribute>()
                            .Select(index => new { prop, index }))
                    .ToArray();

                var indexParams = items
                    .Where(item => item.index.Name == string.Empty)
                    .Select(item => new IndexParam(item.index, item.prop));

                var namedIndexParams = items
                    .Where(item => item.index.Name != string.Empty)
                    .GroupBy(item => item.index.Name)
                    .Select(g => new IndexParam(
                        g.First().index,
                        g.OrderBy(item => item.index.Order).Select(item => item.prop).ToArray()));

                lock (modelBuilder)
                {
                    var entity = modelBuilder.Entity(entityType.ClrType);
                    foreach (var indexParam in indexParams.Concat(namedIndexParams))
                    {
                        var indexBuilder = entity
                            .HasIndex(indexParam.PropertyNames)
                            .IsUnique(indexParam.IsUnique)
                            .ForSqlServerIsClustered(indexParam.IsClustered);

                        if (indexParam.IndexName != string.Empty)
                        {
                            indexBuilder.HasName(indexParam.IndexName);
                        }
                    }
                }
            });
        }
    }
}
