namespace NetCoreTemplate.Services.Base
{
    using System.Collections.Generic;

    using NetCoreTemplate.Services.Interfaces;
    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;

    using TrackableEntities.Common.Core;

    public class BaseService<TEntity> : IBaseService<TEntity>
        where TEntity : class, ITrackable, IMergeable
    {
        private readonly IPersistenceLayer persistence;

        public BaseService(IPersistenceLayer persistence)
        {
            this.persistence = persistence;
        }

        public bool AutoCommit
        {
            get => persistence.AllowAutoSave;
            set => persistence.AllowAutoSave = value;
        }

        public void Add(TEntity entity)
        {
            persistence.Add(entity);
        }

        public void AddRange(ICollection<TEntity> entities)
        {
            persistence.AddRange(entities);
        }

        public void AddOrUpdate(TEntity entity)
        {
            persistence.AddOrUpdate(entity);
        }

        public void AddOrUpdateRange(ICollection<TEntity> entities)
        {
            persistence.AddOrUpdateRange(entities);
        }

        public void Delete(TEntity entity)
        {
            persistence.Delete(entity);
        }

        public void DeleteRange(ICollection<TEntity> entities)
        {
            persistence.DeleteRange(entities);
        }

        public void SaveChanges() => persistence.SaveChanges();

        public void Dispose()
        {
            persistence?.Dispose();
        }
    }
}
