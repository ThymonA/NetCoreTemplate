namespace NetCoreTemplate.ViewModelProcessors.Base
{
    using System.Collections.Generic;
    using System.Linq;

    using NetCoreTemplate.Authentication;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Interfaces;
    using NetCoreTemplate.ViewModels.General;
    using NetCoreTemplate.ViewModels.Interfaces;

    using Microsoft.AspNetCore.Http;

    public abstract class BaseAuthenticationLoader : BaseLoader
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILoader<UserViewModel, int> userLoader;
        private readonly IPermissionProvider permissionProvider;

        protected BaseAuthenticationLoader(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            httpContextAccessor = serviceContainer.GetService<IHttpContextAccessor>();
            userLoader = serviceContainer.GetService<ILoader<UserViewModel, int>>();
            permissionProvider = serviceContainer.GetService<IPermissionProvider>();
        }

        protected UserViewModel CurrentUser { get; set; }

        protected List<string> Actions { get; set; }

        protected void GetUserInformation()
        {
            var httpContext = httpContextAccessor.HttpContext;

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return;
            }

            var userId = httpContext.User.Claims
                .First(x => x.Type == Claims.UserId)
                .Value
                .ToInt();

            var permissions = permissionProvider
                .GetPermissions(userId);

            CurrentUser = userLoader.Load(userId);
            Actions = permissions.Select(x => x.Action).ToList();
        }

        protected void AddUserInformation<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IBaseAuthenticationViewModel
        {
            viewModel.User = CurrentUser;
            viewModel.Actions = Actions;
        }
    }

    public abstract class BaseAuthenticationLoader<TViewModel> : BaseAuthenticationLoader, ILoader<TViewModel>, IReloader<TViewModel>
        where TViewModel : class, IBaseAuthenticationViewModel
    {
        protected BaseAuthenticationLoader(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }

        public TViewModel Load()
        {
            GetUserInformation();
            LoadLanguage();
            LoadSystemUser();
            Label = GetLabels();
            Config = GetConfig();

            var viewModel = CreateViewModel();

            return FinalizeViewModel(viewModel);
        }

        public TViewModel Reload(TViewModel viewModel)
        {
            GetUserInformation();
            LoadLanguage();
            LoadSystemUser();
            Label = GetLabels();
            Config = GetConfig();

            var reloadedViewModel = ReloadViewModel(viewModel);

            return FinalizeViewModel(reloadedViewModel);
        }

        protected abstract TViewModel CreateViewModel();

        protected abstract TViewModel ReloadViewModel(TViewModel viewModel);

        protected new T FinalizeViewModel<T>(T viewModel)
            where T : class, IBaseAuthenticationViewModel
        {
            AddLanguague(viewModel);
            AddLabels(viewModel);
            AddConfig(viewModel);
            AddPath(viewModel);
            AddUserInformation(viewModel);
            AddSystemUser(viewModel);

            return viewModel;
        }
    }

    public abstract class BaseAuthenticationIntLoader<TViewModel> : BaseAuthenticationLoader, ILoader<TViewModel, int>
        where TViewModel : class, IBaseAuthenticationViewModel
    {
        protected BaseAuthenticationIntLoader(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }

        public TViewModel Load(int param)
        {
            GetUserInformation();
            LoadLanguage();
            LoadSystemUser();
            Label = GetLabels();
            Config = GetConfig();

            var viewModel = CreateViewModel(param);

            return FinalizeViewModel(viewModel);
        }

        protected abstract TViewModel CreateViewModel(int param);

        protected new T FinalizeViewModel<T>(T viewModel)
            where T : class, IBaseAuthenticationViewModel
        {
            AddLanguague(viewModel);
            AddLabels(viewModel);
            AddConfig(viewModel);
            AddPath(viewModel);
            AddUserInformation(viewModel);
            AddSystemUser(viewModel);

            return viewModel;
        }
    }

    public abstract class BaseAuthenticationLoader<TViewModel, TRequestModel> : BaseAuthenticationLoader, ILoader<TViewModel, TRequestModel>, IReloader<TViewModel>
        where TViewModel : class, IBaseAuthenticationViewModel
    {
        protected BaseAuthenticationLoader(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }

        public TViewModel Load(TRequestModel param)
        {
            GetUserInformation();
            LoadLanguage();
            LoadSystemUser();
            Label = GetLabels();
            Config = GetConfig();

            var viewModel = CreateViewModel(param);

            return FinalizeViewModel(viewModel);
        }

        public TViewModel Reload(TViewModel viewModel)
        {
            GetUserInformation();
            LoadLanguage();
            LoadSystemUser();
            Label = GetLabels();
            Config = GetConfig();

            var reloadedViewModel = ReloadViewModel(viewModel);

            return FinalizeViewModel(reloadedViewModel);
        }

        protected abstract TViewModel CreateViewModel(TRequestModel param);

        protected abstract TViewModel ReloadViewModel(TViewModel viewModel);

        protected new T FinalizeViewModel<T>(T viewModel)
            where T : class, IBaseAuthenticationViewModel
        {
            AddLanguague(viewModel);
            AddLabels(viewModel);
            AddConfig(viewModel);
            AddPath(viewModel);
            AddUserInformation(viewModel);
            AddSystemUser(viewModel);

            return viewModel;
        }
    }
}
