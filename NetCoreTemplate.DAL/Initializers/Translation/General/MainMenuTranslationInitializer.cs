namespace NetCoreTemplate.DAL.Initializers.Translation.General
{
    using System.Collections.Generic;

    public sealed class MainMenuTranslationInitializer : BaseTranslationInitializer
    {
        public override string Module => "Dashboard";

        public override string Type => "MainMenu";

        public override List<Translation> Translations()
        {
            return new List<Translation>
            {
                new Translation
                {
                    Key = "Dashboard",
                    NL = "Dashboard",
                    EN = "Dashboard",
                    DE = "Instrumententafel",
                    FR = "Tableau de bord"
                },
                new Translation
                {
                    Key = "Roles",
                    NL = "Rollen",
                    EN = "Roles",
                    DE = "Rollen",
                    FR = "Rôles"
                },
            };
        }
    }
}
