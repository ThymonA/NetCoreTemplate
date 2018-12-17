namespace NetCoreTemplate.DAL.Initializers.Translation.General
{
    using System.Collections.Generic;

    public sealed class AdminTranslationInitializer : BaseTranslationInitializer
    {
        public override string Module => "Dashboard";

        public override string Type => "Admin";

        public override List<Translation> Translations()
        {
            return new List<Translation>
            {
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
                    Key = "Reset",
                    NL = "Resetten",
                    EN = "Reset",
                    DE = "Zurücksetzen",
                    FR = "Réinitialiser"
                },
                new Translation
                {
                    Key = "EmailEmpty",
                    NL = "E-mail kan gevuld zijn",
                    EN = "Email can not be empty",
                    DE = "E-Mail darf nicht leer sein",
                    FR = "Email ne peut pas être vide"
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
                    Key = "PasswordReset",
                    NL = "Wachtwoord opnieuw instellen",
                    EN = "Reset password",
                    DE = "Passwort zurücksetzen",
                    FR = "Réinitialiser le mot de passe"
                },
                new Translation
                {
                    Key = "Message",
                    NL = "Stel uw wachtwoord opnieuw in door uw e-mailadres in te voeren.",
                    EN = "Reset your password by enter your email address.",
                    DE = "Setzen Sie Ihr Passwort zurück, indem Sie Ihre E-Mail-Adresse eingeben.",
                    FR = "Réinitialiser votre mot de passe en entrant votre adresse email."
                }
            };
        }
    }
}
