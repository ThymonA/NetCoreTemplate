namespace NetCoreTemplate.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModels.Controllers.Dashboard;
    using NetCoreTemplate.Web.Controllers.Base;

    public class DashboardController : BaseController
    {
        public DashboardController(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }

        public IActionResult Index()
        {
            var loader = GetLoader<HomeViewModel>();

            return View("Index", loader.Load());
        }
    }
}
