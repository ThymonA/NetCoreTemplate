namespace NetCoreTemplate.Services.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IBaseService<TEntity> : IDisposable
        where TEntity : class
    {
        bool AutoCommit { get; set; }

        void Add(TEntity entity);

        void AddRange(ICollection<TEntity> entities);

        void AddOrUpdate(TEntity entity);

        void AddOrUpdateRange(ICollection<TEntity> entities);

        void Delete(TEntity entity);

        void DeleteRange(ICollection<TEntity> entities);

        void SaveChanges();
    }
}
