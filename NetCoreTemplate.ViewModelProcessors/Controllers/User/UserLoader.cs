namespace NetCoreTemplate.ViewModelProcessors.Controllers.User
{
    using System.Collections.Generic;
    using System.Linq;

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.DAL.Permissions;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.General;
    using NetCoreTemplate.ViewModels.Models;

    public sealed class UserLoader : BaseAuthenticationLoader<UserViewModel, int>
    {
        private readonly IUserProvider userProvider;
        private readonly IRoleProvider roleProvider;
        private readonly IPermissionProvider permissionProvider;

        public UserLoader(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            this.userProvider = serviceContainer.GetService<IUserProvider>();
            this.roleProvider = serviceContainer.GetService<IRoleProvider>();
            this.permissionProvider = serviceContainer.GetService<IPermissionProvider>();
        }

        protected override TranslationSettings TranslationSettings => new TranslationSettings("Dashboard", "User");

        protected override UserViewModel CreateViewModel(int param)
        {
            var user = userProvider.GetUserById(param);

            UserViewModel vm;

            if (user.IsNullOrDefault())
            {
                vm = new UserViewModel
                {
                    Id = 0,
                    Active = true,
                };

                LoadRoles(vm, new List<Role>());

                return vm;
            }

            vm = new UserViewModel
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Active = user.Active
            };

            var userRoles = roleProvider.GetRolesForUser(user.Id);

            LoadRoles(vm, userRoles);

            return vm;
        }

        private void LoadRoles(UserViewModel viewModel, IReadOnlyCollection<Role> userRoles)
        {
            var roles = roleProvider.GetAll()
                .OrderBy(x => x.Name)
                .Where(x => x.Active)
                .ToList();
            var userRoleViewModels = roles.Select(
                role => new RoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name,
                    Active = userRoles.Any(y => y.Id == role.Id)
                });

            viewModel.Roles = new List<RoleViewModel>();
            viewModel.RoleNames.AddRange(userRoles.Select(x => x.Name));
            viewModel.Roles.AddRange(userRoleViewModels);
        }

        protected override UserViewModel ReloadViewModel(UserViewModel viewModel)
        {
            var permissions = permissionProvider
                .GetPermissions(UserId)
                .Select(x => x.Action)
                .ToList();

            if (!permissions.Contains(Permissions.GetActionKey(Module.Dashboard, Type.User, Action.Edit))
                && (viewModel.Id <= default(int) || UserId != viewModel.Id))
            {
                return CreateViewModel(UserId);
            }

            var roles = roleProvider.GetAll()
                .OrderBy(x => x.Name)
                .Where(x => x.Active)
                .ToList();
            var userRoleViewModels = roles.Select(
                role => new RoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name,
                    Active = viewModel.Roles
                        .FirstOrDefault(x => x.Id == role.Id)?.Active ?? false
                }).ToList();

            viewModel.RoleNames.AddRange(userRoleViewModels.Where(x => x.Active).Select(x => x.Name));
            viewModel.Roles = new List<RoleViewModel>();
            viewModel.Roles.AddRange(userRoleViewModels);

            return viewModel;
        }
    }
}
