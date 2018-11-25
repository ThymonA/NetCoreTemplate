namespace NetCoreTemplate.Web.Extensions.FormHelper
{
    using System;
    using System.Linq.Expressions;
    using System.Text;

    using NetCoreTemplate.SharedKernel.Extensions;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class SignatureHelper
    {
        public static IHtmlContent BoxedSignatureFor<TModel>(
            this IHtmlHelper<TModel> helper,
            Expression<Func<TModel, string>> expression,
            string label)
        {
            var id = helper.IdFor(expression);
            var value = helper.ValueFor(expression);
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(helper.HiddenFor(expression).ToHtmlString());
            stringBuilder.AppendLine($"<span class=\"signature-title\">{label}</span>");
            stringBuilder.AppendLine($"<div id=\"{id}Signature\" class=\"signature\"></div>");
            stringBuilder.AppendLine($"<a class=\"btn btn-secondary resetBtn\" id=\"{id}Reset\">Reset Handtekening</a>");
            stringBuilder.AppendLine(@"
            <script>
                $(document).ready(function() {
                    var hasImage = " + (string.IsNullOrWhiteSpace(value) ? "false" : "true") + @";
                    var " + id.ToLower() + @";

                    if(hasImage) {
                        var value = $('#" + id + @"').val();
                        $('#" + id + @"Signature').append('<img src=""' + value + '"" class=""signature-image"">');
                        $('#" + id + @"Reset').on('click', function() {
                            $('#" + id + @"Signature').empty();
                            " + id.ToLower() + @" = $('#" + id + @"Signature').jSignature({
                                UndoButton: false
                            });

                            $('#" + id + @"Reset').on('click', function() {
                                " + id.ToLower() + @".jSignature('reset');
                            });

                            $('#" + id + @"Signature').bind('change', function(e) {
                                $('#" + id + @"').val(" + id.ToLower() + @".jSignature('getData'));
                            });
                        });
                    } else {
                        " + id.ToLower() + @" = $('#" + id + @"Signature').jSignature({
                            UndoButton: false
                        });

                        $('#" + id + @"Reset').on('click', function() {
                            " + id.ToLower() + @".jSignature('reset');
                        });

                        $('#" + id + @"Signature').bind('change', function(e) {
                            $('#" + id + @"').val(" + id.ToLower() + @".jSignature('getData'));
                        });
                    }
                });
            </script>");

            return new HtmlString(stringBuilder.ToString());
        }
    }
}
