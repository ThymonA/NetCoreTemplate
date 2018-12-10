namespace NetCoreTemplate.ViewModelProcessors.Base
{
    using System.Linq;

    using NetCoreTemplate.Authentication;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.ViewModelProcessors.Interfaces;
    using NetCoreTemplate.ViewModels.Interfaces;

    using Microsoft.AspNetCore.Http;

    using NetCoreTemplate.SharedKernel.ServiceContainer;

    public abstract class BaseProcessor<TViewModel> : IProcessor<TViewModel>
        where TViewModel : class, IBaseViewModel
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        protected BaseProcessor(IServiceContainer serviceContainer)
        {
            this.httpContextAccessor = serviceContainer.GetService<IHttpContextAccessor>();
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

        public abstract void Process(TViewModel viewModel);
    }
}
