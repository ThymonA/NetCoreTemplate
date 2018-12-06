namespace NetCoreTemplate.Services.General
{
    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Services.Base;
    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;

    public sealed class RolePermissionService : BaseService<RolePermission>
    {
        public RolePermissionService(IPersistenceLayer persistence)
            : base(persistence)
        {
        }
    }
}
