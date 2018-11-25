namespace NetCoreTemplate.Providers.General
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Base;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;

    public sealed class PermissionProvider : BaseProvider<Permission>, IPermissionProvider
    {
        public PermissionProvider(IPersistenceLayer persistence)
            : base(persistence)
        {
        }

        public bool HasPermission(User user, string action)
        {
            return !user.Id.IsNullOrDefault() && HasPermission(user.Id, action);
        }

        public bool HasPermission(int userId, string action)
        {
            return GetAllPermissions(userId)
                .Any(y => y.Action.Equals(action, StringComparison.OrdinalIgnoreCase));
        }

        public List<Permission> GetPermissions(User user)
        {
            return user.Id.IsNullOrDefault() ? new List<Permission>() : GetPermissions(user.Id);
        }

        public List<Permission> GetPermissions(int userId)
        {
            return GetAllPermissions(userId).ToList();
        }

        private IQueryable<Permission> GetAllPermissions(int userId)
        {
            var userRoles = Persistence.Get<UserRole>()
                .Where(x => x.User_Id == userId && x.Role.Active);

            var permissions = userRoles.SelectMany(x => x.Role.RolePermissions.Select(y => y.Permission));
            var groupedPermissions = permissions.GroupBy(x => x.Action);

            return groupedPermissions.Select(x => x.First());
        }
    }
}
