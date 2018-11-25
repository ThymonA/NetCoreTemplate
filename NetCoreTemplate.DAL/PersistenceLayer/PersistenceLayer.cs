namespace NetCoreTemplate.DAL.PersistenceLayer
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using MoreLinq;

    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;

    using TrackableEntities.Common.Core;
    using TrackableEntities.EF.Core;

    public sealed class PersistenceLayer : IPersistenceLayer
    {
        private DatabaseContext Database = new DatabaseContext();

        public bool AllowAutoSave { get; set; } = true;

        public async Task<List<TEntity>> Where<TEntity>(Expression<Func<TEntity, bool>> expression)
            where TEntity : class
        {
            return await Task.Run(() => Database.Set<TEntity>().Where(expression).ToListAsync());
        }

        public IQueryable<TEntity> Get<TEntity>()
            where TEntity : class
        {
            return Database.Set<TEntity>();
        }

        public void AddOrUpdate<TEntity>(TEntity entity)
            where TEntity : class, ITrackable, IMergeable
        {
            AddOrUpdateRange(new Collection<TEntity> { entity }, AllowAutoSave);
        }

        public void AddOrUpdateRange<TEntity>(ICollection<TEntity> entities)
            where TEntity : class, ITrackable, IMergeable
        {
            AddOrUpdateRange(entities, AllowAutoSave);
        }

        public void Add<TEntity>(TEntity entity)
            where TEntity : class, ITrackable, IMergeable
        {
            entity.TrackingState = TrackingState.Added;

            AddOrUpdateRange(new List<TEntity> { entity }, true);
        }

        public void AddRange<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : class, ITrackable, IMergeable
        {
            var entityList = entities.ToList();
            entityList.ForEach(x => x.TrackingState = TrackingState.Added);

            AddOrUpdateRange(entityList, true);
        }

        public void Delete<TEntity>(TEntity entity)
            where TEntity : class, ITrackable, IMergeable
        {
            DeleteRange(new List<TEntity> { entity });
        }

        public void DeleteRange<TEntity>(ICollection<TEntity> entities)
            where TEntity : class, ITrackable, IMergeable
        {
            entities.ForEach(x => x.TrackingState = TrackingState.Deleted);

            foreach (var entity in entities)
            {
                Database.Remove(entity);

                Database.ApplyChanges(entity);
            }

            SaveChanges();
        }

        private void AddOrUpdateRange<TEntity>(IEnumerable<TEntity> entities, bool allowAutoSave)
            where TEntity : class, ITrackable, IMergeable
        {
            var oldState = AllowAutoSave;
            var added = new Collection<TEntity>();
            var updated = new Collection<TEntity>();

            AllowAutoSave = allowAutoSave;

            foreach (var entity in entities)
            {
                switch (entity.TrackingState)
                {
                    case TrackingState.Added:
                        added.Add(entity);
                        Database.Add(entity);
                        break;
                    case TrackingState.Unchanged:
                        var state = Database.Entry(entity).State;

                        switch (state)
                        {
                            case EntityState.Added:
                                entity.TrackingState = TrackingState.Added;
                                added.Add(entity);
                                break;
                            case EntityState.Unchanged:
                            case EntityState.Modified:
                            case EntityState.Detached:
                                entity.TrackingState = TrackingState.Modified;
                                updated.Add(entity);
                                break;
                        }

                        break;
                    case TrackingState.Modified:
                        Database.Update(entity);
                        updated.Add(entity);
                        break;
                }

                Database.ApplyChanges(entity);
            }

            SaveChanges();

            AllowAutoSave = oldState;
        }

        public int SaveChanges()
        {
            if (AllowAutoSave)
            {
                Database.SaveChanges();
            }

            return default(int);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}