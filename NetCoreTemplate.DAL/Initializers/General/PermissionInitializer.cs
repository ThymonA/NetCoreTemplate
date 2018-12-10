namespace NetCoreTemplate.DAL.Initializers.General
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Microsoft.EntityFrameworkCore;

    using NetCoreTemplate.DAL.Initializers.Translation;
    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.DAL.Permissions;

    using Action = NetCoreTemplate.DAL.Permissions.Action;
    using Type = NetCoreTemplate.DAL.Permissions.Type;

    public sealed class PermissionInitializer : EntityWithTranslationInitializer<Permission>
    {
        public PermissionInitializer(DatabaseContext context)
            : base(context)
        {
        }

        public override Expression<Func<DatabaseContext, DbSet<Permission>>> Expression => x => x.Permission;

        public override Expression<Func<Permission, int>> KeyExpression => x => x.Id;

        public override List<Permission> SeedMetaData()
        {
            return new List<Permission>
            {
                new Permission
                {
                    Id = 1,
                    Action = Permissions.GetActionKey(Module.Dashboard, Type.Roles, Action.View),
                    Translation = new Translation
                    {
                        NL = "Gebruiker kan overzicht met rollen bekijken.",
                        EN = "User can view overview with roles.",
                        DE = "Der Benutzer kann die Übersicht mit Rollen anzeigen.",
                        FR = "L'utilisateur peut voir la vue d'ensemble avec les rôles."
                    }
                },
                new Permission
                {
                    Id = 2,
                    Action = Permissions.GetActionKey(Module.Dashboard, Type.Role, Action.Edit),
                    Translation = new Translation
                    {
                        NL = "Gebruiker kan rollen toevoegen, bekijken en bewerken.",
                        EN = "User can add, view and edit roles.",
                        DE = "Der Benutzer kann Rollen hinzufügen, anzeigen und bearbeiten.",
                        FR = "L'utilisateur peut ajouter, voir et éditer des rôles."
                    }
                },
                new Permission
                {
                    Id = 3,
                    Action = Permissions.GetActionKey(Module.Dashboard, Type.Role, Action.Delete),
                    Translation = new Translation
                    {
                        NL = "Gebruiker kan rollen verwijderen.",
                        EN = "User can delete roles.",
                        DE = "Der Benutzer kann Rollen löschen.",
                        FR = "L'utilisateur peut supprimer des rôles."
                    }
                },
                new Permission
                {
                    Id = 4,
                    Action = Permissions.GetActionKey(Module.Dashboard, Type.Users, Action.View),
                    Translation = new Translation
                    {
                        NL = "Gebruiker kan overzicht met gebruikers bekijken.",
                        EN = "User can view overview with users.",
                        DE = "Benutzer können die Übersicht mit Benutzern anzeigen.",
                        FR = "L'utilisateur peut voir la vue d'ensemble avec les utilisateurs."
                    }
                },
                new Permission
                {
                    Id = 5,
                    Action = Permissions.GetActionKey(Module.Dashboard, Type.User, Action.Edit),
                    Translation = new Translation
                    {
                        NL = "Gebruiker kan gebruikers toevoegen, bekijken en bewerken.",
                        EN = "User can add, view and edit users.",
                        DE = "Benutzer können Benutzer hinzufügen, anzeigen und bearbeiten.",
                        FR = "L'utilisateur peut ajouter, voir et éditer des utilisateurs."
                    }
                },
                new Permission
                {
                    Id = 6,
                    Action = Permissions.GetActionKey(Module.Dashboard, Type.User, Action.Delete),
                    Translation = new Translation
                    {
                        NL = "Gebruiker kan gebruikers verwijderen.",
                        EN = "User can delete users.",
                        DE = "Benutzer kann Benutzer löschen.",
                        FR = "L'utilisateur peut supprimer des utilisateurs."
                    }
                },
                new Permission
                {
                    Id = 7,
                    Action = Permissions.GetActionKey(Module.Dashboard, Type.User, Action.Own),
                    Translation = new Translation
                    {
                        NL = "Gebruiker kan persoonlijke gegevens bewerken.",
                        EN = "User can edit personal data.",
                        DE = "Der Benutzer kann persönliche Daten bearbeiten.",
                        FR = "L'utilisateur peut éditer des données personnelles."
                    }
                }
            };
        }
    }
}
