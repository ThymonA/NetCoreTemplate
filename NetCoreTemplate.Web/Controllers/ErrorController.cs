namespace NetCoreTemplate.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModels.Controllers.Error;
    using NetCoreTemplate.Web.Controllers.Base;

    public class ErrorController : BaseController
    {
        public ErrorController(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }

        [HttpGet("error/{code?}")]
        [HttpPost("error/{code?}")]
        public IActionResult Error(string code)
        {
            code = string.IsNullOrWhiteSpace(code) ? "404" : code;

            if (User.Identity.IsAuthenticated)
            {
                var authErrorLoader = GetLoader<AuthErrorViewModel, string>();

                return View("AuthView", authErrorLoader.Load(code));
            }

            var baseErrorLoader = GetLoader<BaseErrorViewModel, string>();

            return View("BaseView", baseErrorLoader.Load(code));
        }
    }
}
