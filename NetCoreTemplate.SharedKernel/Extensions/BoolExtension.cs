namespace NetCoreTemplate.SharedKernel.Extensions
{
    using System;

    public static class BoolExtension
    {
        public static bool ToBoolean(this string input)
        {
            try
            {
                return Convert.ToBoolean(input);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ToBoolean(this string input, bool defaultValue)
        {
            try
            {
                return Convert.ToBoolean(input);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}
