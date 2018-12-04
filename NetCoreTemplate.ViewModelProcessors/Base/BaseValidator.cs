namespace NetCoreTemplate.ViewModelProcessors.Base
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Http;

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Interfaces;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.SharedKernel.Validation;
    using NetCoreTemplate.ViewModelProcessors.Interfaces;
    using NetCoreTemplate.ViewModels.Interfaces;

    public abstract class BaseValidator<TViewModel> : IValidator<TViewModel>
        where TViewModel : class, IBaseViewModel
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IBaseProvider<Language> languageProvider;

        protected int LanguageId { get; set; }

        public abstract ValidationResult Validate(TViewModel viewModel);

        protected BaseValidator(IServiceContainer serviceContainer)
        {
            httpContextAccessor = serviceContainer.GetService<IHttpContextAccessor>();
            languageProvider = serviceContainer.GetService<IBaseProvider<Language>>();

            LoadLanguage();
        }

        protected void LoadLanguage()
        {
            try
            {
                var languageCookie =
                    httpContextAccessor.HttpContext
                        .Request.Cookies
                        .FirstOrDefault(x => x.Key == "language");

                var languageId = 1;

                if (!languageCookie.IsNullOrDefault())
                {
                    languageId = languageCookie.Value.ToInt();
                }

                var languague = languageProvider.GetEntity(x => x.Id == languageId);

                if (languague.IsNullOrDefault())
                {
                    LanguageId = 1;
                }
                else
                {
                    LanguageId = languague.Id;
                }
            }
            catch (Exception)
            {
                LanguageId = 1;
            }
        }
    }
}
