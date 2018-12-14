namespace NetCoreTemplate.Web.Controllers
{
    using System;
    using System.Linq;
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
    using NetCoreTemplate.ViewModels.Controllers.User;
    using NetCoreTemplate.ViewModels.General;
    using NetCoreTemplate.Web.Controllers.Attributes;
    using NetCoreTemplate.Web.Controllers.Base;
    using NetCoreTemplate.Web.Extensions;

    using Action = NetCoreTemplate.DAL.Permissions.Action;
    using Type = NetCoreTemplate.DAL.Permissions.Type;

    public class UserController : BaseController
    {
        private readonly ITranslationManager translationManager;
        private readonly IPermissionProvider permissionProvider;
        private readonly IUserProvider userProvider;
        private readonly IBaseProvider<UserRole> userRoleProvider;
        private readonly IBaseService<User> userService;
        private readonly IBaseService<UserRole> userRoleService;

        public UserController(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            this.translationManager = serviceContainer.GetService<ITranslationManager>();
            this.permissionProvider = serviceContainer.GetService<IPermissionProvider>();
            this.userProvider = serviceContainer.GetService<IUserProvider>();
            this.userRoleProvider = serviceContainer.GetService<IBaseProvider<UserRole>>();
            this.userService = serviceContainer.GetService<IBaseService<User>>();
            this.userRoleService = serviceContainer.GetService<IBaseService<UserRole>>();
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
                    var reloader = GetLatestReloader<UserViewModel>();
                    return View("Details", reloader.Reload(viewModel));
                });
        }

        [HttpGet("user/delete/{id}")]
        [ClaimAuthorization(Module.Dashboard, Type.User, Action.Delete)]
        public IActionResult Delete(int id)
        {
            var user = userProvider.GetEntity(x => x.Id == id);

            if (user.IsNullOrDefault())
            {
                return RedirectToAction("List", "User");
            }

            try
            {
                var userRoles = userRoleProvider.Where(x => x.User_Id == user.Id);

                using (var transaction = new TransactionScope(TransactionScopeOption.Required))
                {
                    userRoleService.DeleteRange(userRoles);
                    userService.Delete(user);

                    transaction.Complete();
                }

                return RedirectToAction("List", "User");
            }
            catch (Exception)
            {
                var validationResult = new ValidationResult<UserViewModel>();
                var loader = GetLoader<UserViewModel, int>();
                var viewModel = loader.Load(user.Id);

                validationResult.AddError(
                    m => m.Id,
                    translationManager.GetTranslationLabel(LanguageId, "Dashboard:User:NotDeleted"));

                ModelState.Clear();
                ModelState.AddValidationResult(validationResult);

                return View("Details", viewModel);
            }
        }
    }
}
