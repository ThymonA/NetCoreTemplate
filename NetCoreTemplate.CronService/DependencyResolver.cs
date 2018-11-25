namespace NetCoreTemplate.CronService
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using NetCoreTemplate.Authentication;
    using NetCoreTemplate.CronService.Base.Task;
    using NetCoreTemplate.CronService.Interfaces;
    using NetCoreTemplate.DAL.Configuration;
    using NetCoreTemplate.DAL.Managers;
    using NetCoreTemplate.DAL.PersistenceLayer;
    using NetCoreTemplate.FileManager;
    using NetCoreTemplate.FileManager.Interfaces;
    using NetCoreTemplate.Providers.General;
    using NetCoreTemplate.Providers.Interfaces;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.Services.Interfaces;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.Interfaces.Managers;
    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.SharedKernel.ServiceContainer.Event;
    using NetCoreTemplate.ViewModelProcessors.Interfaces;

    using Microsoft.AspNetCore.Http;
    using Microsoft.DotNet.PlatformAbstractions;

    using NetCoreTemplate.Authentication.Client;

    using RazorLight;

    using SimpleInjector;
    using SimpleInjector.Lifestyles;

    public static class DependencyResolver
    {
        private static SimpleInjectorEventHandler handlers;

        public static Container Container { get; private set; }

        public static Scope BeginThreadScope()
        {
            return ThreadScopedLifestyle.BeginScope(Container);
        }

        public static void Register()
        {
            if (handlers.IsNullOrDefault())
            {
                handlers = new SimpleInjectorEventHandler();
            }

            var container = new Container();

            container.Options.DefaultScopedLifestyle = new ThreadScopedLifestyle();

            var serviceContainer = new SimpleInjectorServiceContainer(container);
            container.RegisterSingleton(() => serviceContainer);
            container.RegisterSingleton<IServiceContainer>(() => serviceContainer);
            ServiceContainer.Register(serviceContainer);

            handlers.OnRegister(new SimpleInjectorEventArgs { Container = container });

            RegisterGeneralDependencies(container);
            RegisterProviders(container);
            RegisterServices(container);
            RegisterViewModelProcessors(container);
            RegisterViewEngine(container);
            RegisterTasks(container);

            container.Verify();
            Container = container;
        }

        private static void RegisterGeneralDependencies(Container container)
        {
            container.RegisterSingleton<IHttpContextAccessor>(() => new HttpContextAccessor());
            container.Register<IPersistenceLayer, PersistenceLayer>(Lifestyle.Scoped);
            container.Register<ITranslationManager, TranslationManager>();
            container.Register(typeof(IBaseService<>), new List<Assembly> { typeof(IBaseService<>).Assembly }, Lifestyle.Scoped);
            container.Register(typeof(IBaseProvider<>), new List<Assembly> { typeof(IBaseProvider<>).Assembly }, Lifestyle.Scoped);
            container.Register<IDatabaseConfiguration, DatabaseConfiguration>();
            container.Register<IAuthenticationClient, AuthenticationClient>();
            container.Register<IFileEncrypter, FileEncrypter>();
            container.Register<IFileWriter, FileWriter>();
            container.Register<IFileManager, FileManager>();
            container.Collection.Register(typeof(IEntityHandler<>), typeof(IEntityHandler<>).Assembly);
        }

        private static void RegisterProviders(Container container)
        {
            /** General */
            container.Register<IUserProvider, UserProvider>(Lifestyle.Scoped);
            container.Register<IPermissionProvider, PermissionProvider>(Lifestyle.Scoped);
            container.Register<IRoleProvider, RoleProvider>(Lifestyle.Scoped);
        }

        private static void RegisterServices(Container container)
        {
        }

        private static void RegisterViewModelProcessors(Container container)
        {
        }

        private static void RegisterViewEngine(Container container)
        {
            var basePath = ApplicationEnvironment.ApplicationBasePath;
            var directorySeparator = Path.DirectorySeparatorChar;
            var index = basePath.IndexOf("bin", StringComparison.Ordinal);
            basePath = basePath.Substring(0, index <= 0 ? basePath.Length : index) + $"Views{directorySeparator}";

            var engine = new RazorLightEngineBuilder()
                .UseFilesystemProject(basePath)
                .UseMemoryCachingProvider()
                .Build();

            container.RegisterSingleton<IRazorLightEngine>(() => engine);
        }

        private static void RegisterTasks(Container container)
        {
            container.Register<ITaskFactory, TaskFactory>();
            container.Collection.Register(
                typeof(ITask),
                new List<Type>
                {
                });
        }
    }
}
