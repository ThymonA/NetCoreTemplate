namespace NetCoreTemplate.Providers.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using NetCoreTemplate.Providers.Interfaces;
    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;

    public abstract class BaseProvider<TEntity> : IBaseProvider<TEntity>
        where TEntity : class
    {
        protected IPersistenceLayer Persistence;

        protected BaseProvider(IPersistenceLayer persistence)
        {
            Persistence = persistence;
        }

        public IQueryable<TEntity> GetAll()
        {
            return Persistence.Get<TEntity>();
        }

        public IList<TEntity> GetAllAsList()
        {
            return Persistence.Get<TEntity>().ToList();
        }

        public TEntity GetEntity(Expression<Func<TEntity, bool>> expression)
        {
            return Persistence.Get<TEntity>().FirstOrDefault(expression);
        }

        public IList<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return Persistence.Get<TEntity>().Where(expression).ToList();
        }

        public bool Exists(Expression<Func<TEntity, bool>> expression)
        {
            return Persistence.Get<TEntity>().Any(expression);
        }
    }
}
