namespace NetCoreTemplate.DAL.Initializers.Translation.General
{
    using System.Collections.Generic;

    public sealed class SignInTranslationInitializer : BaseTranslationInitializer
    {
        public override string Module => "Dashboard";

        public override string Type => "SignIn";

        public override List<Translation> Translations()
        {
            return new List<Translation>
            {
                new Translation
                {
                    Key = "Username",
                    NL = "Gebruikersnaam",
                    EN = "Username",
                    DE = "Nutzername",
                    FR = "Nom d'utilisateur"
                },
                new Translation
                {
                    Key = "Password",
                    NL = "Wachtwoord",
                    EN = "Password",
                    DE = "Passwort",
                    FR = "Mot de passe"
                },
                new Translation
                {
                    Key = "SignIn",
                    NL = "Log in",
                    EN = "Login",
                    DE = "Anmeldung",
                    FR = "S'identifier"
                },
                new Translation
                {
                    Key = "Deactivated",
                    NL = "Uw account is uitgeschakeld.",
                    EN = "Your account has been disabled.",
                    DE = "Ihr Account wurde deaktiviert.",
                    FR = "Votre compte a été désactivé"
                },
                new Translation
                {
                    Key = "WrongLogin",
                    NL = "Uw gebruikersnaam of wachtwoord is verkeerd.",
                    EN = "Your username or password is wrong.",
                    DE = "Ihr Benutzername oder Passwort ist falsch.",
                    FR = "Votre nom d'utilisateur ou mot de passe est incorrect."
                },
                new Translation
                {
                    Key = "Message",
                    NL = "Log in met uw account.",
                    EN = "Sign in with your account.",
                    DE = "Melden Sie sich mit Ihrem Konto an.",
                    FR = "Connectez-vous avec votre compte."
                },
            };
        }
    }
}
