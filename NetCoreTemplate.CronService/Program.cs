namespace NetCoreTemplate.CronService
{
    using System;
    using System.Globalization;
    using System.IO;

    using NetCoreTemplate.CronService.Base.Service;
    using NetCoreTemplate.CronService.Interfaces;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.SharedKernel.ServiceContainer.Event;

    using Microsoft.Extensions.Configuration;

    public static class Program
    {
        public static IConfiguration Configuration { get; private set; }

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            SetDefaultCultureCode();

            Configuration = builder.Build();

            SimpleInjectorEventHandler.RegisterEvent += RegisterEvent;

            DependencyResolver.Register();

            Console.WriteLine("Starting CronService");

            var service = new Service(ServiceContainer.Current.GetService<ITaskFactory>());
            service.Start();
        }

        public static void SetDefaultCultureCode()
        {
            var defaultCulture = new CultureInfo("nl-NL");
            defaultCulture.NumberFormat.NumberDecimalSeparator = ",";
            defaultCulture.NumberFormat.CurrencyDecimalSeparator = ",";
            defaultCulture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";

            CultureInfo.CurrentCulture = defaultCulture;
            CultureInfo.CurrentUICulture = defaultCulture;
            CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
            CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;
        }

        private static void RegisterEvent(object sender, SimpleInjectorEventArgs args)
        {
            var container = args.Container;

            container.RegisterSingleton(() => Configuration);
        }
    }
}
