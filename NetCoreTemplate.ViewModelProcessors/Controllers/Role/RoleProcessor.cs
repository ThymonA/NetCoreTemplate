namespace NetCoreTemplate.ViewModelProcessors.Controllers.Role
{
    using System.Collections.Generic;
    using System.Linq;

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Interfaces;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.Services.Interfaces;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.General;

    public sealed class RoleProcessor : BaseProcessor<RoleViewModel>
    {
        private readonly IRoleProvider roleProvider;
        private readonly IBaseProvider<RolePermission> rolePermissionProvider;
        private readonly IPermissionProvider permissionProvider;
        private readonly IBaseService<Role> roleService;
        private readonly IBaseService<RolePermission> rolePermissionService;

        public RoleProcessor(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            this.roleProvider = serviceContainer.GetService<IRoleProvider>();
            this.rolePermissionProvider = serviceContainer.GetService<IBaseProvider<RolePermission>>();
            this.permissionProvider = serviceContainer.GetService<IPermissionProvider>();
            this.roleService = serviceContainer.GetService<IBaseService<Role>>();
            this.rolePermissionService = serviceContainer.GetService<IBaseService<RolePermission>>();
        }

        public override void Process(RoleViewModel viewModel)
        {
            if (viewModel.Permissions.IsNullOrDefault() || viewModel.Permissions.Count == default(int))
            {
                viewModel.Permissions = new List<PermissionViewModel>();
            }

            var permissions = permissionProvider.GetAllAsList();
            var role = roleProvider.GetEntity(r => r.Id == viewModel.Id);

            IEnumerable<PermissionViewModel> selectedViewModelPermissions;
            List<Permission> selectedPermissions;
            if (role.IsNullOrDefault())
            {
                role = new Role
                {
                    Name = viewModel.Name,
                    Active = viewModel.Active
                };

                selectedViewModelPermissions = viewModel.Permissions.Where(x => x.Active);
                selectedPermissions = permissions
                    .Where(x => selectedViewModelPermissions.Any(y => y.Id == x.Id))
                    .ToList();

                roleService.Add(role);

                if (!selectedPermissions.Any())
                {
                    return;
                }

                var rolePermissions = selectedPermissions.Select(x =>
                    new RolePermission
                    {
                        Role = role,
                        Role_Id = role.Id,
                        Permission_Id = x.Id,
                        Permission = x
                    }).ToList();

                rolePermissionService.AddRange(rolePermissions);

                return;
            }

            var currentPermissions = rolePermissionProvider.Where(x => x.Role_Id == role.Id);
            selectedViewModelPermissions = viewModel.Permissions.Where(x => x.Active);
            selectedPermissions = permissions
                .Where(x => selectedViewModelPermissions.Any(y => y.Id == x.Id))
                .ToList();
            var deletedRoles = currentPermissions
                .Where(x => !selectedPermissions.Any(y => y.Id == x.Permission_Id))
                .ToList();
            var newRoles = selectedPermissions
                .Where(x => !currentPermissions.Any(y => x.Id == y.Permission_Id))
                .Select(x => new RolePermission
                {
                    Role = role,
                    Role_Id = role.Id,
                    Permission = x,
                    Permission_Id = x.Id
                }).ToList();

            role.Name = viewModel.Name;
            role.Active = viewModel.Active;

            roleService.AddOrUpdate(role);

            if (deletedRoles.Any())
            {
                rolePermissionService.DeleteRange(deletedRoles);
            }

            if (newRoles.Any())
            {
                rolePermissionService.AddRange(newRoles);
            }
        }
    }
}
