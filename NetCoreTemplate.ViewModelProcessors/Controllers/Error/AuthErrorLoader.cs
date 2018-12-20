namespace NetCoreTemplate.ViewModelProcessors.Controllers.Error
{
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.Controllers.Error;
    using NetCoreTemplate.ViewModels.Models;

    public sealed class AuthErrorLoader : BaseAuthenticationLoader<AuthErrorViewModel, string>
    {
        public AuthErrorLoader(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }

        protected override TranslationSettings TranslationSettings => new TranslationSettings("Dashboard", "Admin");

        protected override AuthErrorViewModel CreateViewModel(string code)
        {
            var status = code.ToInt(404);

            return new AuthErrorViewModel { Code = status };
        }

        protected override AuthErrorViewModel ReloadViewModel(AuthErrorViewModel viewModel)
        {
            return viewModel;
        }
    }
}
