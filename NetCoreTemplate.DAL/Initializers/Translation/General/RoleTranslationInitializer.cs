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
                    Key = "Role",
                    NL = "Rol",
                    EN = "Role",
                    DE = "Rolle",
                    FR = "Rôle"
                },
                new Translation
                {
                    Key = "NewRole",
                    NL = "Nieuwe rol",
                    EN = "New role",
                    DE = "Neue Rolle",
                    FR = "Nouveau rôle"
                },
                new Translation
                {
                    Key = "Permission",
                    NL = "Toestemming",
                    EN = "Permission",
                    DE = "Genehmigung",
                    FR = "Autorisation"
                },
                new Translation
                {
                    Key = "RoleActive",
                    NL = "Rol actief",
                    EN = "Role active",
                    DE = "Rolle aktiv",
                    FR = "Rôle actif"
                },
                new Translation
                {
                    Key = "Add",
                    NL = "Toevoegen",
                    EN = "Add",
                    DE = "Hinzufügen",
                    FR = "Ajouter"
                },
                new Translation
                {
                    Key = "Edit",
                    NL = "Bewerken",
                    EN = "Edit",
                    DE = "Bearbeiten",
                    FR = "Modifier"
                },
                new Translation
                {
                    Key = "Delete",
                    NL = "Verwijderen",
                    EN = "Delete",
                    DE = "Löschen",
                    FR = "Effacer"
                },
                new Translation
                {
                    Key = "Cancel",
                    NL = "Annuleren",
                    EN = "Cancel",
                    DE = "Stornieren",
                    FR = "Annuler"
                },
                new Translation
                {
                    Key = "Save",
                    NL = "Opslaan",
                    EN = "Save",
                    DE = "Sparen",
                    FR = "Sauvegarder"
                },
            };
        }
    }
}
