namespace NetCoreTemplate.Web.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using NetCoreTemplate.DAL.Permissions;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Interfaces;
    using NetCoreTemplate.ViewModels.Controllers.User;
    using NetCoreTemplate.ViewModels.General;
    using NetCoreTemplate.Web.Controllers.Attributes;
    using NetCoreTemplate.Web.Controllers.Base;

    public class UserController : BaseController
    {
        private readonly IServiceContainer serviceContainer;
        private readonly IPermissionProvider permissionProvider;

        public UserController(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            this.serviceContainer = serviceContainer;
            this.permissionProvider = serviceContainer.GetService<IPermissionProvider>();
        }

        [HttpGet("users")]
        [ClaimAuthorization(Module.Dashboard, Type.Roles, Action.View)]
        public IActionResult List()
        {
            var loader = GetListLoader<UserListViewModel>();

            return View("List", loader.Load());
        }

        [HttpGet("user/{id?}")]
        [ClaimAuthorization(Module.Dashboard, Type.Role, Action.Edit, Action.Own)]
        public IActionResult Details(int? id)
        {
            var permissions = permissionProvider
                .GetPermissions(UserId)
                .Select(x => x.Action)
                .ToList();

            if (!permissions.Contains(Permissions.GetActionKey(Module.Dashboard, Type.User, Action.Edit))
                && ((id ?? default(int)) <= default(int) || UserId != (id ?? default(int))))
            {
                return Unauthorized();
            }

            var loader = GetLoader<UserViewModel, int>();

            return View("Details", loader.Load(id ?? default(int)));
        }

        [HttpPost("user/{id?}")]
        [ClaimAuthorization(Module.Dashboard, Type.Role, Action.Edit, Action.Own)]
        public IActionResult Details(UserViewModel viewModel)
        {
            var permissions = permissionProvider
                .GetPermissions(UserId)
                .Select(x => x.Action)
                .ToList();

            if (!permissions.Contains(Permissions.GetActionKey(Module.Dashboard, Type.User, Action.Edit))
                && (viewModel.Id <= default(int) || UserId != viewModel.Id))
            {
                return Unauthorized();
            }

            return ProcessViewModel(
                viewModel,
                x => RedirectToAction("List", "User"),
                x =>
                {
                    var reloader = serviceContainer.GetServices<IReloader<UserViewModel>>().Last();
                    return View("Details", reloader.Reload(viewModel));
                });
        }
    }
}
