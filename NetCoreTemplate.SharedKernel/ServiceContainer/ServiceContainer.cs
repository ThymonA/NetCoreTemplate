namespace NetCoreTemplate.SharedKernel.ServiceContainer
{
    public static class ServiceContainer
    {
        public static IServiceContainer Current { get; private set; }

        public static void Register(IServiceContainer container)
        {
            Current = container;
        }
    }
}
