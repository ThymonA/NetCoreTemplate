namespace NetCoreTemplate.CronService.Interfaces
{
    using System.Collections.Generic;

    public interface ITaskFactory
    {
        IEnumerable<ITask> GetTaskInstances();
    }
}
