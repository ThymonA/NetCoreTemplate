namespace NetCoreTemplate.Providers.Interfaces.General
{
    using System.Collections.Generic;

    using NetCoreTemplate.DAL.Models.General;

    public interface IRoleProvider : IBaseProvider<Role>
    {
        List<Role> GetRolesForUser(int userId);
    }
}
