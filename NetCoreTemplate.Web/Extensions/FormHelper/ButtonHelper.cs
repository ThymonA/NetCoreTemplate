namespace NetCoreTemplate.Web.Extensions.FormHelper
{
    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class ButtonHelper
    {
        public static IHtmlContent ButtonFor<TModel>(
            this IHtmlHelper<TModel> helper,
            string text,
            string classes = null)
        {
            return new HtmlString($"<button type=\"submit\" class=\"btn btn-brand btn-lg btn-block {classes}\">{text}</button>");
        }
    }
}
