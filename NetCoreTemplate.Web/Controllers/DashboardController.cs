namespace NetCoreTemplate.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModels.Controllers.Admin;
    using NetCoreTemplate.ViewModels.Controllers.Dashboard;
    using NetCoreTemplate.Web.Controllers.Base;

    public class DashboardController : BaseController
    {
        public DashboardController(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }

        [AllowAnonymous]
        [HttpGet("")]
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var homeLoader = GetLoader<HomeViewModel>();

                return View("Index", homeLoader.Load());
            }

            var loginLoader = GetLoader<SignInViewModel>();

            return View("../Admin/SignIn", loginLoader.Load());
        }
    }
}
