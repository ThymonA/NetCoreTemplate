namespace NetCoreTemplate.SharedKernel.ServiceContainer.Event
{
    using System;

    using NetCoreTemplate.SharedKernel.Extensions;

    public class SimpleInjectorEventHandler
    {
        public static EventHandler<SimpleInjectorEventArgs> RegisterEvent { get; set; }

        public void OnRegister(SimpleInjectorEventArgs args)
        {
            var handler = RegisterEvent;

            if (handler.IsNullOrDefault())
            {
                return;
            }

            handler(this, args);
        }
    }
}
