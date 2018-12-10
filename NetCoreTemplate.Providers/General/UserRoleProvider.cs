namespace NetCoreTemplate.Providers.General
{
    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Base;
    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;

    public sealed class UserRoleProvider : BaseProvider<UserRole>
    {
        public UserRoleProvider(IPersistenceLayer persistence)
            : base(persistence)
        {
        }
    }
}
