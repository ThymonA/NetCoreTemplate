namespace NetCoreTemplate.DAL
{
    using NetCoreTemplate.DAL.Configuration;
    using NetCoreTemplate.DAL.Extensions;
    using NetCoreTemplate.DAL.Models.FileManager;
    using NetCoreTemplate.DAL.Models.Translation;
    using NetCoreTemplate.SharedKernel.ServiceContainer;

    using Microsoft.EntityFrameworkCore;

    using NetCoreTemplate.DAL.Models.General;

    public class DatabaseContext : DbContext
    {
        private static IServiceContainer Container => ServiceContainer.Current;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var databaesConfiguration = Container.GetService<IDatabaseConfiguration>();
            var connection = databaesConfiguration.ConnectionString;

            optionsBuilder.UseSqlServer(connection);
            optionsBuilder.EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.BuildIndexesFromAnnotations();
            builder.Entity<RolePermission>()
                .HasKey(x => new { x.Role_Id, x.Permission_Id });
            builder.Entity<UserRole>()
                .HasKey(x => new { x.User_Id, x.Role_Id });
            builder.Entity<TranslationLabel>()
                .HasKey(x => new { x.TranslationLabelDefinition_Id, x.Language_Id });
            builder.Entity<EntityLabel>()
                .HasKey(x => new { x.EntityLabelDefinition_Id, x.Language_Id });
        }

        /** General */

        public DbSet<Language> Language { get; set; }

        public DbSet<Permission> Permission { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<RolePermission> RolePermission { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<UserRole> UserRole { get; set; }

        /** Translation */

        public DbSet<EntityLabel> EntityLabel { get; set; }

        public DbSet<EntityLabelDefinition> EntityLabelDefinition { get; set; }

        public DbSet<TranslationLabel> TranslationLabel { get; set; }

        public DbSet<TranslationLabelDefinition> TranslationLabelDefinition { get; set; }

        /** File Manager */

        public DbSet<FileManagerDirectory> FileManagerDirectory { get; set; }

        public DbSet<FileManagerFile> FileManagerFile { get; set; }
    }
}
