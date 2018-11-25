namespace NetCoreTemplate.Web.Extensions.HtmlHelper
{
    using System.Collections.Generic;
    using System.Text;

    using NetCoreTemplate.ViewModels.Interfaces;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class LabelHelper
    {
        public static IHtmlContent CountLabel<TViewModel>(
            this IHtmlHelper<TViewModel> helper,
            IDictionary<string, string> optionals = null)
            where TViewModel : class, IBaseListViewModel
        {
            var model = helper.ViewData.Model;
            var label = new StringBuilder();

            var numberOfItemsPerPage = model.PageSize;
            var maxPages = model.PageCount;
            var currentPage = model.PageNumber;
            var numberOfResults = model.TotalItemCount;

            label.AppendLine("<div class=\"dataTables_info\" id=\"bs4-table_info\" role=\"status\" aria-live=\"polite\">");

            if (currentPage > maxPages)
            {
                label.AppendLine($"0 tot 0 van {numberOfResults} weergeven");
            }
            else
            {
                var currentMaxResults = currentPage * numberOfItemsPerPage;
                var startNumber = currentMaxResults - numberOfItemsPerPage + 1;

                label.AppendLine(currentMaxResults > numberOfResults
                    ? $"{startNumber} tot {numberOfResults} van {numberOfResults} weergeven"
                    : $"{startNumber} tot {currentMaxResults} van {numberOfResults} weergeven");
            }

            label.AppendLine("</div>");

            return new HtmlString(label.ToString());
        }
    }
}
