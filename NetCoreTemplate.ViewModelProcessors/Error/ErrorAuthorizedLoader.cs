namespace NetCoreTemplate.ViewModelProcessors.Error
{
    using System.Collections.Generic;

    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.Error;
    using NetCoreTemplate.ViewModels.Models;

    public class ErrorAuthorizedLoader : BaseAuthenticationLoader<ErrorAuthorizedViewModel, int>
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

        public ErrorAuthorizedLoader(
            IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }

        protected override TranslationSettings TranslationSettings => new TranslationSettings("Admin", "SignIn");

        protected override ErrorAuthorizedViewModel CreateViewModel(int param)
        {
            var codeExists = CodeDescription.ContainsKey(param);

            return new ErrorAuthorizedViewModel
            {
                Code = codeExists ? param.ToString() : "500",
                Description = codeExists ? CodeDescription[param] : CodeDescription[500]
            };
        }

        protected override ErrorAuthorizedViewModel ReloadViewModel(ErrorAuthorizedViewModel viewModel)
        {
            return CreateViewModel(viewModel.Code.ToInt());
        }
    }
}
