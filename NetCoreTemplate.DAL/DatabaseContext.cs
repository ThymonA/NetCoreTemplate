namespace NetCoreTemplate.DAL
{
    using System;

    using NetCoreTemplate.DAL.Extensions;
    using NetCoreTemplate.DAL.Models.FileManager;
    using NetCoreTemplate.DAL.Models.Translation;
    using NetCoreTemplate.SharedKernel.ServiceContainer;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    using NetCoreTemplate.DAL.Configuration;
    using NetCoreTemplate.DAL.Models.General;

    public class DatabaseContext : DbContext
    {
        private static IServiceContainer Container => ServiceContainer.Current;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = Container.GetService<IConfiguration>();
            var databaseProvider = DbContextExtension.DatabaseProvider;

            if (string.IsNullOrWhiteSpace(databaseProvider))
            {
                databaseProvider = configuration["Database:DatabaseProvider"];
            }

            if (string.IsNullOrWhiteSpace(databaseProvider))
            {
                throw new ArgumentNullException($"Configuration 'Database:DatabaseProvider' can't be null");
            }

            switch (databaseProvider.ToUpper().Trim())
            {
                case DatabaseConfiguration.MSSQL:
                    optionsBuilder.UseSqlServer(configuration["Database:MSSQL:ConnectionString"]);
                    break;
                case DatabaseConfiguration.MYSQL:
                    optionsBuilder.UseMySql(configuration["Database:MYSQL:ConnectionString"]);
                    break;
                default:
                    throw new NotImplementedException($"Database provider '{databaseProvider}' has not been implemented.");
            }

            DbContextExtension.DatabaseProvider = databaseProvider;

            optionsBuilder.EnableSensitiveDataLogging();

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder moderBuilder)
        {
            base.OnModelCreating(moderBuilder);

            moderBuilder.Entity<Language>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Code).IsRequired();
                entity.Property(e => e.CultureCode).IsRequired();
                entity.Property(e => e.Name).IsRequired();
            });

            moderBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Action).IsRequired();
            });

            moderBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
            });

            moderBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(e => new { e.Permission_Id, e.Role_Id });
                entity.HasOne(e => e.Permission)
                    .WithMany(e => e.RolePermissions);
                entity.HasOne(e => e.Role)
                    .WithMany(e => e.RolePermissions);
            });

            moderBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired();
            });

            moderBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.Role_Id, e.User_Id });
                entity.HasOne(e => e.Role)
                    .WithMany(e => e.UserRoles);
                entity.HasOne(e => e.User)
                    .WithMany(e => e.UserRoles);
            });

            moderBuilder.Entity<MailQueue>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.To).IsRequired();
            });

            moderBuilder.Entity<EntityLabel>(entity =>
            {
                entity.HasKey(e => new { e.EntityLabelDefinition_Id, e.Language_Id });
                entity.Property(e => e.Label).IsRequired();
                entity.HasOne(e => e.EntityLabelDefinition)
                    .WithMany(e => e.EntityLabels);
                entity.HasOne(e => e.Language)
                    .WithMany(e => e.EntityLabels);
            });

            moderBuilder.Entity<EntityLabelDefinition>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Key).IsRequired();
            });

            moderBuilder.Entity<TranslationLabel>(entity =>
            {
                entity.HasKey(e => new { e.Language_Id, e.TranslationLabelDefinition_Id });
                entity.Property(e => e.Label).IsRequired();
                entity.HasOne(e => e.Language)
                    .WithMany(e => e.TranslationLabels);
                entity.HasOne(e => e.TranslationLabelDefinition)
                    .WithMany(e => e.TranslationLabels);
            });

            moderBuilder.Entity<TranslationLabelDefinition>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Key).IsRequired();
                entity.Property(e => e.Module).IsRequired();
                entity.Property(e => e.Type).IsRequired();
            });

            moderBuilder.Entity<FileManagerDirectory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.CreatedUser)
                    .WithMany(e => e.FileManagerDirectories);
                entity.HasOne(e => e.Parent)
                    .WithMany(e => e.FileManagerDirectories);
            });
        }

        /** General */

        public DbSet<Language> Language { get; set; }

        public DbSet<Permission> Permission { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<RolePermission> RolePermission { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<MailQueue> MailQueue { get; set; }

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
