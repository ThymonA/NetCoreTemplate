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

    public abstract class BaseAuthenticationListLoader<TEntity, TListViewModel, TEntityViewModel> : 
        BaseLoaderList<TEntity, TListViewModel, TEntityViewModel>, 
        IListLoader<TListViewModel>, 
        IReloader<TListViewModel>
        where TEntity : class
        where TEntityViewModel : class, IBaseViewModel
        where TListViewModel : class, IBaseAuthenticationListViewModel<TEntityViewModel>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILoader<UserViewModel, int> userLoader;
        private readonly IPermissionProvider permissionProvider;

        protected BaseAuthenticationListLoader(IServiceContainer serviceContainer)
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

        public TListViewModel Load()
        {
            LoadLanguage();
            LoadContextInformation();
            GetUserInformation();

            Label = GetLabels();
            Config = GetConfig();

            var viewModel = CreateViewModel();

            ReloadContextInformation(viewModel);

            var data = RetrieveData(viewModel).Result;
            var viewModelData = data.Select(FillViewModel).ToList();

            viewModel.Data = viewModelData;

            return FinalizeViewModel(viewModel);
        }

        public TListViewModel Reload(TListViewModel viewModel)
        {
            return Load();
        }

        protected void AddUserInformation(TListViewModel viewModel)
        {
            viewModel.User = CurrentUser;
            viewModel.Actions = Actions;
        }

        protected new TListViewModel FinalizeViewModel(TListViewModel viewModel)
        {
            AddUserInformation(viewModel);
            AddLanguague(viewModel);
            AddLabels(viewModel);
            AddListInfo(viewModel);
            AddConfig(viewModel);
            AddPath(viewModel);
            AddPageSize(viewModel);
            AddBaseOrderBy(viewModel);

            return viewModel;
        }
    }
}
