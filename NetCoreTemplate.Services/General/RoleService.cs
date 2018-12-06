namespace NetCoreTemplate.Services.General
{
    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Services.Base;
    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;

    public sealed class RoleService : BaseService<Role>
    {
        public RoleService(IPersistenceLayer persistence)
            : base(persistence)
        {
        }
    }
}
