namespace NetCoreTemplate.ViewModelProcessors.Controllers.Role
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Microsoft.EntityFrameworkCore;

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.SharedKernel.Expressions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.General;
    using NetCoreTemplate.ViewModels.Models;

    public sealed class RoleListLoader : BaseAuthenticationListLoader<Role, RoleListViewModel, RoleViewModel>
    {
        public RoleListLoader(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }

        protected override Expression<Func<Role, object>> DefaultOrderBy => role => role.Name;

        protected override IQueryable<Role> BaseQuery => EntityProvider.GetAll().Include(role => role.RolePermissions);

        protected override TranslationSettings TranslationSettings => new TranslationSettings("Dashboard", "Role");

        protected override Dictionary<string, List<SortExpression<Role>>> CustomOrderByExpressions =>
            new Dictionary<string, List<SortExpression<Role>>>
            {
                {
                    "Name",
                    new List<SortExpression<Role>>
                    {
                        new SortExpression<Role>(role => role.Name)
                    }
                },
                {
                    "Active",
                    new List<SortExpression<Role>>
                    {
                        new SortExpression<Role>(role => role.Active)
                    }
                },
                {
                    "Permissions",
                    new List<SortExpression<Role>>
                    {
                        new SortExpression<Role>(role => role.RolePermissions.Count)
                    }
                }
            };

        protected override RoleListViewModel CreateViewModel()
        {
            return new RoleListViewModel();
        }

        protected override RoleViewModel FillViewModel(Role role)
        {
            var permissions = role.RolePermissions.Select(rolePermission =>
                new PermissionViewModel
                {
                    Id = rolePermission.Permission_Id
                });

            var viewModel = new RoleViewModel
            {
                Id = role.Id,
                Name = role.Name,
                Active = role.Active,
                Permissions = new List<PermissionViewModel>()
            };

            viewModel.Permissions.AddRange(permissions);

            return viewModel;
        }
    }
}
