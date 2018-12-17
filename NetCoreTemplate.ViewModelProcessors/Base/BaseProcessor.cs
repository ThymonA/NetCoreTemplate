namespace NetCoreTemplate.ViewModelProcessors.Base
{
    using System;
    using System.Linq;

    using NetCoreTemplate.Authentication;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.ViewModelProcessors.Interfaces;
    using NetCoreTemplate.ViewModels.Interfaces;

    using Microsoft.AspNetCore.Http;

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Interfaces;
    using NetCoreTemplate.SharedKernel.ServiceContainer;

    public abstract class BaseProcessor<TViewModel> : IProcessor<TViewModel>
        where TViewModel : class, IBaseViewModel
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IBaseProvider<Language> languageProvider;

        protected int LanguageId { get; set; }

        protected BaseProcessor(IServiceContainer serviceContainer)
        {
            this.httpContextAccessor = serviceContainer.GetService<IHttpContextAccessor>();
            this.languageProvider = serviceContainer.GetService<IBaseProvider<Language>>();

            LoadLanguage();
        }

        protected int UserId
        {
            get
            {
                var httpContext = httpContextAccessor.HttpContext;

                if (!httpContext.User.Identity.IsAuthenticated)
                {
                    return default(int);
                }

                return httpContext.User.Claims
                    .First(x => x.Type == Claims.UserId)
                    .Value
                    .ToInt();
            }
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

        public abstract void Process(TViewModel viewModel);
    }
}
