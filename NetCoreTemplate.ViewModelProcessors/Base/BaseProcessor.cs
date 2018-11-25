namespace NetCoreTemplate.ViewModelProcessors.Base
{
    using System.Linq;

    using NetCoreTemplate.Authentication;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.ViewModelProcessors.Interfaces;
    using NetCoreTemplate.ViewModels.Interfaces;

    using Microsoft.AspNetCore.Http;

    public abstract class BaseProcessor<TViewModel> : IProcessor<TViewModel>
        where TViewModel : class, IBaseViewModel
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        protected BaseProcessor(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
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
