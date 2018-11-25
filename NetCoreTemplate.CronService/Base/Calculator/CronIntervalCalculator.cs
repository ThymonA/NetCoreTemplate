namespace NetCoreTemplate.CronService.Base.Calculator
{
    using System;

    using NetCoreTemplate.CronService.Interfaces;

    using NCrontab;

    public sealed class CronIntervalCalculator : IIntervalCalculator
    {
        private readonly CrontabSchedule schedule;

        public CronIntervalCalculator(string expression)
        {
            schedule = CrontabSchedule.Parse(expression);
        }

        public DateTime GetNextOccurence(DateTime from)
        {
            return schedule.GetNextOccurrence(from);
        }
    }
}
