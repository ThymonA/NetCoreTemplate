namespace NetCoreTemplate.CronService.Base.Service
{
    using NetCoreTemplate.CronService.Interfaces;

    internal sealed class MainService
    {
        private readonly ITaskFactory taskFactory;

        public MainService(ITaskFactory taskFactory)
        {
            this.taskFactory = taskFactory;
        }

        public void StartTasks()
        {
            var tasks = taskFactory.GetTaskInstances();

            foreach (var task in tasks)
            {
                task.Start();
            }
        }
    }
}
