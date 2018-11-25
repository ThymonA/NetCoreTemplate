namespace NetCoreTemplate.Providers.FileManager
{
    using NetCoreTemplate.DAL.Models.FileManager;
    using NetCoreTemplate.Providers.Base;
    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;

    public sealed class FileManagerFileProvider : BaseProvider<FileManagerFile>
    {
        public FileManagerFileProvider(IPersistenceLayer persistence)
            : base(persistence)
        {
        }
    }
}
