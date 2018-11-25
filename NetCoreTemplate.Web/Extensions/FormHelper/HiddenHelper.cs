namespace NetCoreTemplate.Web.Extensions.FormHelper
{
    using System;
    using System.Linq.Expressions;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class HiddenHelper
    {
        public static IHtmlContent BoxedHiddenFor<TModel, TProperty>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            TProperty value)
        {
            var name = helper.NameFor(expression);
            var id = helper.IdFor(expression);

            return new HtmlString($"<input id=\"{id}\" name=\"{name}\" type=\"hidden\" value=\"{value}\">");
        }
    }
}
