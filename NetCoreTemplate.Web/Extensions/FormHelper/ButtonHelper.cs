namespace NetCoreTemplate.Web.Extensions.FormHelper
{
    using System;
    using System.Linq.Expressions;

    using NetCoreTemplate.SharedKernel.Extensions;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class ButtonHelper
    {
        public static IHtmlContent ButtonFor<TModel, TProperty>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string text,
            string value,
            string classes = null)
        {
            var name = helper.NameFor(expression);
            var id = helper.IdFor(expression);
            
            return new HtmlString($"<button type=\"submit\" name=\"{name}\" id=\"{id}\" value=\"{value}\" class=\"{classes}\">{text}</button>");
        }

        public static IHtmlContent ButtonWithIconFor<TModel, TProperty>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string text,
            string value,
            string iconClass,
            string classes = null)
        {
            var name = helper.NameFor(expression);
            var id = helper.IdFor(expression);

            return new HtmlString($"<button type=\"submit\" name=\"{name}\" id=\"{id}\" value=\"{value}\" class=\"{classes}\"><i class=\"{iconClass}\"></i>{text}</button>");
        }
    }
}
