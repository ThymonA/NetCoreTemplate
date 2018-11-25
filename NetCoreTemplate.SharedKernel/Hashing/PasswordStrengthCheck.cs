namespace NetCoreTemplate.SharedKernel.Hashing
{
    using System.Linq;

    using NetCoreTemplate.SharedKernel.Enums;

    public static class PasswordStrengthCheck
    {
        public static PasswordStrength GetPasswordStrength(string password)
        {
            var score = 1;

            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(password.Trim()))
            {
                return PasswordStrength.Blank;
            }

            if (!HasMinimumLength(password, 5))
            {
                return PasswordStrength.VeryWeak;
            }

            if (HasMinimumLength(password, 8))
            {
                score++;
            }

            if (HasUpperCaseLetter(password) && HasLowerCaseLetter(password))
            {
                score++;
            }

            if (HasDigit(password))
            {
                score++;
            }

            if (HasSpecialChar(password))
            {
                score++;
            }

            return (PasswordStrength)score;
        }

        #region Helper Methods

        public static bool HasMinimumLength(string password, int minLength)
        {
            return password.Length >= minLength;
        }

        public static bool HasDigit(string password)
        {
            return password.Any(char.IsDigit);
        }

        public static bool HasSpecialChar(string password)
        {
            return password.IndexOfAny("!@#$%^&*?_~-£().,".ToCharArray()) != -1;
        }

        public static bool HasUpperCaseLetter(string password)
        {
            return password.Any(char.IsUpper);
        }

        public static bool HasLowerCaseLetter(string password)
        {
            return password.Any(char.IsLower);
        }

        #endregion
    }
}
