namespace NetCoreTemplate.DAL.Initializers.Translation.General
{
    using System.Collections.Generic;

    public sealed class RoleTranslationInitializer : BaseTranslationInitializer
    {
        public override string Module => "Dashboard";

        public override string Type => "Role";

        public override List<Translation> Translations()
        {
            return new List<Translation>
            {
                new Translation
                {
                    Key = "Name",
                    NL = "Naam",
                    EN = "Name",
                    DE = "Name",
                    FR = "Prénom"
                },
                new Translation
                {
                    Key = "Active",
                    NL = "Actief",
                    EN = "Active",
                    DE = "Aktiv",
                    FR = "Actif"
                },
                new Translation
                {
                    Key = "NotActive",
                    NL = "Niet actief",
                    EN = "Not active",
                    DE = "Nicht aktiv",
                    FR = "Pas actif"
                },
                new Translation
                {
                    Key = "Permissions",
                    NL = "Aantal machtigingen",
                    EN = "Number of permissions",
                    DE = "Anzahl der Berechtigungen",
                    FR = "Nombre de permissions"
                },
                new Translation
                {
                    Key = "Actions",
                    NL = "Acties",
                    EN = "Actions",
                    DE = "Aktionen",
                    FR = "Actes"
                },
            };
        }
    }
}
