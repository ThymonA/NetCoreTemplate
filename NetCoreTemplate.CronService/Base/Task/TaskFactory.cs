namespace NetCoreTemplate.CronService.Base.Task
{
    using System.Collections.Generic;

    using NetCoreTemplate.CronService.Interfaces;

    public sealed class TaskFactory : ITaskFactory
    {
        private readonly IEnumerable<ITask> tasks;

        public TaskFactory(IEnumerable<ITask> tasks)
        {
            this.tasks = tasks;
        }

        public IEnumerable<ITask> GetTaskInstances()
        {
            return tasks;
        }
    }
}
