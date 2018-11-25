namespace NetCoreTemplate.Web.Extensions.FormHelper
{
    using System;
    using System.Linq.Expressions;
    using System.Text;

    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.ViewModels.Interfaces;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class TextBoxHelper
    {
        public static IHtmlContent BoxedTextBoxFor<TModel, TProperty>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string label,
            bool disable = false,
            bool hideLabel = false,
            string textboxType = "text")
            where TModel : IBaseViewModel
        {
            var stringBuilder = new StringBuilder();
            var id = helper.IdFor(expression);
            var error = helper.ValidationMessageFor(expression).ToHtmlString();
            var hasError = error.Contains("field-validation-error");
            var value = helper.ValueFor(expression);

            stringBuilder.AppendLine("<div class=\"form-group\">");

            if (!hideLabel)
            {
                stringBuilder.AppendLine($"<label for=\"{id}\">{label}:</label>");
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                stringBuilder.AppendLine((disable ?
                    helper.TextBoxFor(expression, null, new { @class = "form-control", placeholder = label, type = textboxType, disabled = "disabled", autocomplete = "off", value = "" }) :
                    helper.TextBoxFor(expression, null, new { @class = "form-control", placeholder = label, type = textboxType, autocomplete = "off", value = "" }))
                    .ToHtmlString());
            }
            else
            {
                stringBuilder.AppendLine((disable ?
                    helper.TextBoxFor(expression, null, new { @class = "form-control", placeholder = label, type = textboxType, disabled = "disabled", autocomplete = "off" }) :
                    helper.TextBoxFor(expression, null, new { @class = "form-control", placeholder = label, type = textboxType, autocomplete = "off" }))
                    .ToHtmlString());
            }

            if (hasError)
            {
                stringBuilder.AppendLine(error);
            }

            stringBuilder.AppendLine("</div>");

            return new HtmlString(stringBuilder.ToString());
        }

        public static IHtmlContent BoxedTextAreaFor<TModel, TProperty>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string label,
            bool disable = false,
            string textboxType = "text")
            where TModel : IBaseViewModel
        {
            var stringBuilder = new StringBuilder();
            var id = helper.IdFor(expression);
            var error = helper.ValidationMessageFor(expression).ToHtmlString();
            var hasError = error.Contains("field-validation-error");

            stringBuilder.AppendLine("<div class=\"form-group\">");
            stringBuilder.AppendLine($"<label for=\"{id}\">{label}</label>");
            stringBuilder.AppendLine((disable ?
                helper.TextAreaFor(expression, new { @class = "form-control form-control-line", placeholder = label, type = textboxType, disabled = "disabled" }) :
                helper.TextAreaFor(expression, new { @class = "form-control form-control-line", placeholder = label, type = textboxType }))
                .ToHtmlString());

            if (hasError)
            {
                stringBuilder.AppendLine(error);
            }

            stringBuilder.AppendLine("</div>");

            return new HtmlString(stringBuilder.ToString());
        }

        public static IHtmlContent BoxedEditorFor<TModel, TProperty>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string label,
            bool disable = false)
            where TModel : IBaseViewModel
        {
            var stringBuilder = new StringBuilder();
            var id = helper.IdFor(expression);
            var error = helper.ValidationMessageFor(expression).ToHtmlString();
            var hasError = error.Contains("field-validation-error");

            stringBuilder.AppendLine("<div class=\"form-group\">");
            stringBuilder.AppendLine($"<label for=\"{id}\">{label}</label>");
            stringBuilder.AppendLine((disable ?
                helper.TextAreaFor(expression, new { @class = "form-control", placeholder = label, disabled = "disabled" }) :
                helper.TextAreaFor(expression, new { @class = "form-control", placeholder = label }))
                .ToHtmlString());

            if (hasError)
            {
                stringBuilder.AppendLine(error);
            }

            stringBuilder.AppendLine(
                $@"<script>
                    $(document).ready(function() {{
                        $('#{id}').froalaEditor();
                    }});
                </script>");
            stringBuilder.AppendLine("</div>");

            return new HtmlString(stringBuilder.ToString());
        }
    }
}
