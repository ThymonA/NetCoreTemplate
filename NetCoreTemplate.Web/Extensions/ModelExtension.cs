namespace NetCoreTemplate.Web.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NetCoreTemplate.ViewModels.Interfaces;

    public static class ModelExtension
    {
        public static bool IsActive(this IBaseViewModel viewModel, string path)
        {
            var pathList = new List<string>();

            if (!path.StartsWith("/"))
            {
                pathList.Add($"/{path}");
            }

            if (path.StartsWith("/"))
            {
                pathList.Add(path.Substring(1));
            }

            if (!path.EndsWith("/"))
            {
                pathList.Add($"{path}/");
            }

            if (path.EndsWith("/"))
            {
                pathList.Add(path.Substring(0, path.Length - 1));
            }

            pathList.Add(path);

            return pathList.Any(x => x.Equals(viewModel.Path));
        }

        public static string LanguageImage(this IBaseViewModel viewModel)
        {
            switch (viewModel.Language)
            {
                case "NL":
                    return "http://i65.tinypic.com/2d0kyno.png";
                case "EN":
                    return "http://i64.tinypic.com/fd60km.png";
                case "DE":
                    return "http://i63.tinypic.com/10zmzyb.png";
                case "FR":
                    return "http://i65.tinypic.com/300b30k.png";
                default:
                    return "http://i64.tinypic.com/fd60km.png";
            }
        }

        public static string ToString(this IBaseViewModel viewModel, DateTimeOffset dateTimeOffset, bool inculdeTime = true)
        {
            var cultureInfo = viewModel.CultureInfo;
            var result = dateTimeOffset.ToString(cultureInfo.DateTimeFormat.ShortDatePattern, cultureInfo);

            if (inculdeTime)
            {
                result += " " + dateTimeOffset.ToString(cultureInfo.DateTimeFormat.ShortTimePattern, cultureInfo);
            }

            return result;
        }
    }
}
