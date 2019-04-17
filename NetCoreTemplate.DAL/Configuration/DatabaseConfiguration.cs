namespace NetCoreTemplate.DAL.Configuration
{
    using NetCoreTemplate.SharedKernel.Extensions;

    using Microsoft.Extensions.Configuration;

    public class DatabaseConfiguration : IDatabaseConfiguration
    {
        public const string MYSQL = "MYSQL";
        public const string MSSQL = "MSSQL";

        private readonly IConfiguration configuration;

        public DatabaseConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;

            ConnectionString = configuration[$"Database:{nameof(ConnectionString)}"];
            SeedTestData = configuration[$"Database:{nameof(SeedTestData)}"].ToBoolean();
            SeedMetaData = configuration[$"Database:{nameof(SeedMetaData)}"].ToBoolean();
        }

        public IDatabaseConfiguration GetConfiguration()
        {
            return new DatabaseConfiguration(configuration);
        }

        public string ConnectionString { get; }

        public bool SeedTestData { get; }

        public bool SeedMetaData { get; }
    }
}