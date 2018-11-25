namespace NetCoreTemplate.DAL.Configuration
{
    public interface IDatabaseConfiguration
    {
        string ConnectionString { get; }

        bool SeedTestData { get; }

        bool SeedMetaData { get; }

        IDatabaseConfiguration GetConfiguration();
    }
}
