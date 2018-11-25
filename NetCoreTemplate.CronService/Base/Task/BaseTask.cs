namespace NetCoreTemplate.CronService.Base.Task
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using NetCoreTemplate.CronService.Interfaces;

    public abstract class BaseTask : ITask
    {
        public string Name { get; }

        protected virtual bool RestartOnError { get; set; } = true;

        private readonly CancellationTokenSource cancelToken = new CancellationTokenSource();

        private readonly IIntervalCalculator intervalCalculator;

        protected BaseTask(IIntervalCalculator intervalCalculator)
        {
            this.intervalCalculator = intervalCalculator;
            Name = GetType().Name;
        }

        protected BaseTask(IIntervalCalculator intervalCalculator, string name)
        {
            this.intervalCalculator = intervalCalculator;
            Name = name;
        }

        public void Start()
        {
            Console.WriteLine($"Starting task '{Name}'");

            StartTaskProcess();
        }

        public void Stop()
        {
            cancelToken.Cancel();
        }

        protected abstract void TaskToExecute();

        private void StartTaskProcess()
        {
            Task.Run(() =>
            {
                StartTimer(DateTime.Now, cancelToken.Token).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        private async Task StartTimer(DateTime startTime, CancellationToken token)
        {
            var nextRunTime = intervalCalculator.GetNextOccurence(startTime);

            try
            {
                while (!cancelToken.IsCancellationRequested)
                {
                    await DelayUntil(nextRunTime, token);

                    ExecuteTask();

                    nextRunTime = intervalCalculator.GetNextOccurence(DateTime.Now);
                }

                Console.WriteLine($"Task '{Name}' has been stopped.");
            }
            catch (Exception e)
            {
                OnError(e);
            }
        }

        private void ExecuteTask()
        {
            Console.WriteLine($"Executing task '{Name}'");

            try
            {
                TaskToExecute();
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error was encountered in task '{Name}':", exception);
            }
        }

        private async Task DelayUntil(DateTime nextRunTime, CancellationToken token)
        {
            var delay = nextRunTime - DateTime.Now;

            if (delay > TimeSpan.Zero)
            {
                Console.WriteLine($"Entering sleepmode for task '{Name}' with delay of {delay}");
                await Task.Delay(delay, token).ContinueWith(x => { }, token);
            }
        }

        private void OnError(Exception exception)
        {
            Console.WriteLine($"Task {Name} encountered a fatal error and has stopped.", exception);

            if (RestartOnError)
            {
                Console.WriteLine($"Attempting to restart task '{Name}'");

                StartTaskProcess();

                Console.WriteLine($"Task '{Name}' has been restarted.");
            }
        }
    }
}
