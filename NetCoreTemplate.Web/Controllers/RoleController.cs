namespace NetCoreTemplate.Web.Controllers
{
    using System;
    using System.Transactions;

    using Microsoft.AspNetCore.Mvc;

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.DAL.Permissions;
    using NetCoreTemplate.Providers.Interfaces;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.Services.Interfaces;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.Interfaces.Managers;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.SharedKernel.Validation;
    using NetCoreTemplate.ViewModels.General;
    using NetCoreTemplate.Web.Controllers.Attributes;
    using NetCoreTemplate.Web.Controllers.Base;
    using NetCoreTemplate.Web.Extensions;

    using Action = NetCoreTemplate.DAL.Permissions.Action;
    using Type = NetCoreTemplate.DAL.Permissions.Type;

    public class RoleController : BaseController
    {
        private readonly ITranslationManager translationManager;
        private readonly IRoleProvider roleProvider;
        private readonly IBaseProvider<RolePermission> rolePermissionProvider;
        private readonly IBaseProvider<UserRole> userRoleProvider;
        private readonly IBaseService<Role> roleService;
        private readonly IBaseService<RolePermission> rolePermissionService;
        private readonly IBaseService<UserRole> userRoleService;

        public RoleController(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            this.translationManager = serviceContainer.GetService<ITranslationManager>();
            this.roleProvider = serviceContainer.GetService<IRoleProvider>();
            this.rolePermissionProvider = serviceContainer.GetService<IBaseProvider<RolePermission>>();
            this.userRoleProvider = serviceContainer.GetService<IBaseProvider<UserRole>>();
            this.roleService = serviceContainer.GetService<IBaseService<Role>>();
            this.rolePermissionService = serviceContainer.GetService<IBaseService<RolePermission>>();
            this.userRoleService = serviceContainer.GetService<IBaseService<UserRole>>();
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

        [HttpGet("role/delete/{id}")]
        [ClaimAuthorization(Module.Dashboard, Type.Role, Action.Delete)]
        public IActionResult Delete(int id)
        {
            var role = roleProvider.GetEntity(x => x.Id == id);

            if (role.IsNullOrDefault())
            {
                return RedirectToAction("List", "Role");
            }

            try
            {
                var rolePermissions = rolePermissionProvider.Where(x => x.Role_Id == role.Id);
                var userRoles = userRoleProvider.Where(x => x.Role_Id == role.Id);

                using (var transaction = new TransactionScope(TransactionScopeOption.Required))
                {
                    rolePermissionService.DeleteRange(rolePermissions);
                    userRoleService.DeleteRange(userRoles);
                    roleService.Delete(role);

                    transaction.Complete();
                }

                return RedirectToAction("List", "Role");
            }
            catch (Exception)
            {
                var validationResult = new ValidationResult<UserViewModel>();
                var loader = GetLoader<RoleViewModel, int>();
                var viewModel = loader.Load(role.Id);

                validationResult.AddError(
                    m => m.Id,
                    translationManager.GetTranslationLabel(LanguageId, "Dashboard:Role:NotDeleted"));

                ModelState.Clear();
                ModelState.AddValidationResult(validationResult);

                return View("Details", viewModel);
            }
        }
    }
}
