namespace NetCoreTemplate.SharedKernel.Extensions
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtension
    {
        public static string RemoveMiltipleSpaces(this string input)
        {
            return Regex.Replace(input.Trim(), @"\s+", " ");
        }

        public static string GenerateRandomString(int length = 15)
        {
            var random = new Random();
            var password = string.Empty;

            for (var i = 0; i < length; i++)
            {
                password += ((char)(random.Next(1, 26) + 64)).ToString();
            }

            return password;
        }

        public static string ToBase64(this string text)
        {
            return ToBase64(text, Encoding.UTF8);
        }

        public static string ToBase64(this string text, Encoding encoding)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            var textAsBytes = encoding.GetBytes(text);

            return Convert.ToBase64String(textAsBytes);
        }

        public static bool TryParseBase64(this string text, out string decodedText)
        {
            return TryParseBase64(text, Encoding.UTF8, out decodedText);
        }

        public static bool TryParseBase64(this string text, Encoding encoding, out string decodedText)
        {
            if (string.IsNullOrEmpty(text))
            {
                decodedText = text;
                return false;
            }

            try
            {
                var textAsBytes = Convert.FromBase64String(text);

                decodedText = encoding.GetString(textAsBytes);

                return true;
            }
            catch (Exception)
            {
                decodedText = null;

                return false;
            }
        }
    }
}
