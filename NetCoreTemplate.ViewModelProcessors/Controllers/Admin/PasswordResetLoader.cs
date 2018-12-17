namespace NetCoreTemplate.ViewModelProcessors.Controllers.Admin
{
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.Controllers.Admin;
    using NetCoreTemplate.ViewModels.Models;

    public sealed class PasswordResetLoader : BaseLoader<PasswordResetViewModel>
    {
        public PasswordResetLoader(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }

        protected override TranslationSettings TranslationSettings => new TranslationSettings("Dashboard", "SignIn");

        protected override PasswordResetViewModel CreateViewModel()
        {
            return new PasswordResetViewModel();
        }

        protected override PasswordResetViewModel ReloadViewModel(PasswordResetViewModel viewModel)
        {
            return viewModel;
        }
    }
}
