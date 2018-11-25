namespace NetCoreTemplate.Web.Extensions.FormHelper
{
    using System;
    using System.Linq.Expressions;
    using System.Text;

    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.ViewModels.Interfaces;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class LabelHelper
    {
        public static IHtmlContent BoxedLabelFor<TModel, TProperty>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string label)
            where TModel : IBaseViewModel
        {
            var stringBuilder = new StringBuilder();
            var name = helper.NameFor(expression);
            var id = helper.IdFor(expression);
            var error = helper.ValidationMessageFor(expression).ToHtmlString();
            var hasError = error.Contains("field-validation-error");
            var value = helper.ValueFor(expression);

            stringBuilder.AppendLine("<div class=\"form-group\">");
            stringBuilder.AppendLine($"<label for=\"{id}\" class=\"label-text\">{label}</label>");
            stringBuilder.AppendLine($"<span id=\"{id}\" name=\"{name}\" value=\"{value}\" class=\"label-text\">{value}</span>");

            if (hasError)
            {
                stringBuilder.AppendLine(error);
            }

            stringBuilder.AppendLine("</div>");

            return new HtmlString(stringBuilder.ToString());
        }

        public static IHtmlContent BoxedLabelFor<TModel>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, DateTime>> expression,
            string label)
            where TModel : IBaseViewModel
        {
            var stringBuilder = new StringBuilder();
            var name = helper.NameFor(expression);
            var id = helper.IdFor(expression);
            var error = helper.ValidationMessageFor(expression).ToHtmlString();
            var hasError = error.Contains("field-validation-error");
            var value = helper.ValueFor(expression);
            value = value.Split(" ")[0];

            stringBuilder.AppendLine("<div class=\"form-group\">");
            stringBuilder.AppendLine($"<label for=\"{id}\" class=\"label-text\">{label}</label>");
            stringBuilder.AppendLine($"<span id=\"{id}\" name=\"{name}\" value=\"{value}\" class=\"label-text\">{value}</span>");

            if (hasError)
            {
                stringBuilder.AppendLine(error);
            }

            stringBuilder.AppendLine("</div>");

            return new HtmlString(stringBuilder.ToString());
        }

        public static IHtmlContent BoxedLabelFor<TModel>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, DateTime?>> expression,
            string label)
            where TModel : IBaseViewModel
        {
            var stringBuilder = new StringBuilder();
            var name = helper.NameFor(expression);
            var id = helper.IdFor(expression);
            var error = helper.ValidationMessageFor(expression).ToHtmlString();
            var hasError = error.Contains("field-validation-error");
            var value = helper.ValueFor(expression);

            if (string.IsNullOrWhiteSpace(value) || value.Length < 1)
            {
                return BoxedLabelFor(helper, expression, "-", label);
            }

            value = value.Split(" ")[0];

            stringBuilder.AppendLine("<div class=\"form-group\">");
            stringBuilder.AppendLine($"<label for=\"{id}\" class=\"label-text\">{label}</label>");
            stringBuilder.AppendLine($"<span id=\"{id}\" name=\"{name}\" value=\"{value}\" class=\"label-text\">{value}</span>");

            if (hasError)
            {
                stringBuilder.AppendLine(error);
            }

            stringBuilder.AppendLine("</div>");

            return new HtmlString(stringBuilder.ToString());
        }

        public static IHtmlContent BoxedLabelFor<TModel, TProperty>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            string value,
            string label)
            where TModel : IBaseViewModel
        {
            var stringBuilder = new StringBuilder();
            var name = helper.NameFor(expression);
            var id = helper.IdFor(expression);
            var error = helper.ValidationMessageFor(expression).ToHtmlString();
            var hasError = error.Contains("field-validation-error");

            stringBuilder.AppendLine("<div class=\"form-group\">");
            stringBuilder.AppendLine($"<label for=\"{id}\" class=\"label-text\">{label}</label>");
            stringBuilder.AppendLine($"<span id=\"{id}\" name=\"{name}\" value=\"{value}\" class=\"label-text\">{value}</span>");

            if (hasError)
            {
                stringBuilder.AppendLine(error);
            }

            stringBuilder.AppendLine("</div>");

            return new HtmlString(stringBuilder.ToString());
        }
    }
}
