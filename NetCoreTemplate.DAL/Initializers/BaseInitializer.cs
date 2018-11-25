namespace NetCoreTemplate.DAL.Initializers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class BaseInitializer<TEntity> : IDisposable
        where TEntity : class
    {
        protected BaseInitializer(DatabaseContext context)
        {
            Context = context;
        }

        public bool AllowSeedTestData => MainInitializer.AllowSeedTestData;

        public bool AllowSeedMetaData => MainInitializer.AllowSeedMetaData;

        public DatabaseContext Context { get; }

        public IQueryable<TEntity> Seed()
        {
            if (AllowSeedTestData)
            {
                SeedTestData();
            }

            if (AllowSeedMetaData)
            {
                SeedData();
            }

            Context.SaveChanges();

            return Context.Set<TEntity>();
        }

        public abstract List<TEntity> SeedData();

        public virtual List<TEntity> SeedTestData()
        {
            return new List<TEntity>();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
