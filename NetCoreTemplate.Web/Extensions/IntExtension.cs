namespace NetCoreTemplate.Web.Extensions
{
    using System;

    using NetCoreTemplate.SharedKernel.Dictionary;

    public static class IntExtension
    {
        public static string MinutesToText(this int minutes, TranslatedDictionary labels)
        {
            var timespan = TimeSpan.FromMinutes(minutes);

            var result = string.Empty;

            if (timespan.Days > 0)
            {
                result += timespan.Days + " ";
                result += timespan.Days > 1 ? labels["General:Dashboard:Days"] : labels["General:Dashboard:Day"];
                result += " " + timespan.Hours + " ";
                result += timespan.Hours > 1 ? labels["General:Dashboard:Hours"] : labels["General:Dashboard:Hour"];
                result += " " + timespan.Minutes + " ";
                result += timespan.Minutes > 1 ? labels["General:Dashboard:Minutes"] : labels["General:Dashboard:Minute"];
            }
            else if (timespan.Hours > 0)
            {
                result += timespan.Hours + " ";
                result += timespan.Hours > 1 ? labels["General:Dashboard:Hours"] : labels["General:Dashboard:Hour"];
                result += " " + timespan.Minutes + " ";
                result += timespan.Minutes > 1 ? labels["General:Dashboard:Minutes"] : labels["General:Dashboard:Minute"];
            }
            else if (timespan.Minutes > 0)
            {
                result += timespan.Minutes + " ";
                result += timespan.Minutes > 1 ? labels["General:Dashboard:Minutes"] : labels["General:Dashboard:Minute"];
            }
            else
            {
                result += labels["General:Dashboard:Started"];
            }

            return result;
        }
    }
}
