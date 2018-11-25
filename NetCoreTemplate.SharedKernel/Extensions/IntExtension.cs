namespace NetCoreTemplate.SharedKernel.Extensions
{
    using System;

    public static class IntExtension
    {
        private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static double ConvertToMegabytes(this int bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        public static string SizeSuffix(this int value, int decimalPlaces = 1)
        {
            if (value < 0)
            {
                return "-" + SizeSuffix(-value);
            }

            if (value == 0)
            {
                return string.Format("{0:n" + decimalPlaces + "} bytes", 0);
            }

            var mag = (int)Math.Log(value, 1024);
            var adjustedSize = (decimal)value / (1L << (mag * 10));

            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", adjustedSize, SizeSuffixes[mag]);
        }

        public static int ToInt(this object value)
        {
            return ToInt(value, default(int));
        }

        public static int ToInt(this object value, int defaultValue)
        {
            var result = defaultValue;

            if (value.IsNullOrDefault())
            {
                return result;
            }

            var parse = value.ToString();
            while (parse.StartsWith("0") && parse.Length > 1)
            {
                parse = parse.Remove(0, 1);
            }

            return !int.TryParse(parse, out result)
                ? defaultValue
                : result;
        }
    }
}
