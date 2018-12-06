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
    }
}
