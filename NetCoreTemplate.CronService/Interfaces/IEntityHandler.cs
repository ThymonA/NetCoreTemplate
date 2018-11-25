namespace NetCoreTemplate.CronService.Interfaces
{
    public interface IEntityHandler<TEntity>
        where TEntity : class
    {
        void Process();
    }
}
