namespace NetCoreTemplate.Web.Extensions.FormHelper
{
    using System;
    using System.Linq.Expressions;
    using System.Text;

    using NetCoreTemplate.Providers.Interfaces;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModels.Interfaces;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    using NetCoreTemplate.DAL.Models.Contact;

    public static class AutocompleteHelper
    {
        public static IHtmlContent BoxedContactSearchFor<TModel>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, int>> expression,
            string label,
            bool disable = false)
            where TModel : IBaseViewModel
        {
            var stringBuilder = new StringBuilder();
            var id = helper.IdFor(expression);
            var name = helper.NameFor(expression);
            var error = helper.ValidationMessageFor(expression).ToHtmlString();
            var hasError = error.Contains("field-validation-error");
            var value = helper.ValueFor(expression).ToInt();
            var contactProvider = ServiceContainer.Current.GetService<IBaseProvider<Contact>>();
            var contact = contactProvider.GetEntity(x => x.Id == value);

            stringBuilder.AppendLine("<div class=\"form-group\">");
            stringBuilder.AppendLine($"<label for=\"{id}\">{label}</label>");
            stringBuilder.AppendLine($"<select class=\"js-select2 js-select2-enabled form-control\" data-val=\"true\" id=\"{id}\" name=\"{name}\"{(disable ? " disabled=\"disabled\"" : string.Empty)}>");

            if (!contact.IsNullOrDefault())
            {
                var text = !string.IsNullOrWhiteSpace(contact.Company) ? contact.Company
                    : $"{(string.IsNullOrWhiteSpace(contact.Lastname) ? string.Empty : $"{contact.Lastname}, ")}{contact.Firstname}";
                stringBuilder.AppendLine($"<option value=\"{contact.Id}\" selected=\"selected\">{$"{text}".RemoveMiltipleSpaces()}</option>");
            }

            stringBuilder.AppendLine("</select>");
            stringBuilder.AppendLine(
                $@"<script>
                $(document).ready(function() {{
                    $('#{id}').select2({{
                        ajax: {{
                            delay: 250,
                            dataType: 'json',
                            url: '/customer/autocomplete/',
                            data: function (params) {{
                                var query = {{
                                    input: params.term
                                }}
                
                                return query;
                            }},
                            cache: true
                        }}
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

        public static IHtmlContent BoxedContactSearchFor<TModel>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, int?>> expression,
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

            stringBuilder.AppendLine("<div class=\"form-group\">");
            stringBuilder.AppendLine($"<label for=\"{id}\">{label}</label>");
            stringBuilder.AppendLine($"<select class=\"js-select2 js-select2-enabled form-control\" data-val=\"true\" id=\"{id}\" name=\"{name}\"{(disable ? " disabled=\"disabled\"" : string.Empty)}>");

            if (string.IsNullOrWhiteSpace(value))
            {
                var contactProvider = ServiceContainer.Current.GetService<IBaseProvider<Contact>>();
                var contact = contactProvider.GetEntity(x => x.Id == value.ToInt());

                if (!contact.IsNullOrDefault())
                {
                    stringBuilder.AppendLine($"<option value=\"{contact.Id}\" selected=\"selected\">{$"{contact.Lastname}, {contact.Firstname}".RemoveMiltipleSpaces()}</option>");
                }
                else
                {
                    stringBuilder.AppendLine($"<option value=\"\" selected=\"selected\">Geen relatie geselecteerd</option>");
                }
            }

            stringBuilder.AppendLine("</select>");
            stringBuilder.AppendLine(
                $@"<script>
                $(document).ready(function() {{
                    $('#{id}').select2({{
                        ajax: {{
                            delay: 250,
                            dataType: 'json',
                            url: '/customer/autocomplete/',
                            data: function (params) {{
                                var query = {{
                                    input: params.term
                                }}
                
                                return query;
                            }},
                            cache: true
                        }}
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
