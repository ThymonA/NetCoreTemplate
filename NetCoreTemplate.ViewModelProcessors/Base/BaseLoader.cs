namespace NetCoreTemplate.ViewModelProcessors.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NetCoreTemplate.Authentication;
    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Interfaces;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.SharedKernel.Dictionary;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.Interfaces.Managers;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Interfaces;
    using NetCoreTemplate.ViewModels.Interfaces;
    using NetCoreTemplate.ViewModels.Models;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;

    public abstract class BaseLoader
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ITranslationManager translationManager;
        private readonly IConfiguration configuration;
        private readonly IBaseProvider<Language> languageProvider;
        private readonly IUserProvider userProvider;

        protected BaseLoader(IServiceContainer serviceContainer)
        {
            httpContextAccessor = serviceContainer.GetService<IHttpContextAccessor>();
            translationManager = serviceContainer.GetService<ITranslationManager>();
            configuration = serviceContainer.GetService<IConfiguration>();
            languageProvider = serviceContainer.GetService<IBaseProvider<Language>>();
            userProvider = serviceContainer.GetService<IUserProvider>();
        }

        protected abstract TranslationSettings TranslationSettings { get; }

        protected TranslatedDictionary Label { get; set; }

        protected Dictionary<string, string> Config { get; set; }

        protected int SystemUser { get; set; }

        protected int LanguageId { get; set; }

        protected string Languague { get; set; }

        protected string CultureCode { get; set; }

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

        protected void LoadSystemUser()
        {
            var systemUser = userProvider.GetUserByEmail("system@NetCoreTemplate.nl");

            if (systemUser.IsNullOrDefault())
            {
                SystemUser = 0;
            }

            SystemUser = systemUser.Id;
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
                    Languague = "NL";
                    CultureCode = "nl-NL";
                }
                else
                {
                    LanguageId = languague.Id;
                    Languague = languague.Code;
                    CultureCode = languague.CultureCode;
                }
            }
            catch (Exception)
            {
                LanguageId = 1;
                Languague = "NL";
                CultureCode = "nl-NL";
            }
        }

        protected TranslatedDictionary GetLabels()
        {
            var settings = TranslationSettings;
            settings.AddSettings("General", "Dashboard");
            settings.AddSettings("Dashboard", "MainMenu");

            var dictionary = translationManager.GetTranslationLabels(LanguageId, settings.ModuleTypes);
            var translatedDictionary = new TranslatedDictionary();
            
            translatedDictionary.AddRange(dictionary);

            return translatedDictionary;
        }

        protected Dictionary<string, string> GetConfig()
        {
            var items = configuration.AsEnumerable();

            return items.ToDictionary(item => item.Key, item => item.Value);
        }

        protected TViewModel FinalizeViewModel<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IBaseViewModel
        {
            AddLanguague(viewModel);
            AddLabels(viewModel);
            AddConfig(viewModel);
            AddPath(viewModel);
            AddSystemUser(viewModel);

            return viewModel;
        }

        protected void AddLabels<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IBaseViewModel
        {
            viewModel.Label = Label;
        }

        protected void AddSystemUser<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IBaseViewModel
        {
            viewModel.SystemUser = SystemUser;
        }

        protected void AddConfig<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IBaseViewModel
        {
            viewModel.Config = Config;
        }

        protected void AddPath<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IBaseViewModel
        {
            try
            {
                viewModel.Path = httpContextAccessor.HttpContext.Request.Path.Value;
            }
            catch (Exception)
            {
                viewModel.Path = "/";
            }
        }

        protected void AddLanguague<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IBaseViewModel
        {
            viewModel.Language = Languague;
            viewModel.CultureCode = CultureCode;
        }
    }

    public abstract class BaseLoader<TViewModel> : BaseLoader, ILoader<TViewModel>, IReloader<TViewModel>
        where TViewModel : class, IBaseViewModel
    {
        protected BaseLoader(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }

        public TViewModel Load()
        {
            LoadLanguage();
            LoadSystemUser();
            Label = GetLabels();
            Config = GetConfig();

            var viewModel = CreateViewModel();

            return FinalizeViewModel(viewModel);
        }

        public TViewModel Reload(TViewModel viewModel)
        {
            LoadLanguage();
            LoadSystemUser();
            Label = GetLabels();
            Config = GetConfig();

            var reloadedViewModel = ReloadViewModel(viewModel);

            return FinalizeViewModel(reloadedViewModel);
        }

        protected abstract TViewModel CreateViewModel();

        protected abstract TViewModel ReloadViewModel(TViewModel viewModel);
    }

    public abstract class BaseLoader<TViewModel, TRequestModel> : BaseLoader, ILoader<TViewModel, TRequestModel>, IReloader<TViewModel>
        where TViewModel : class, IBaseViewModel
    {
        protected BaseLoader(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }

        public TViewModel Load(TRequestModel param)
        {
            LoadLanguage();
            LoadSystemUser();
            Label = GetLabels();
            Config = GetConfig();

            var viewModel = CreateViewModel(param);

            return FinalizeViewModel(viewModel);
        }

        public TViewModel Reload(TViewModel viewModel)
        {
            LoadLanguage();
            LoadSystemUser();
            Label = GetLabels();
            Config = GetConfig();

            var reloadedViewModel = ReloadViewModel(viewModel);

            return FinalizeViewModel(reloadedViewModel);
        }

        protected abstract TViewModel CreateViewModel(TRequestModel param);

        protected abstract TViewModel ReloadViewModel(TViewModel viewModel);
    }
}
