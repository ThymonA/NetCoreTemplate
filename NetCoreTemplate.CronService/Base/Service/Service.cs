namespace NetCoreTemplate.CronService.Base.Service
{
    using System;
    using System.Threading;

    using NetCoreTemplate.CronService.Interfaces;

    public sealed class Service : IDisposable
    {
        private readonly ITaskFactory taskFactory;

        private readonly ManualResetEvent shutdownEvent = new ManualResetEvent(false);

        public Service(ITaskFactory taskFactory)
        {
            this.taskFactory = taskFactory;
        }

        public void Start()
        {
            new Thread(ThreadWorker).Start();

            var service = new MainService(taskFactory);
            service.StartTasks();
        }

        public void Dispose()
        {
            shutdownEvent.Dispose();
        }

        private void ThreadWorker()
        {
            while (!shutdownEvent.WaitOne(0))
            {
                Thread.Sleep(1000);
            }
        }
    }
}
