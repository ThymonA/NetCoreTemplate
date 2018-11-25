namespace NetCoreTemplate.ViewModelProcessors.Error
{
    using System.Collections.Generic;

    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModelProcessors.Interfaces;
    using NetCoreTemplate.ViewModels.Controllers.Admin;
    using NetCoreTemplate.ViewModels.Error;
    using NetCoreTemplate.ViewModels.Models;

    public class ErrorAnonymousLoader : BaseLoader<ErrorAnonymousViewModel, int>
    {
        private static readonly Dictionary<int, string> CodeDescription = new Dictionary<int, string>
        {
            { 400, "Foute aanvraag" },
            { 401, "U moet eerst ingelogd zijn om deze pagina te bekijken" },
            { 403, "U heeft geen toestemming om deze pagina te bezoeken" },
            { 404, "Wij hebben uw pagina niet kunnen vinden" },
            { 500, "Server kan uw aanvraag niet verwerken" },
            { 503, "Server is niet beschikbaar om uw aanvraag te verwerken" },
            { 504, "Tweede server is niet te bereiken" }
        };

        private readonly ILoader<SignInViewModel> signInLoader;

        public ErrorAnonymousLoader(
            IServiceContainer serviceContainer,
            ILoader<SignInViewModel> signInLoader)
            : base(serviceContainer)
        {
            this.signInLoader = signInLoader;
        }

        protected override TranslationSettings TranslationSettings => new TranslationSettings("Admin", "SignIn");

        protected override ErrorAnonymousViewModel CreateViewModel(int param)
        {
            var codeExists = CodeDescription.ContainsKey(param);

            return new ErrorAnonymousViewModel
            {
                Code = codeExists ? param.ToString() : "500",
                Description = codeExists ? CodeDescription[param] : CodeDescription[500],
                SignInViewModel = signInLoader.Load()
            };
        }

        protected override ErrorAnonymousViewModel ReloadViewModel(ErrorAnonymousViewModel viewModel)
        {
            return CreateViewModel(viewModel.Code.ToInt());
        }
    }
}
