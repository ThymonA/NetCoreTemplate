namespace NetCoreTemplate.DAL.Initializers.Translation.General
{
    using System.Collections.Generic;

    public sealed class GeneralTranslationInitializer : BaseTranslationInitializer
    {
        public override string Module => "General";

        public override string Type => "Dashboard";

        public override List<Translation> Translations()
        {
            return new List<Translation>
            {
                new Translation
                {
                    Key = "Previous",
                    NL = "Vorige",
                    EN = "Previous",
                    DE = "Bisherige",
                    FR = "Précédent"
                },
                 new Translation
                {
                    Key = "Next",
                    NL = "Volgende",
                    EN = "Next",
                    DE = "Nächster",
                    FR = "Suivant"
                }
            };
        }
    }
}
