namespace NetCoreTemplate.Web.Extensions.FormHelper
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;

    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.ViewModels.Interfaces;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class DropdownListHelper
    {
        public static IHtmlContent BoxedDropdownFor<TModel, TProperty>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> items,
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
                helper.DropDownListFor(expression, items, new { @class = "js-select2 form-control", disabled = "disabled" }) :
                helper.DropDownListFor(expression, items, new { @class = "js-select2 form-control" }))
                .ToHtmlString());

            if (hasError)
            {
                stringBuilder.AppendLine(error);
            }

            stringBuilder.AppendLine("</div>");

            stringBuilder.AppendLine($@"
            <script>
                $(document).ready(function() {{
                    $('#{id}').select2();
                }});
            </script>");

            return new HtmlString(stringBuilder.ToString());
        }

        public static IHtmlContent DropdownFilterFor<TModel, TProperty>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> items,
            string label = null,
            bool disable = false)
            where TModel : IBaseViewModel
        {
            var stringBuilder = new StringBuilder();
            var name = helper.NameFor(expression);
            var error = helper.ValidationMessageFor(expression).ToHtmlString();
            var hasError = error.Contains("field-validation-error");

            if (!string.IsNullOrWhiteSpace(label))
            {
                stringBuilder.AppendLine($"<label for=\"{name}\">{label}</label>");
            }

            stringBuilder.AppendLine((disable ?
                helper.DropDownListFor(expression, items, new { @class = "js-select2 form-control", disabled = "disabled" }) :
                helper.DropDownListFor(expression, items, new { @class = "js-select2 form-control" }))
                .ToHtmlString());

            stringBuilder.AppendLine("<script>");
            stringBuilder.AppendLine("$('#" + name + "').on('change', function() {");
            stringBuilder.AppendLine("$(this).closest('form').submit();");
            stringBuilder.AppendLine("});");
            stringBuilder.AppendLine("</script>");

            if (hasError)
            {
                stringBuilder.AppendLine(error);
            }

            return new HtmlString(stringBuilder.ToString());
        }

        public static IHtmlContent BoxedDropdownFilterFor<TModel, TProperty>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> items,
            string label = null,
            bool disable = false)
            where TModel : IBaseViewModel
        {
            var stringBuilder = new StringBuilder();
            var name = helper.NameFor(expression);
            var error = helper.ValidationMessageFor(expression).ToHtmlString();
            var hasError = error.Contains("field-validation-error");

            stringBuilder.AppendLine("<div class=\"form-group\">");

            if (!string.IsNullOrWhiteSpace(label))
            {
                stringBuilder.AppendLine($"<label for=\"{name}\">{label}</label>");
            }

            stringBuilder.AppendLine((disable ?
                helper.DropDownListFor(expression, items, new { @class = "js-select2 form-control", disabled = "disabled" }) :
                helper.DropDownListFor(expression, items, new { @class = "js-select2 form-control" }))
                .ToHtmlString());

            stringBuilder.AppendLine($@"
            <script>
                $(document).ready(function() {{
                    $('#{name}').on('change', function() {{
                        $(this).closest('form').submit();
                    }});
                }});
            </script>");

            if (hasError)
            {
                stringBuilder.AppendLine(error);
            }

            stringBuilder.AppendLine("</div>");

            return new HtmlString(stringBuilder.ToString());
        }
    }
}
