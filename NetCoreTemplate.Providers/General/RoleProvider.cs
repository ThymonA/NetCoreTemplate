namespace NetCoreTemplate.Providers.General
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Base;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;

    public sealed class RoleProvider : BaseProvider<Role>, IRoleProvider
    {
        public RoleProvider(IPersistenceLayer persistence)
            : base(persistence)
        {
        }

        public List<Role> GetRolesForUser(int userId)
        {
            return Persistence.Get<UserRole>()
                .Include(x => x.Role)
                .Where(x => x.User_Id == userId)
                .Select(x => x.Role)
                .ToList();
        }
    }
}
