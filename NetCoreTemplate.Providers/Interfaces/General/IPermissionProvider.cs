namespace NetCoreTemplate.Providers.Interfaces.General
{
    using System.Collections.Generic;

    using NetCoreTemplate.DAL.Models.General;

    public interface IPermissionProvider : IBaseProvider<Permission>
    {
        bool HasPermission(User user, string action);

        bool HasPermission(int userId, string action);

        List<Permission> GetPermissions(User user);

        List<Permission> GetPermissions(int userId);
    }
}
