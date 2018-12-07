namespace NetCoreTemplate.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using NetCoreTemplate.DAL.Permissions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModels.General;
    using NetCoreTemplate.Web.Controllers.Attributes;
    using NetCoreTemplate.Web.Controllers.Base;

    public class RoleController : BaseController
    {
        public RoleController(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }

        [HttpGet("roles")]
        [ClaimAuthorization(Module.Dashboard, Type.Roles, Action.View)]
        public IActionResult List()
        {
            var loader = GetListLoader<RoleListViewModel>();

            return View("List", loader.Load());
        }

        [HttpGet("role/{id?}")]
        [ClaimAuthorization(Module.Dashboard, Type.Role, Action.Edit)]
        public IActionResult Details(int? id)
        {
            var loader = GetLoader<RoleViewModel, int>();

            return View("Details", loader.Load(id ?? default(int)));
        }

        [HttpPost("role/{id?}")]
        [ClaimAuthorization(Module.Dashboard, Type.Role, Action.Edit)]
        public IActionResult Details(RoleViewModel viewModel)
        {
            return ProcessViewModel(
                viewModel,
                x => RedirectToAction("List", "Role"),
                x =>
                {
                    var reloader = GetReloader<RoleViewModel>();
                    return View("Details", reloader.Reload(viewModel));
                });
        }
    }
}
