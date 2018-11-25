namespace NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using TrackableEntities.Common.Core;

    public interface IPersistenceLayer : IDisposable
    {
        bool AllowAutoSave { get; set; }

        Task<List<T>> Where<T>(Expression<Func<T, bool>> expression)
            where T : class;

        IQueryable<T> Get<T>()
            where T : class;

        void AddOrUpdate<T>(T entity)
            where T : class, ITrackable, IMergeable;

        void AddOrUpdateRange<T>(ICollection<T> entityList)
            where T : class, ITrackable, IMergeable;

        void Add<T>(T entity)
            where T : class, ITrackable, IMergeable;

        void AddRange<T>(IEnumerable<T> entityList)
            where T : class, ITrackable, IMergeable;

        void Delete<T>(T entity)
            where T : class, ITrackable, IMergeable;

        void DeleteRange<T>(ICollection<T> entityList)
            where T : class, ITrackable, IMergeable;

        int SaveChanges();
    }
}
