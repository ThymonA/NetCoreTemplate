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
                    NL = "Wachtwoord herstellen",
                    EN = "Recover password",
                    DE = "Passwort wiederherstellen",
                    FR = "Récupérer mot de passe"
                },
                new Translation
                {
                    Key = "Message",
                    NL = "Vul uw e-mailadres in om uw wachtwoord te resetten.",
                    EN = "Enter your email address to reset your password.",
                    DE = "Geben Sie Ihre E-Mail-Addresse ein, um Ihr Passwort zurückzusetzen",
                    FR = "Saisissez votre adresse email pour réinitialisez votre mot de passe"
                }
            };
        }
    }
}
