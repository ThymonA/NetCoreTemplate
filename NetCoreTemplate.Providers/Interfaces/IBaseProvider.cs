namespace NetCoreTemplate.Providers.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IBaseProvider<TEntity>
        where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        IList<TEntity> GetAllAsList();

        TEntity GetEntity(Expression<Func<TEntity, bool>> expression);

        IList<TEntity> Where(Expression<Func<TEntity, bool>> expression);

        bool Exists(Expression<Func<TEntity, bool>> expression);
    }
}
