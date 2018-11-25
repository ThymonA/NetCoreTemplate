namespace NetCoreTemplate.CronService.Base.Task
{
    using NetCoreTemplate.CronService.Base.Calculator;

    public abstract class ThreadScopedTask : BaseTask
    {
        protected ThreadScopedTask(string value)
            : base(new CronIntervalCalculator(value))
        {
        }

        protected override sealed void TaskToExecute()
        {
            using (DependencyResolver.BeginThreadScope())
            {
                ExecuteTask();
            }
        }

        protected abstract void ExecuteTask();
    }
}
