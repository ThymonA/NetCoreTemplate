namespace NetCoreTemplate.Web.Extensions.FormHelper
{
    using System;
    using System.Linq.Expressions;
    using System.Text;

    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.ViewModels.Interfaces;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class CheckboxHelper
    {
        public static IHtmlContent CheckboxFor<TModel>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> expression,
            bool disable = false)
            where TModel : IBaseViewModel
        {
            return CheckboxFor(helper, expression, string.Empty, disable);
        }

        public static IHtmlContent CheckboxFor<TModel>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> expression,
            string label,
            bool disable = false)
            where TModel : IBaseViewModel
        {
            var stringBuilder = new StringBuilder();
            var id = helper.IdFor(expression);
            var name = helper.NameFor(expression);
            var error = helper.ValidationMessageFor(expression).ToHtmlString();
            var hasError = error.Contains("field-validation-error");
            var value = helper.ValueFor(expression);
            var hasChecked = value.ToBoolean();

            stringBuilder.AppendLine("<label class=\"custom-control custom-checkbox\">");
            //stringBuilder.AppendLine($"<input type=\"checkbox\" class=\"custom-control-input\" id=\"{id}\" name=\"{name}\"{(hasChecked ? " checked" : string.Empty)}>");
            stringBuilder.AppendLine((disable ?
                  helper.CheckBoxFor(expression, new { @class = "custom-control-input", disabled = "disabled" }) :
                  helper.CheckBoxFor(expression, new { @class = "custom-control-input" }))
                .ToHtmlString());
            stringBuilder.AppendLine($"<span class=\"custom-control-label\">{(!string.IsNullOrWhiteSpace(label) ? label : string.Empty)}</span>");
            stringBuilder.AppendLine("</label>");

            if (hasError)
            {
                stringBuilder.AppendLine(error);
            }

            return new HtmlString(stringBuilder.ToString());
        }

        public static IHtmlContent BoxedCheckboxFor<TModel>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> expression,
            bool disable = false)
            where TModel : IBaseViewModel
        {
            return BoxedCheckboxFor(helper, expression, string.Empty, disable);
        }

        public static IHtmlContent BoxedCheckboxFor<TModel>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> expression,
            string label,
            bool disable = false)
            where TModel : IBaseViewModel
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("<div class=\"form-group\">");
            stringBuilder.AppendLine(CheckboxFor(helper, expression, label, disable).ToHtmlString());
            stringBuilder.AppendLine("</div>");

            return new HtmlString(stringBuilder.ToString());
        }
    }
}
