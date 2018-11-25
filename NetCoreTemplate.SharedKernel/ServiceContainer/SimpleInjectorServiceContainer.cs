namespace NetCoreTemplate.SharedKernel.ServiceContainer
{
    using System.Collections.Generic;
    using System.Linq;

    using SimpleInjector;

    public sealed class SimpleInjectorServiceContainer : IServiceContainer
    {
        private readonly Container container;

        public SimpleInjectorServiceContainer(Container container)
        {
            this.container = container;
        }

        public TService GetService<TService>()
            where TService : class
        {
            return container.GetInstance<TService>();
        }

        public IEnumerable<TService> GetServices<TService>()
            where TService : class
        {
            try
            {
                return container.GetAllInstances<TService>();
            }
            catch (ActivationException)
            {
                return Enumerable.Empty<TService>();
            }
        }
    }
}
