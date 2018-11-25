namespace NetCoreTemplate.CronService.Interfaces
{
    using System;

    public interface IIntervalCalculator
    {
        DateTime GetNextOccurence(DateTime from);
    }
}
