namespace NetCoreTemplate.Web.Extensions.HtmlHelper
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModels.Interfaces;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class PaginationHelper
    {
        public static IHtmlContent Pagination<TListViewModel>(
            this IHtmlHelper<TListViewModel> helper,
            IDictionary<string, string> optionals = null)
            where TListViewModel : class, IBaseListViewModel
        {
            var model = helper.ViewData.Model;
            var pagination = new StringBuilder();

            pagination.AppendLine("<nav aria-label=\"navigation\">");
            pagination.AppendLine("<ul class=\"pagination\">");

            if (model.PageNumber > 1)
            {
                var prevHref = PagingUrl((model.PageNumber - 1).ToString(), model.SearchTerm, optionals);
                pagination.AppendLine("<li class=\"page-item\">");
                pagination.AppendLine($"<a href=\"{prevHref}\" class=\"page-link\">{model.Label["General:Dashboard:Previous"]}</a>");
                pagination.AppendLine("</li>");
            }

            if (model.PageNumber <= 1)
            {
                pagination.AppendLine("<li class=\"page-item\">");
                pagination.AppendLine($"<a href=\"#\" class=\"page-link\">{model.Label["General:Dashboard:Previous"]}</a>");
                pagination.AppendLine("</li>");
            }

            var pages = GetPagesToDisplay(model.PageNumber, model.PageCount);

            foreach (var page in pages)
            {
                var currentHref = PagingUrl(page.ToString(), model.SearchTerm, optionals);
                pagination.AppendLine($"<li class=\"page-item {(model.PageNumber == page ? "active" : string.Empty)}\">");
                pagination.AppendLine($"<a class=\"page-link\" href=\"{currentHref}\">{page}</a>");
                pagination.AppendLine("</li>");
            }

            if (model.PageNumber < model.PageCount)
            {
                var nextHref = PagingUrl((model.PageNumber + 1).ToString(), model.SearchTerm, optionals);
                pagination.AppendLine("<li class=\"page-item\">");
                pagination.AppendLine($"<a href=\"{nextHref}\" class=\"page-link\">{model.Label["General:Dashboard:Next"]}</a>");
                pagination.AppendLine("</li>");
            }

            if (model.PageNumber >= model.PageCount)
            {
                pagination.AppendLine("<li class=\"page-item\">");
                pagination.AppendLine($"<a href=\"#\" class=\"page-link\">{model.Label["General:Dashboard:Next"]}</a>");
                pagination.AppendLine("</li>");
            }

            pagination.AppendLine("</ul></nav>");

            return new HtmlString(pagination.ToString());
        }

        private static string PagingUrl(string pageNumber, string searchTerm, IDictionary<string, string> optionals = null)
        {
            var context = ServiceContainer.Current.GetService<IHttpContextAccessor>().HttpContext;
            var redirectUrl = $"{context.Request.PathBase.Value}";

            var queryStringValues = HttpUtility.ParseQueryString(context.Request.QueryString.ToString());

            queryStringValues.Set("page", pageNumber);

            if (!string.IsNullOrEmpty(searchTerm))
            {
                queryStringValues.Set("searchTerm", searchTerm);
            }

            if (optionals != null)
            {
                foreach (var optional in optionals.Where(optional => optional.Key != "page"))
                {
                    queryStringValues.Set(optional.Key, optional.Value);
                }
            }

            return $"{redirectUrl}?{queryStringValues}";
        }

        private static IEnumerable<int> GetPagesToDisplay(int pageNumber, int pageCount)
        {
            return new List<int> { pageNumber - 2, pageNumber - 1, pageNumber, pageNumber + 1, pageNumber + 2 }
                .Where(x => x >= 1 && x <= pageCount);
        }
    }
}
