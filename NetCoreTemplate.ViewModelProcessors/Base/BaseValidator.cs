namespace NetCoreTemplate.ViewModelProcessors.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Http;

    using NetCoreTemplate.Authentication;
    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Interfaces;
    using NetCoreTemplate.Providers.Interfaces.General;
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
        private readonly IPermissionProvider permissionProvider;

        protected int LanguageId { get; set; }

        protected virtual bool LoadCurrentPermissions { get; set; } = false;

        protected List<string> Actions { get; } = new List<string>();

        public abstract ValidationResult Validate(TViewModel viewModel);

        protected BaseValidator(IServiceContainer serviceContainer)
        {
            this.httpContextAccessor = serviceContainer.GetService<IHttpContextAccessor>();
            this.languageProvider = serviceContainer.GetService<IBaseProvider<Language>>();
            this.permissionProvider = serviceContainer.GetService<IPermissionProvider>();

            LoadLanguage();
            LoadUser();
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

        protected void LoadUser()
        {
            if (LoadCurrentPermissions)
            {
                var httpContext = httpContextAccessor.HttpContext;

                if (httpContext.IsNullOrDefault() || !httpContext.User.Identity.IsAuthenticated)
                {
                    return;
                }

                var userId = httpContext.User.Claims
                    .First(x => x.Type == Claims.UserId)
                    .Value
                    .ToInt();

                var permissions = permissionProvider
                    .GetPermissions(userId)
                    .Select(x => x.Action);

                Actions.AddRange(permissions);
            }
        }
    }
}
