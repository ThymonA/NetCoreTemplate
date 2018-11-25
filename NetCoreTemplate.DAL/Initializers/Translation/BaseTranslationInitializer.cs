namespace NetCoreTemplate.DAL.Initializers.Translation
{
    using System.Collections.Generic;

    public abstract class BaseTranslationInitializer
    {
        public abstract string Module { get; }

        public abstract string Type { get; }

        public abstract List<Translation> Translations();
    }
}
