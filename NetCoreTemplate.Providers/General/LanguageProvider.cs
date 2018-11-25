namespace NetCoreTemplate.Providers.General
{
    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Base;
    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;

    public sealed class LanguageProvider : BaseProvider<Language>
    {
        public LanguageProvider(IPersistenceLayer persistence)
            : base(persistence)
        {
        }
    }
}
