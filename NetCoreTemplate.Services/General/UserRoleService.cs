namespace NetCoreTemplate.Services.General
{
    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Services.Base;
    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;

    public sealed class UserRoleService : BaseService<UserRole>
    {
        public UserRoleService(IPersistenceLayer persistence)
            : base(persistence)
        {
        }
    }
}
