namespace NetCoreTemplate.ViewModelProcessors.Controllers.Role
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.DAL.Models.Translation;
    using NetCoreTemplate.Providers.Interfaces;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.General;
    using NetCoreTemplate.ViewModels.Models;

    public sealed class RoleLoader : BaseAuthenticationLoader<RoleViewModel, int>
    {
        private readonly IRoleProvider roleProvider;
        private readonly IPermissionProvider permissionProvider;
        private readonly IBaseProvider<EntityLabel> entityLabelProvider;

        public RoleLoader(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            this.roleProvider = serviceContainer.GetService<IRoleProvider>();
            this.permissionProvider = serviceContainer.GetService<IPermissionProvider>();
            this.entityLabelProvider = serviceContainer.GetService<IBaseProvider<EntityLabel>>();
        }

        protected override TranslationSettings TranslationSettings => new TranslationSettings("Dashboard", "Role");

        protected override RoleViewModel CreateViewModel(int roleId)
        {
            RoleViewModel viewModel;

            var role = roleProvider.GetAll()   
                .Include(x => x.RolePermissions)
                .ThenInclude(x => x.Permission)
                .FirstOrDefault(x => x.Id == roleId);

            if (role.IsNullOrDefault())
            {
                viewModel = new RoleViewModel
                {
                    Id = 0,
                    Name = string.Empty,
                    Active = true,
                    Permissions = new List<PermissionViewModel>()
                };

                LoadAllPermissions(viewModel, new List<Permission>());

                return viewModel;
            }

            viewModel = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                Active = role.Active,
                Permissions = new List<PermissionViewModel>()
            };

            var permissions = role.RolePermissions
                .Select(x => x.Permission)
                .ToList();

            LoadAllPermissions(viewModel, permissions);

            return viewModel;
        }

        protected override RoleViewModel ReloadViewModel(RoleViewModel viewModel)
        {
            if (viewModel.Permissions.IsNullOrDefault() || viewModel.Permissions.Count == default(int))
            {
                viewModel.Permissions = new List<PermissionViewModel>();
            }

            var existingPermissions = permissionProvider.GetAll().ToList();
            var activePermissions = viewModel.Permissions.Where(x => x.Active);

            var permissionViewModels = existingPermissions.Select(permission =>
                new PermissionViewModel
                {
                    Id = permission.Id,
                    Action = GetEntityTranslation(permission.EntityLabelDefinition_Id),
                    Active = activePermissions.Any(y => y.Id == permission.Id)
                });

            viewModel.Permissions = new List<PermissionViewModel>();
            viewModel.Permissions.AddRange(permissionViewModels);

            return viewModel;
        }

        private void LoadAllPermissions(RoleViewModel viewModel, List<Permission> permissions)
        {
            if (permissions.IsNullOrDefault() || permissions.Count == default(int))
            {
                permissions = new List<Permission>();
            }

            var existingPermissions = permissionProvider.GetAll().ToList();
            var permissionViewModels = existingPermissions.Select(permission =>
                new PermissionViewModel
                {
                    Id = permission.Id,
                    Action = GetEntityTranslation(permission.EntityLabelDefinition_Id),
                    Active = permissions.Any(y => y.Id == permission.Id)
                });

            viewModel.Permissions = new List<PermissionViewModel>();
            viewModel.Permissions.AddRange(permissionViewModels);
        }

        private string GetEntityTranslation(int entityLabelDefinitionId)
        {
            var languageId = LanguageId;

            var entityTranslationLabel = entityLabelProvider
                .GetEntity(x => x.EntityLabelDefinition_Id == entityLabelDefinitionId &&
                                x.Language_Id == languageId);

            return entityTranslationLabel.IsNullOrDefault()
                       ? string.Empty
                       : entityTranslationLabel.Label;
        }
    }
}
