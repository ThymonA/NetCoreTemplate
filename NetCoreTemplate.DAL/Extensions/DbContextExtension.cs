namespace NetCoreTemplate.DAL.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;

    using NetCoreTemplate.DAL.Initializers;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Migrations;

    using TrackableEntities.Common.Core;
    using TrackableEntities.EF.Core;

    public static class DbContextExtension
    {
        public static bool AllMigrationApplied(this DbContext context)
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(x => x.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(x => x.Key);

            return !total.Except(applied).Any();
        }

        public static void Seed(this DatabaseContext context)
        {
            MainInitializer.Seed(context);
        }

        public static void AddOrUpdateRange<TEntity, TProperty>(
            this DatabaseContext context,
            Expression<Func<DatabaseContext, DbSet<TEntity>>> expression,
            Expression<Func<TEntity, TProperty>> test,
            IEnumerable<TEntity> entities)
            where TEntity : class
        {
            context.Database.OpenConnection();
            var setIdentityOn = $"SET IDENTITY_INSERT dbo.[{typeof(TEntity).Name}] ON";
            context.Database.ExecuteSqlCommand(setIdentityOn);

            var added = new Collection<TEntity>();
            var updated = new Collection<TEntity>();
            var dbSet = expression.Compile()(context);

            foreach (var entity in entities)
            {
                var value = test.Compile()(entity);
                var p = test.Parameters.First();
                var predicate = Expression.Lambda<Func<TEntity, bool>>(
                    Expression.Equal(test.Body, Expression.Constant(value)), p);
                var exists = dbSet.Any(predicate);

                if (exists)
                {
                    updated.Add(entity);
                    dbSet.Update(entity);
                }
                else
                {
                    added.Add(entity);
                    dbSet.Add(entity);
                }
            }

            context.SaveChanges();
            var setIdentityOff = $"SET IDENTITY_INSERT dbo.[{typeof(TEntity).Name}] OFF";
            context.Database.ExecuteSqlCommand(setIdentityOff);
            context.Database.CloseConnection();
        }

        public static void AddOrUpdateRangeWithoutIdentity<TEntity, TProperty>(
            this DatabaseContext context,
            Expression<Func<DatabaseContext, DbSet<TEntity>>> expression,
            Expression<Func<TEntity, TProperty>> test,
            IEnumerable<TEntity> entities)
            where TEntity : class
        {
            context.Database.OpenConnection();

            var added = new Collection<TEntity>();
            var updated = new Collection<TEntity>();
            var dbSet = expression.Compile()(context);

            foreach (var entity in entities)
            {
                var value = test.Compile()(entity);
                var p = test.Parameters.First();
                var predicate = Expression.Lambda<Func<TEntity, bool>>(
                    Expression.Equal(test.Body, Expression.Constant(value)), p);
                var exists = dbSet.Any(predicate);

                if (exists)
                {
                    updated.Add(entity);
                    dbSet.Update(entity);
                }
                else
                {
                    added.Add(entity);
                    dbSet.Add(entity);
                }
            }

            context.SaveChanges();
            context.Database.CloseConnection();
        }

        public static void AddOrUpdateRange<TEntity>(this DatabaseContext context, IEnumerable<TEntity> entities)
            where TEntity : class, ITrackable, IMergeable
        {
            var added = new Collection<TEntity>();
            var updated = new Collection<TEntity>();

            foreach (var entity in entities)
            {
                switch (entity.TrackingState)
                {
                    case TrackingState.Added:
                        added.Add(entity);
                        context.Add(entity);
                        context.ApplyChanges(entity);
                        break;
                    case TrackingState.Modified:
                        var state = context.Entry(entity).State;

                        switch (state)
                        {
                            case EntityState.Added:
                                entity.TrackingState = TrackingState.Added;
                                added.Add(entity);
                                context.ApplyChanges(entity);
                                break;
                            case EntityState.Modified:
                            case EntityState.Detached:
                                entity.TrackingState = TrackingState.Modified;
                                updated.Add(entity);
                                context.ApplyChanges(entity);
                                break;
                            default:
                                break;
                        }

                        break;
                    case TrackingState.Unchanged:
                        break;
                }
            }

            context.SaveChanges();
        }
    }
}
