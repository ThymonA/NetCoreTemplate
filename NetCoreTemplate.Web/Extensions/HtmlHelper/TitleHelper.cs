namespace NetCoreTemplate.Web.Extensions.HtmlHelper
{
    using System;
    using System.Linq.Expressions;
    using System.Text;

    using NetCoreTemplate.ViewModels.Interfaces;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class TitleHelper
    {
        public static IHtmlContent H2Title<TViewModel>(
            this IHtmlHelper<TViewModel> helper,
            Expression<Func<TViewModel, int>> expression,
            Tuple<bool, string> view,
            Tuple<bool, string> edit,
            Tuple<bool, string> add)
            where TViewModel : class, IBaseViewModel
        {
            var title = new StringBuilder();
            var value = expression.Compile()(helper.ViewData.Model);

            title.AppendLine("<h2 class=\"font-w700 text-black mb-10\">");

            if (value == default(int))
            {
                if (add.Item1)
                {
                    title.AppendLine("<i class=\"fa fa-plus text-muted mr-5\"></i> " + add.Item2);
                }
                else
                {
                    title.AppendLine("<i class=\"fa fa-eye text-muted mr-5\"></i> " + view.Item2);
                }
            }
            else
            {
                if (edit.Item1)
                {
                    title.AppendLine("<i class=\"fa fa-pencil text-muted mr-5\"></i> " + edit.Item2);
                }
                else
                {
                    title.AppendLine("<i class=\"fa fa-eye text-muted mr-5\"></i> " + view.Item2);
                }
            }

            title.AppendLine("</h2>");

            return new HtmlString(title.ToString());
        }
    }
}
