namespace NetCoreTemplate.SharedKernel.ServiceContainer.Event
{
    using System;

    using SimpleInjector;

    public class SimpleInjectorEventArgs : EventArgs
    {
        public Container Container { get; set; }
    }
}
