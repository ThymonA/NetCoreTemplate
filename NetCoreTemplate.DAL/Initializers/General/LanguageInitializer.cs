namespace NetCoreTemplate.DAL.Initializers.General
{
    using System.Collections.Generic;

    using NetCoreTemplate.DAL.Extensions;
    using NetCoreTemplate.DAL.Models.General;

    public sealed class LanguageInitializer : BaseInitializer<Language>
    {
        public LanguageInitializer(DatabaseContext context)
            : base(context)
        {
        }

        public override List<Language> SeedData()
        {
            var list = GetLanguages();

            Context.AddOrUpdateRange(
                x => x.Language,
                x => x.Id,
                list);

            return list;
        }

        private static List<Language> GetLanguages()
        {
            return new List<Language>
            {
                new Language { Id = 1, Name = "Dutch", Code = "NL", CultureCode = "nl-NL" },
                new Language { Id = 2, Name = "English", Code = "EN", CultureCode = "en-GB" },
                new Language { Id = 3, Name = "German", Code = "DE", CultureCode = "de-DE" },
                new Language { Id = 4, Name = "French", Code = "FR", CultureCode = "fr-FR" }
            };
        }
    }
}
