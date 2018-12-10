namespace NetCoreTemplate.ViewModelProcessors.Controllers.User
{
    using System.Linq;
    using System.Transactions;

    using Microsoft.EntityFrameworkCore;

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.DAL.Permissions;
    using NetCoreTemplate.Providers.Interfaces;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.Services.Interfaces;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.General;

    public sealed class UserProcessor : BaseProcessor<UserViewModel>
    {
        private readonly IUserProvider userProvider;
        private readonly IRoleProvider roleProvider;
        private readonly IPermissionProvider permissionProvider;
        private readonly IBaseProvider<UserRole> userRoleProvider;
        private readonly IBaseService<User> userService;
        private readonly IBaseService<UserRole> userRoleService;

        public UserProcessor(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            this.userProvider = serviceContainer.GetService<IUserProvider>();
            this.roleProvider = serviceContainer.GetService<IRoleProvider>();
            this.permissionProvider = serviceContainer.GetService<IPermissionProvider>();
            this.userRoleProvider = serviceContainer.GetService<IBaseProvider<UserRole>>();
            this.userService = serviceContainer.GetService<IBaseService<User>>();
            this.userRoleService = serviceContainer.GetService<IBaseService<UserRole>>();
        }

        public override void Process(UserViewModel viewModel)
        {
            var currentUserPermissions = permissionProvider
                .GetPermissions(UserId)
                .Select(x => x.Action)
                .ToList();
            var user = userProvider.GetEntity(x => x.Id == viewModel.Id);
            var roles = roleProvider.GetAllAsList();

            using (var transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                if (user.IsNullOrDefault())
                {
                    if (!currentUserPermissions.Contains(Permissions.GetActionKey(Module.Dashboard, Type.User, Action.Edit)))
                    {
                        return;
                    }

                    user = new User
                    {
                        Email = viewModel.Email,
                        Password = string.Empty,
                        Firstname = viewModel.Firstname,
                        Lastname = viewModel.Lastname,
                        ResetToken = string.Empty,
                        ResetTokenDate = null,
                        Active = viewModel.Active
                    };

                    userService.Add(user);
                }
                else
                {
                    user.Firstname = viewModel.Firstname;
                    user.Lastname = viewModel.Lastname;

                    if (currentUserPermissions.Contains(Permissions.GetActionKey(Module.Dashboard, Type.User, Action.Edit)))
                    {
                        user.Email = viewModel.Email;
                        user.Active = viewModel.Active;
                    }

                    userService.AddOrUpdate(user);
                    var currentUserRoles = userRoleProvider.Where(x => x.User_Id == user.Id);
                    userRoleService.DeleteRange(currentUserRoles);
                }

                var selectedRoles = viewModel.Roles.Where(x => x.Active);
                var userRoles = roles.Where(x => selectedRoles.Any(y => y.Id == x.Id))
                    .Select(role => new UserRole
                    {
                        User_Id = user.Id,
                        User = user,
                        Role_Id = role.Id,
                        Role = role
                    }).ToList();

                userRoleService.AddRange(userRoles);

                transaction.Complete();
            }
        }
    }
}
