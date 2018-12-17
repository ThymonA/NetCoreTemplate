namespace NetCoreTemplate.Web.Controllers.Base
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using NetCoreTemplate.Authentication;
    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Interfaces;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Interfaces;
    using NetCoreTemplate.ViewModels.Interfaces;
    using NetCoreTemplate.Web.Extensions;

    using Microsoft.AspNetCore.Mvc;

    public abstract class BaseController : Controller
    {
        private static CultureInfo cultureInfo;

        private readonly IServiceContainer serviceContainer;
        private readonly IBaseProvider<Language> languageProvider;

        protected BaseController(IServiceContainer serviceContainer)
        {
            this.serviceContainer = serviceContainer;
            this.languageProvider = serviceContainer.GetService<IBaseProvider<Language>>();
        }

        protected int UserId
        {
            get
            {
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return default(int);
                }

                return HttpContext.User.Claims
                    .First(x => x.Type == Claims.UserId)
                    .Value
                    .ToInt();
            }
        }

        protected int LanguageId
        {
            get
            {
                var languageCookie = HttpContext.Request.Cookies
                    .FirstOrDefault(x => x.Key == "language");

                var languageId = 1;

                if (!languageCookie.IsNullOrDefault())
                {
                    languageId = languageCookie.Value.ToInt();
                }

                var languague = languageProvider
                    .GetEntity(x => x.Id == languageId);

                if (languague.IsNullOrDefault())
                {
                    return 1;
                }
                else
                {
                    return languague.Id;
                }
            }
        }

        protected CultureInfo CultureInfo()
        {
            if (cultureInfo.IsNullOrDefault())
            {
                var cultureCode = languageProvider
                    .GetEntity(x => x.Id == LanguageId)
                    .CultureCode;

                cultureInfo = new CultureInfo(cultureCode);
            }

            return cultureInfo;
        }

        protected ILoader<TViewModel> GetLoader<TViewModel>()
            where TViewModel : class, IBaseViewModel
        {
            return serviceContainer.GetService<ILoader<TViewModel>>();
        }

        protected ILoader<TViewModel, TRequestModel> GetLoader<TViewModel, TRequestModel>()
            where TViewModel : class, IBaseViewModel
        {
            return serviceContainer.GetService<ILoader<TViewModel, TRequestModel>>();
        }

        protected IListLoader<TViewModel> GetListLoader<TViewModel>()
            where TViewModel : class, IBaseViewModel
        {
            return serviceContainer.GetService<IListLoader<TViewModel>>();
        }

        protected IReloader<TViewModel> GetReloader<TViewModel>()
            where TViewModel : class, IBaseViewModel
        {
            var reloaders = serviceContainer.GetServices<IReloader<TViewModel>>();

            return reloaders.First();
        }

        protected IReloader<TViewModel> GetLatestReloader<TViewModel>()
            where TViewModel : class, IBaseViewModel
        {
            var reloaders = serviceContainer.GetServices<IReloader<TViewModel>>();

            return reloaders.Last();
        }

        protected IValidator<TViewModel> GetValidator<TViewModel>()
            where TViewModel : class, IBaseViewModel
        {
            return serviceContainer.GetService<IValidator<TViewModel>>();
        }

        protected IProcessor<TViewModel> GetProcessor<TViewModel>()
            where TViewModel : class, IBaseViewModel
        {
            return serviceContainer.GetService<IProcessor<TViewModel>>();
        }

        protected async Task<IActionResult> ProcessViewModel<TViewModel>(
            TViewModel viewModel,
            Func<TViewModel, Task> customFunc,
            Func<IActionResult> successAction,
            Func<IActionResult> failureAction)
            where TViewModel : class, IBaseViewModel
        {
            try
            {
                var validator = GetValidator<TViewModel>();
                var validationResult = validator.Validate(viewModel);

                ModelState.AddValidationResult(validationResult);

                if (!ModelState.IsValid)
                {
                    return failureAction();
                }

                await customFunc(viewModel);

                return successAction();
            }
            catch (Exception)
            {
                return failureAction();
            }
        }

        protected IActionResult ProcessViewModel<TViewModel>(
            TViewModel viewModel,
            Func<TViewModel, IActionResult> successAction,
            Func<TViewModel, IActionResult> failureAction)
            where TViewModel : class, IBaseViewModel
        {
            return ProcessViewModel(
                GetValidator<TViewModel>(),
                GetProcessor<TViewModel>(),
                viewModel,
                successAction,
                failureAction);
        }

        protected IActionResult ProcessViewModel<TViewModel>(
            IValidator<TViewModel> validator,
            IProcessor<TViewModel> processor,
            TViewModel viewModel,
            Func<TViewModel, IActionResult> successAction,
            Func<TViewModel, IActionResult> failureAction)
            where TViewModel : class, IBaseViewModel
        {
            try
            {
                var validationResult = validator.Validate(viewModel);

                ModelState.AddValidationResult(validationResult);

                if (!ModelState.IsValid)
                {
                    return failureAction(viewModel);
                }

                processor.Process(viewModel);

                return successAction(viewModel);
            }
            catch (Exception e)
            {
                return failureAction(viewModel);
            }
        }
    }
}
