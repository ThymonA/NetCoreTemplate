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
                    Key = "SuccessTitle",
                    NL = "Geslaagd",
                    EN = "Successful",
                    DE = "Erfolgreich",
                    FR = "Réussi"
                },
                new Translation
                {
                    Key = "Back",
                    NL = "Terug",
                    EN = "Back",
                    DE = "Zurück",
                    FR = "Retour"
                },
                new Translation
                {
                    Key = "SuccessMessage",
                    NL = "We hebben uw verzoek ontvangen, u ontvangt binnen enkele minuten een e-mail.",
                    EN = "We have received your request, you will receive an email within a few minutes.",
                    DE = "Wir haben Ihre Anfrage erhalten, Sie erhalten innerhalb weniger Minuten eine E-Mail.",
                    FR = "Nous avons reçu votre demande, vous recevrez un email dans quelques minutes."
                },
                new Translation
                {
                    Key = "MailSubject",
                    NL = "Uw verzoek om een wachtwoord opnieuw in te stellen",
                    EN = "Your request for a password reset",
                    DE = "Ihre Anfrage nach einem Passwort zurücksetzen",
                    FR = "Votre demande de réinitialisation du mot de passe"
                },
                new Translation
                {
                    Key = "MailText1",
                    NL = "Wij hebben een verzoek ontvangen",
                    EN = "We have received a request",
                    DE = "Wir haben eine Anfrage erhalten",
                    FR = "Nous avons reçu une demande"
                },
                new Translation
                {
                    Key = "MailText2",
                    NL = "om uw wachtwoord te resetten.",
                    EN = "to reset your password.",
                    DE = "das Passwort zurücksetzen.",
                    FR = "pour réinitialiser votre mot de passe."
                },
                new Translation
                {
                    Key = "MailText3",
                    NL = "Om uw wachtwoord te resetten",
                    EN = "To reset your password",
                    DE = "Das Passwort zurücksetzen",
                    FR = "Pour réinitialiser votre mot de passe"
                },
                new Translation
                {
                    Key = "MailText4",
                    NL = "moet u op de link / knop hieronder drukken",
                    EN = "you must press the link / button below.",
                    DE = "müssen sie den Link / Button unten drücken",
                    FR = "vous devez appuyer sur le lien / bouton ci-dessous"
                },
                new Translation
                {
                    Key = "MailButton",
                    NL = "Herstel mijn wachtwoord",
                    EN = "Restore my password",
                    DE = "Stellen Sie mein Passwort wieder her",
                    FR = "Restaurer mon mot de passe"
                },
                new Translation
                {
                    Key = "PasswordMismatch",
                    NL = "Uw wachtwoord komt niet overeen",
                    EN = "Your password does not match",
                    DE = "Ihr Passwort stimmt nicht überein",
                    FR = "Votre mot de passe ne correspond pas"
                },
                new Translation
                {
                    Key = "EmptyToken",
                    NL = "Uw tokenveld is leeg",
                    EN = "Your token field is empty",
                    DE = "Ihr Tokenfeld ist leer",
                    FR = "Votre champ de jeton est vide"
                },
                new Translation
                {
                    Key = "EmptyPassword",
                    NL = "Uw wachtwoordveld is leeg",
                    EN = "Your password field is empty",
                    DE = "Ihr Passwortfeld ist leer",
                    FR = "Votre mot de passe est vide"
                },
                new Translation
                {
                    Key = "WeakPassword",
                    NL = "Uw wachtwoord is niet sterk genoeg",
                    EN = "Your password is not strong enough",
                    DE = "Ihr Passwort ist nicht stark genug",
                    FR = "Votre mot de passe n'est pas assez fort"
                },
                new Translation
                {
                    Key = "PasswordCapitalLetters",
                    NL = "Hoofdletters",
                    EN = "Capital letters",
                    DE = "Großbuchstaben",
                    FR = "Lettres majuscules"
                },
                new Translation
                {
                    Key = "PasswordLowercase",
                    NL = "Kleine letters",
                    EN = "Lowercase",
                    DE = "Kleinbuchstaben",
                    FR = "Minuscule"
                },
                new Translation
                {
                    Key = "PasswordNumbers",
                    NL = "Getallen",
                    EN = "Numbers",
                    DE = "Zahlen",
                    FR = "Nombres"
                },
                new Translation
                {
                    Key = "PasswordCharacters",
                    NL = "Symbolen",
                    EN = "Characters",
                    DE = "Figuren",
                    FR = "Personnages"
                },
                new Translation
                {
                    Key = "PasswordAtLeastLong",
                    NL = "Minimaal 8 tekens lang",
                    EN = "At least 8 characters long",
                    DE = "Mindestens 8 Zeichen lang",
                    FR = "Au moins 8 caractères"
                },
                new Translation
                {
                    Key = "TokenTitle",
                    NL = "Wachtwoord resetten",
                    EN = "Reset password",
                    DE = "Passwort zurücksetzen",
                    FR = "Réinitialiser le mot de passe"
                },
                new Translation
                {
                    Key = "TokenMessage",
                    NL = "U kunt nu een nieuw wachtwoord instellen voor:",
                    EN = "You can now set a new password for:",
                    DE = "Sie können jetzt ein neues Passwort festlegen für:",
                    FR = "Vous pouvez maintenant définir un nouveau mot de passe pour:"
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
                    Key = "RepeatPassword",
                    NL = "Herhaal wachtwoord",
                    EN = "Repeat password",
                    DE = "Wiederhole das Passwort",
                    FR = "Répéter le mot de passe"
                },
                new Translation
                {
                    Key = "SetPassword",
                    NL = "Wachtwoord instellen",
                    EN = "Set Password",
                    DE = "Passwort festlegen",
                    FR = "Définir le mot de passe"
                },
                new Translation
                {
                    Key = "ExpireTitle",
                    NL = "Token is verlopen",
                    EN = "Token has expired",
                    DE = "Token ist abgelaufen",
                    FR = "Le jeton a expiré"
                },
                new Translation
                {
                    Key = "ExpireMessage",
                    NL = "Uw token voor het opnieuw instellen van uw wachtwoord is verlopen, vraag een nieuwe om uw wachtwoord opnieuw in te stellen.",
                    EN = "Your token for resetting your password has expired, request a new one to reset your password.",
                    DE = "Ihr Token zum Zurücksetzen Ihres Passworts ist abgelaufen. Fordern Sie ein neues an, um Ihr Passwort zurückzusetzen.",
                    FR = "Votre jeton de réinitialisation de votre mot de passe a expiré, demandez-en un nouveau pour réinitialiser votre mot de passe."
                },
                new Translation
                {
                    Key = "Best",
                    NL = "Beste",
                    EN = "Best",
                    DE = "Beste",
                    FR = "Meilleur"
                },
            };
        }
    }
}
