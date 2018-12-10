namespace NetCoreTemplate.DAL.Initializers
{
    using System.Collections.Generic;

    using NetCoreTemplate.DAL.Configuration;
    using NetCoreTemplate.DAL.Initializers.FileManager;
    using NetCoreTemplate.DAL.Initializers.General;
    using NetCoreTemplate.DAL.Initializers.Translation;
    using NetCoreTemplate.DAL.Initializers.Translation.General;
    using NetCoreTemplate.SharedKernel.ServiceContainer;

    public static class MainInitializer
    {
        private static IDatabaseConfiguration DatabaseConfig => ServiceContainer.Current.GetService<IDatabaseConfiguration>();

        public static bool AllowSeedTestData => DatabaseConfig.SeedTestData;

        public static bool AllowSeedMetaData => DatabaseConfig.SeedMetaData;

        public static void Seed(DatabaseContext context)
        {
            if (AllowSeedMetaData)
            {
                ExecuteInitializers(context);
                ExecuteTranslationInitializers(context);
            }
        }

        private static void ExecuteInitializers(DatabaseContext context)
        {
            /** General */
            var languages = new LanguageInitializer(context).Seed();
            var users = new UserInitializer(context).Seed();
            var permissions = new PermissionInitializer(context).Seed();
            var roles = new RoleInitializer(permissions, users, context).Seed();

            /** File Manager */
            var fileManagerDirectories = new FileManagerDirectoryInitializer(users, context).Seed();
        }

        private static void ExecuteTranslationInitializers(DatabaseContext context)
        {
            var listTranslationInitializers = new List<BaseTranslationInitializer>
            {
                new SignInTranslationInitializer(),
                new MainMenuTranslationInitializer(),
                new GeneralTranslationInitializer(),
                new RoleTranslationInitializer(),
                new UserTranslationInitializer()
            };

            TranslationInitializer.SeedTranslations(context, listTranslationInitializers);
        }
    }
}
