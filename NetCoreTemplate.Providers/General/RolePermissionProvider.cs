namespace NetCoreTemplate.Providers.General
{
    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Base;
    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;

    public sealed class RolePermissionProvider : BaseProvider<RolePermission>
    {
        public RolePermissionProvider(IPersistenceLayer persistence)
            : base(persistence)
        {
        }
    }
}
