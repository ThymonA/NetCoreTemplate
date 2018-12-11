namespace NetCoreTemplate.DAL.Initializers.Translation.General
{
    using System.Collections.Generic;

    public sealed class UserTranslationInitializer : BaseTranslationInitializer
    {
        public override string Module => "Dashboard";

        public override string Type => "User";

        public override List<Translation> Translations()
        {
            return new List<Translation>
            {
                new Translation
                {
                    Key = "Firstname",
                    NL = "Voornaam",
                    EN = "First name",
                    DE = "Vorname",
                    FR = "Prénom"
                },
                new Translation
                {
                    Key = "Lastname",
                    NL = "Achternaam",
                    EN = "Last name",
                    DE = "Nachname",
                    FR = "Nom de famille"
                },
                new Translation
                {
                    Key = "Email",
                    NL = "E-mail",
                    EN = "Email",
                    DE = "Email",
                    FR = "Email"
                },
                new Translation
                {
                    Key = "UserActive",
                    NL = "Gebruiker actief",
                    EN = "User active",
                    DE = "Benutzer aktiv",
                    FR = "Utilisateur actif"
                },
                new Translation
                {
                    Key = "NewUser",
                    NL = "Nieuwe gebruiker",
                    EN = "New user",
                    DE = "Neuer Benutzer",
                    FR = "Nouvel utilisateur"
                },
                new Translation
                {
                    Key = "User",
                    NL = "Gebruiker",
                    EN = "User",
                    DE = "Benutzer",
                    FR = "Utilisateur"
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
                    Key = "Role",
                    NL = "Rol",
                    EN = "Role",
                    DE = "Rolle",
                    FR = "Rôle"
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
                    Key = "Actions",
                    NL = "Acties",
                    EN = "Actions",
                    DE = "Aktionen",
                    FR = "Actes"
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
                new Translation
                {
                    Key = "FirstnameEmpty",
                    NL = "Voornaam is verplicht.",
                    EN = "First name is required.",
                    DE = "Vorname ist erforderlich.",
                    FR = "Le prénom est requis."
                },
                new Translation
                {
                    Key = "LastnameEmpty",
                    NL = "Achternaam is verplicht.",
                    EN = "Last name is required.",
                    DE = "Nachname ist erforderlich.",
                    FR = "Le nom de famille est requis."
                },
                new Translation
                {
                    Key = "EmailEmpty",
                    NL = "E-mail is verplicht.",
                    EN = "Email is required.",
                    DE = "E-Mail ist erforderlich.",
                    FR = "Email est requis."
                },
                new Translation
                {
                    Key = "EmailAlreadyExists",
                    NL = "E-mail wordt al door een andere gebruiker gebruikt.",
                    EN = "Email is already used by another user.",
                    DE = "E-Mail wird bereits von einem anderen Benutzer verwendet.",
                    FR = "Le courrier électronique est déjà utilisé par un autre utilisateur."
                },
                new Translation
                {
                    Key = "EmailNotValid",
                    NL = "E-mail is niet geldig.",
                    EN = "Email is not valid.",
                    DE = "Email ist ungültig.",
                    FR = "L'email n'est pas valide."
                },
                new Translation
                {
                    Key = "UserNotFound",
                    NL = "Gebruiker bestaat niet.",
                    EN = "User does not exists.",
                    DE = "Benutzer existiert nicht",
                    FR = "L'utilisateur n'existe pas."
                },
            };
        }
    }
}
