namespace NetCoreTemplate.Web.Extensions
{
    using System.Text;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public static class NotyNotificationExtension
    {
        public static IHtmlContent ShowNotification<TModel>(
            this IHtmlHelper<TModel> helper,
            ITempDataDictionary tempData)
        {
            var stringBuilder = new StringBuilder();

            if (tempData.ContainsKey("notification"))
            {
                var message = tempData.ContainsKey("noty_message") ? tempData["noty_message"] : null;
                var type = tempData.ContainsKey("noty_type") ? tempData["noty_type"] : "information";

                stringBuilder.AppendLine("<script>");
                stringBuilder.AppendLine("$(document).ready(function() {");
                stringBuilder.AppendLine("new Noty({");
                stringBuilder.AppendLine($"type: '{type}',");
                stringBuilder.AppendLine($"text: '{message}',");
                stringBuilder.AppendLine("layout: 'bottomCenter',");
                stringBuilder.AppendLine("theme: 'relax',");
                stringBuilder.AppendLine("timeout: 3000");
                stringBuilder.AppendLine("}).show();");
                stringBuilder.AppendLine("});");
                stringBuilder.AppendLine("</script>");

                tempData.Remove("notification");
            }

            return new HtmlString(stringBuilder.ToString());
        }
    }
}
