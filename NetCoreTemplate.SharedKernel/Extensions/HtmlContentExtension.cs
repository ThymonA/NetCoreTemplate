namespace NetCoreTemplate.SharedKernel.Extensions
{
    using System.IO;
    using System.Text.Encodings.Web;

    using Microsoft.AspNetCore.Html;

    public static class HtmlContentExtension
    {
        public static string ToHtmlString(this IHtmlContent htmlContent)
        {
            var writter = new StringWriter();
            htmlContent.WriteTo(writter, HtmlEncoder.Default);
            return writter.ToString();
        }
    }
}
