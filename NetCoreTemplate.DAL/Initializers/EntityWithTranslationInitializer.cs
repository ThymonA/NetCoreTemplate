namespace NetCoreTemplate.DAL.Initializers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NetCoreTemplate.DAL.Extensions;
    using NetCoreTemplate.DAL.Models.Base;

    using Microsoft.EntityFrameworkCore;

    public abstract class EntityWithTranslationInitializer<TEntity> : IDisposable
        where TEntity : EntityTranslation
    {
        protected EntityWithTranslationInitializer(DatabaseContext context)
        {
            Context = context;
        }

        public bool AllowSeedMetaData => MainInitializer.AllowSeedMetaData;

        public bool AllowSeedTestData => MainInitializer.AllowSeedTestData;

        public DatabaseContext Context { get; }

        public abstract Expression<Func<DatabaseContext, DbSet<TEntity>>> Expression { get; }

        public abstract Expression<Func<TEntity, int>> KeyExpression { get; }

        public IQueryable<TEntity> Seed()
        {
            if (AllowSeedMetaData)
            {
                var entities = SeedMetaData();
                var updatedEntities = new List<TEntity>();

                foreach (var entity in entities)
                {
                    updatedEntities.Add(TranslationInitializer.SeedEntityTranslations(
                        Context,
                        Expression,
                        KeyExpression,
                        entity));
                }

                Context.AddOrUpdateRange(
                    Expression,
                    KeyExpression,
                    updatedEntities);
            }

            if (AllowSeedTestData)
            {
                var entities = SeedTestData();
                var updatedEntities = new List<TEntity>();

                foreach (var entity in entities)
                {
                    updatedEntities.Add(TranslationInitializer.SeedEntityTranslations(
                        Context,
                        Expression,
                        KeyExpression,
                        entity));
                }

                Context.AddOrUpdateRange(
                    Expression,
                    KeyExpression,
                    updatedEntities);
            }

            Context.SaveChanges();

            return Context.Set<TEntity>();
        }

        public virtual List<TEntity> SeedMetaData() { return new List<TEntity>(); }

        public virtual List<TEntity> SeedTestData() { return new List<TEntity>(); }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
