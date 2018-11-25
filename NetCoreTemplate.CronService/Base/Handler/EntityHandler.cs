namespace NetCoreTemplate.CronService.Base.Handler
{
    using NetCoreTemplate.CronService.Interfaces;
    using NetCoreTemplate.Providers.Interfaces;
    using NetCoreTemplate.Services.Interfaces;

    public abstract class EntityHandler<TEntity> : IEntityHandler<TEntity>
        where TEntity : class
    {
        public IBaseProvider<TEntity> Provider { get; }

        public IBaseService<TEntity> Service { get; }

        protected EntityHandler(
            IBaseProvider<TEntity> provider,
            IBaseService<TEntity> service)
        {
            Provider = provider;
            Service = service;
        }

        public void Process() => ProcessTask();

        public virtual void ProcessTask()
        {
        }
    }
}
