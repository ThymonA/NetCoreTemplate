namespace NetCoreTemplate.Services.General
{
    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Services.Base;
    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;

    public sealed class UserService : BaseService<User>
    {
        public UserService(IPersistenceLayer persistence)
            : base(persistence)
        {
        }
    }
}
