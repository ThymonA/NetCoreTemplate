namespace NetCoreTemplate.ViewModelProcessors.Controllers.Admin
{
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.Controllers.Admin;
    using NetCoreTemplate.ViewModels.Models;

    public sealed class SignInLoader : BaseLoader<SignInViewModel>
    {
        public SignInLoader(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }

        protected override TranslationSettings TranslationSettings => new TranslationSettings("Dashboard", "SignIn", "Admin");

        protected override SignInViewModel CreateViewModel()
        {
            return new SignInViewModel();
        }

        protected override SignInViewModel ReloadViewModel(SignInViewModel viewModel)
        {
            viewModel.Password = string.Empty;

            return viewModel;
        }
    }
}
