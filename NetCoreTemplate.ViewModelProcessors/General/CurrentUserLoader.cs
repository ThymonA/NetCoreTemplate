namespace NetCoreTemplate.ViewModelProcessors.General
{
    using System;
    using System.Linq;

    using NetCoreTemplate.Authentication;
    using NetCoreTemplate.DAL.Permissions;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.General;
    using NetCoreTemplate.ViewModels.Models;

    using Action = NetCoreTemplate.DAL.Permissions.Action;
    using Type = NetCoreTemplate.DAL.Permissions.Type;

    public sealed class CurrentUserLoader : BaseLoader<UserViewModel, string>
    {
        private readonly IUserProvider userProvider;
        private readonly IAuthenticationClient authenticationClient;
        private readonly IRoleProvider roleProvider;
        private readonly IPermissionProvider permissionProvider;

        public CurrentUserLoader(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            this.userProvider = serviceContainer.GetService<IUserProvider>();
            this.authenticationClient = serviceContainer.GetService<IAuthenticationClient>();
            this.roleProvider = serviceContainer.GetService<IRoleProvider>();
            this.permissionProvider = serviceContainer.GetService<IPermissionProvider>();
        }

        protected override TranslationSettings TranslationSettings => new TranslationSettings("Dashboard", "User");

        protected override UserViewModel CreateViewModel(string param)
        {
            var user = userProvider.GetUserById(param.ToInt());

            if (user.IsNullOrDefault())
            {
                return new UserViewModel
                {
                    Id = 0,
                };
            }

            var remoteInfo = authenticationClient
                .GetUserInformation(user);

            var roles = roleProvider.GetRolesForUser(user.Id);

            var vm = new UserViewModel
            {
                Id = user.Id,
                Firstname = remoteInfo.Firstname,
                Lastname = remoteInfo.Lastname,
                Email = remoteInfo.Email,
                ResetToken = user.ResetToken,
                ResetTokenDate = user.ResetTokenDate ?? DateTime.MinValue
            };

            vm.RoleNames.AddRange(roles.Select(x => x.Name));

            return vm;
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
                return CreateViewModel(UserId.ToString());
            }

            var roles = roleProvider.GetAll()
                .OrderBy(x => x.Name)
                .Where(x => x.Active)
                .ToList();
            var userRoles = roleProvider.GetRolesForUser(viewModel.Id);
            var userRoleViewModels = roles.Select(
                role => new RoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name,
                    Active = userRoles.Any(y => y.Id == role.Id)
                });

            viewModel.RoleNames.AddRange(userRoles.Select(x => x.Name));
            viewModel.Roles.AddRange(userRoleViewModels);

            return viewModel;
        }
    }
}
