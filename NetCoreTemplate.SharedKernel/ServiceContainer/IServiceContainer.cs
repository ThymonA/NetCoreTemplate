namespace NetCoreTemplate.SharedKernel.ServiceContainer
{
    using System.Collections.Generic;

    public interface IServiceContainer
    {
        TService GetService<TService>()
            where TService : class;

        IEnumerable<TService> GetServices<TService>()
            where TService : class;
    }
}
