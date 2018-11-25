namespace NetCoreTemplate.CronService.Interfaces
{
    public interface ITask
    {
        string Name { get; }

        void Start();

        void Stop();
    }
}
