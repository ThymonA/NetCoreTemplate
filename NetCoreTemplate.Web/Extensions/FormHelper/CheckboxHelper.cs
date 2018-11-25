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
            return helper.CheckboxFor(expression, string.Empty, disable);
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
            var error = helper.ValidationMessageFor(expression).ToHtmlString();
            var hasError = error.Contains("field-validation-error");

            if (!string.IsNullOrWhiteSpace(label))
            {
                stringBuilder.AppendLine($"<label for=\"{id}\">{label}</label>");
            }

            stringBuilder.AppendLine((disable ?
                  helper.CheckBoxFor(expression, new { @class = "tgl tgl-ios", disabled = "disabled" }) :
                  helper.CheckBoxFor(expression, new { @class = "tgl tgl-ios" }))
                .ToHtmlString());
            stringBuilder.AppendLine($"<label for=\"{id}\" class=\"tgl-btn\"></label>");

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
            return helper.BoxedCheckboxFor(expression, string.Empty, disable);
        }

        public static IHtmlContent BoxedCheckboxFor<TModel>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, bool>> expression,
            string label,
            bool disable = false)
            where TModel : IBaseViewModel
        {
            var stringBuilder = new StringBuilder();
            var id = helper.IdFor(expression);
            var error = helper.ValidationMessageFor(expression).ToHtmlString();
            var hasError = error.Contains("field-validation-error");

            stringBuilder.AppendLine("<div class=\"form-group\">");

            if (!string.IsNullOrWhiteSpace(label))
            {
                stringBuilder.AppendLine($"<label for=\"{id}\">{label}</label>");
            }

            stringBuilder.AppendLine((disable ?
                helper.CheckBoxFor(expression, new { @class = "tgl tgl-ios", disabled = "disabled" }) :
                helper.CheckBoxFor(expression, new { @class = "tgl tgl-ios" }))
                .ToHtmlString());
            stringBuilder.AppendLine($"<label for=\"{id}\" class=\"tgl-btn\"></label>");

            if (hasError)
            {
                stringBuilder.AppendLine(error);
            }

            stringBuilder.AppendLine("</div>");

            return new HtmlString(stringBuilder.ToString());
        }
    }
}
