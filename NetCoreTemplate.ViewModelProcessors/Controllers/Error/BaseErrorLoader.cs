namespace NetCoreTemplate.ViewModelProcessors.Controllers.Error
{
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.Controllers.Error;
    using NetCoreTemplate.ViewModels.Models;

    public sealed class BaseErrorLoader : BaseLoader<BaseErrorViewModel, string>
    {
        public BaseErrorLoader(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }

        protected override TranslationSettings TranslationSettings => new TranslationSettings("Dashboard", "Admin");

        protected override BaseErrorViewModel CreateViewModel(string code)
        {
            var status = code.ToInt(404);

            return new BaseErrorViewModel { Code = status };
        }

        protected override BaseErrorViewModel ReloadViewModel(BaseErrorViewModel viewModel)
        {
            return viewModel;
        }
    }
}
