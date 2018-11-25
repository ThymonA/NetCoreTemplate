namespace NetCoreTemplate.SharedKernel.Expressions
{
    public static class StringExtension
    {
        public static bool ContainsOnlyDigits(this string str)
        {
            foreach (var c in str)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
