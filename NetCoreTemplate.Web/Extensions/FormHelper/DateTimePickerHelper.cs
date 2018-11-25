namespace NetCoreTemplate.Web.Extensions.FormHelper
{
    using System;
    using System.Linq.Expressions;
    using System.Text;
    using System.Text.RegularExpressions;

    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.ViewModels.Interfaces;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class DateTimePickerHelper
    {
        public static IHtmlContent BoxedDateTimePickerFor<TModel>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, DateTime>> expression,
            string label,
            bool showOnlyDate = false,
            bool disable = false)
            where TModel : IBaseViewModel
        {
            var stringBuilder = new StringBuilder();
            var name = helper.NameFor(expression);
            var id = helper.IdFor(expression);
            var error = helper.ValidationMessageFor(expression).ToHtmlString();
            var hasError = error.Contains("field-validation-error");
            var model = helper.ViewData.Model;
            var cultureInfo = model.CultureInfo;
            var shortDatePattern = cultureInfo.DateTimeFormat.ShortDatePattern;
            var shortTimePattern = cultureInfo.DateTimeFormat.ShortTimePattern;

            var value = helper.ValueFor(expression);

            stringBuilder.AppendLine("<div class=\"form-group\">");
            stringBuilder.AppendLine($"<label for=\"{id}\">{label}</label>");
            stringBuilder.AppendLine($"<div class=\"input-group date\" id=\"{id}\" data-target-input=\"nearest\">");
            stringBuilder.AppendLine($"<input type=\"text\" class=\"form-control datetimepicker-input\" name=\"{name}\" placeholder=\"{shortDatePattern} {shortTimePattern}\" {(disable ? "disabled=\"disabled\"" : string.Empty)} value=\"{value}\" data-toggle=\"datetimepicker\" data-target=\"#{id}\" autocomplete=\"off\">");
            stringBuilder.AppendLine($"<div class=\"input-group-append\" data-target=\"#{id}\" data-toggle=\"datetimepicker\">");
            stringBuilder.AppendLine("<div class=\"input-group-text\"><i class=\"far fa-calendar\"></i></div>");
            stringBuilder.AppendLine("</div>");
            stringBuilder.AppendLine("</div>");

            if (showOnlyDate)
            {
                stringBuilder.AppendLine(
                    $@"<script>
                    $(document).ready(function() {{
                        $('#{id}').datetimepicker({{
                            locale: 'nl',
                            format: 'L'
                        }});
                    }});
                </script>");
            }
            else
            {
                stringBuilder.AppendLine(
                    $@"<script>
                    $(document).ready(function() {{
                        $('#{id}').datetimepicker({{
                            locale: 'nl'
                        }});
                    }});
                </script>");
            }

            if (hasError)
            {
                stringBuilder.AppendLine(error);
            }

            stringBuilder.AppendLine("</div>");

            return new HtmlString(stringBuilder.ToString());
        }

        public static IHtmlContent BoxedDatePickerFor<TModel>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, DateTime>> expression,
            string label,
            bool disable = false)
            where TModel : IBaseViewModel
        {
            var stringBuilder = new StringBuilder();
            var name = helper.NameFor(expression);
            var id = helper.NameFor(expression);
            var error = helper.ValidationMessageFor(expression).ToHtmlString();
            var hasError = error.Contains("field-validation-error");
            var model = helper.ViewData.Model;
            var shortDatePattern = model.CultureInfo.DateTimeFormat.ShortDatePattern;

            if (Regex.Matches(shortDatePattern, "d").Count == 1)
            {
                shortDatePattern = shortDatePattern.Replace("d", "dd");
            }

            if (Regex.Matches(shortDatePattern, "M").Count == 1)
            {
                shortDatePattern = shortDatePattern.Replace("M", "MM");
            }

            if (Regex.Matches(shortDatePattern, "y").Count == 1)
            {
                shortDatePattern = shortDatePattern.Replace("y", "YY");
            }

            shortDatePattern = shortDatePattern.ToLower();

            var value = helper.ValueFor(expression);
            value = value.Split(" ")[0];

            stringBuilder.AppendLine("<div class=\"form-group\">");
            stringBuilder.AppendLine($"<label for=\"{id}\">{label}</label>");
            stringBuilder.AppendLine($"<input type=\"text\" class=\"js-datepicker form-control\" id=\"{id}\" name=\"{name}\" data-week-start=\"1\" data-autoclose=\"true\" data-today-highlight=\"true\" data-date-format=\"{shortDatePattern}\" placeholder=\"{shortDatePattern}\" {(disable ? "disabled=\"disabled\"" : string.Empty)} value=\"{value}\" autocomplete=\"off\">");

            if (hasError)
            {
                stringBuilder.AppendLine(error);
            }

            stringBuilder.AppendLine($@"
                <script>
                    $(document).ready(function() {{
                        $('#{id}').datepicker({{
                            language: 'nl',
                            format: 'dd-mm-yyyy',
                            autoclose: true,
                            orientation: 'bottom',
                            templates: {{
                                leftArrow: '<i class=""icon dripicons - chevron - left""></i>',
                                rightArrow: '<i class=""icon dripicons-chevron-right"" ></i>'
                            }}
                        }});
                    }});
                </script>");
            stringBuilder.AppendLine("</div>");

            return new HtmlString(stringBuilder.ToString());
        }

        public static IHtmlContent BoxedDatePickerFor<TModel>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, DateTime?>> expression,
            string label,
            bool disable = false)
            where TModel : IBaseViewModel
        {
            var stringBuilder = new StringBuilder();
            var name = helper.NameFor(expression);
            var id = helper.IdFor(expression);
            var error = helper.ValidationMessageFor(expression).ToHtmlString();
            var hasError = error.Contains("field-validation-error");
            var model = helper.ViewData.Model;
            var shortDatePattern = model.CultureInfo.DateTimeFormat.ShortDatePattern;

            if (Regex.Matches(shortDatePattern, "d").Count == 1)
            {
                shortDatePattern = shortDatePattern.Replace("d", "dd");
            }

            if (Regex.Matches(shortDatePattern, "M").Count == 1)
            {
                shortDatePattern = shortDatePattern.Replace("M", "MM");
            }

            if (Regex.Matches(shortDatePattern, "y").Count == 1)
            {
                shortDatePattern = shortDatePattern.Replace("y", "YY");
            }

            shortDatePattern = shortDatePattern.ToLower();

            var value = helper.ValueFor(expression);
            value = value.Split(" ")[0];

            stringBuilder.AppendLine("<div class=\"form-group\">");
            stringBuilder.AppendLine($"<label for=\"{id}\">{label}</label>");
            stringBuilder.AppendLine($"<input type=\"text\" class=\"js-datepicker form-control\" id=\"{id}\" name=\"{name}\" data-week-start=\"1\" data-autoclose=\"true\" data-today-highlight=\"true\" data-date-format=\"{shortDatePattern}\" placeholder=\"{shortDatePattern}\" {(disable ? "disabled=\"disabled\"" : string.Empty)} value=\"{value}\">");

            if (hasError)
            {
                stringBuilder.AppendLine(error);
            }

            stringBuilder.AppendLine($@"
                <script>
                    $(document).ready(function() {{
                        $('#{id}').datepicker({{
                            language: 'nl',
                            format: 'dd-mm-yyyy',
                            autoclose: true,
                            orientation: 'bottom',
                            templates: {{
                                leftArrow: '<i class=""icon dripicons - chevron - left""></i>',
                                rightArrow: '<i class=""icon dripicons-chevron-right"" ></i>'
                            }}
                        }});
                    }});
                </script>");
            stringBuilder.AppendLine("</div>");

            return new HtmlString(stringBuilder.ToString());
        }
    }
}
