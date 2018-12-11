namespace NetCoreTemplate.Providers.General
{
    using NetCoreTemplate.DAL.Models.Translation;
    using NetCoreTemplate.Providers.Base;
    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;

    public sealed class EntityLabelProvider : BaseProvider<EntityLabel>
    {
        public EntityLabelProvider(IPersistenceLayer persistence)
            : base(persistence)
        {
        }
    }
}
