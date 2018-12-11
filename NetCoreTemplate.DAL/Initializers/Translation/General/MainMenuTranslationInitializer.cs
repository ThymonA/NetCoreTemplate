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
                new Translation
                {
                    Key = "Users",
                    NL = "Gebruikers",
                    EN = "Users",
                    DE = "Benutzer",
                    FR = "Utilisateurs"
                },
                new Translation
                {
                    Key = "ChangeLanguage",
                    NL = "Verander van taal",
                    EN = "Change language",
                    DE = "Sprache ändern",
                    FR = "Changer de langue"
                },
                new Translation
                {
                    Key = "Dutch",
                    NL = "Nederlands",
                    EN = "Dutch",
                    DE = "Niederländisch",
                    FR = "Néerlandais"
                },
                new Translation
                {
                    Key = "English",
                    NL = "Engels",
                    EN = "English",
                    DE = "Englisch",
                    FR = "Anglais"
                },
                new Translation
                {
                    Key = "German",
                    NL = "Duits",
                    EN = "German",
                    DE = "Deutsch",
                    FR = "Allemand"
                },
                new Translation
                {
                    Key = "French",
                    NL = "Frans",
                    EN = "French",
                    DE = "Französisch",
                    FR = "Français"
                }
            };
        }
    }
}
