namespace NetCoreTemplate.ViewModelProcessors.Controllers.Dashboard
{
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.Controllers.Dashboard;
    using NetCoreTemplate.ViewModels.Models;

    public class HomeLoader : BaseAuthenticationLoader<HomeViewModel>
    {
        public HomeLoader(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }

        protected override TranslationSettings TranslationSettings => new TranslationSettings("Dashboard", "Home");

        protected override HomeViewModel CreateViewModel()
        {
            return new HomeViewModel();
        }

        protected override HomeViewModel ReloadViewModel(HomeViewModel viewModel)
        {
            return viewModel;
        }
    }
}
