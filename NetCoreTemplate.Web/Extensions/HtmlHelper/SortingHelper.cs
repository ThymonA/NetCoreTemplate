namespace NetCoreTemplate.Web.Extensions.HtmlHelper
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Web;

    using NetCoreTemplate.DAL.Models.Base;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModels.Interfaces;

    using Microsoft.AspNetCore.Html;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public static class SortingHelper
    {
        public static IHtmlContent SortingHeader<TModel>(
            this IHtmlHelper<TModel> helper,
            string name,
            string columTitle)
            where TModel : class, IBaseListViewModel
        {
            var model = helper.ViewData.Model;
            var searchTerm = model.SearchTerm;
            var httpContext = ServiceContainer.Current.GetService<IHttpContextAccessor>().HttpContext;

            var states = GetActiveStates(model, httpContext, name);
            var active = states.Item1;
            var orderByDesc = states.Item2;

            return new HtmlString($"<a class=\"{(active ? "active" : "non-active")}\" style=\"text-decoration: none\" href=\"{SortingUrl(httpContext, name, InverseSortingDirection(model, httpContext, name), searchTerm, model.Path)}\">{columTitle}</a></span><span class=\"sort\"><a href=\"{SortingUrl(httpContext, name, false, searchTerm, model.Path)}\" class=\"sort-up {(!orderByDesc ? "active" : "non-active")}\"><i class=\"fa fa-angle-up\"></i></a><a href=\"{SortingUrl(httpContext, name, true, searchTerm, model.Path)}\" class=\"sort-down {(orderByDesc ? "active" : "non-active")}\"><i class=\"fa fa-angle-down\"></i></a>");
        }

        public static IHtmlContent SortingHeader<TModel, TDbModel>(
        this IHtmlHelper<TModel> helper,
        Expression<Func<TDbModel, object>> sortingExpression,
        string columTitle)
        where TModel : class, IBaseListViewModel
        where TDbModel : TrackableEntity
        {
            var model = helper.ViewData.Model;
            var name = helper.DisplayName(sortingExpression.Body.ToString());
            name = name.Split(", ")[0];
            var searchTerm = model.SearchTerm;
            var httpContext = ServiceContainer.Current.GetService<IHttpContextAccessor>().HttpContext;

            var states = GetActiveStates(model, httpContext, name);
            var active = states.Item1;
            var orderByDesc = states.Item2;

            return new HtmlString($"<a class=\"{(active ? "active" : "non-active")}\" style=\"text-decoration: none\" href=\"{SortingUrl(httpContext, name, InverseSortingDirection(model, httpContext, name), searchTerm, model.Path)}\">{columTitle}</a></span><span class=\"sort\"><a href=\"{SortingUrl(httpContext, name, false, searchTerm, model.Path)}\" class=\"sort-up {(!orderByDesc ? "active" : "non-active")}\"><i class=\"fa fa-angle-up\"></i></a><a href=\"{SortingUrl(httpContext, name, true, searchTerm, model.Path)}\" class=\"sort-down {(orderByDesc ? "active" : "non-active")}\"><i class=\"fa fa-angle-down\"></i></a>");
        }

        private static string SortingUrl(
            HttpContext httpContext,
            string sortPropertyName,
            bool sortDescending,
            string searchTerm,
            string baseUrl)
        {
            var queryStringValues = HttpUtility.ParseQueryString(httpContext.Request.QueryString.ToString());

            queryStringValues.Set("sortBy", sortPropertyName);
            queryStringValues.Set("sortDescending", sortDescending.ToString());
            queryStringValues.Set("searchTerm", searchTerm);
            queryStringValues.Set("page", "1");

            return $"{baseUrl}?{queryStringValues}";
        }

        private static bool InverseSortingDirection(IBaseListViewModel model, HttpContext httpContext, string name)
        {
            var states = GetActiveStates(model, httpContext, name);

            return states.Item1 & !states.Item2;
        }

        private static Tuple<bool, bool> GetActiveStates(IBaseListViewModel model, HttpContext httpContext, string name)
        {
            var queryStringValues = HttpUtility.ParseQueryString(httpContext.Request.QueryString.ToString());
            bool active;
            var orderByDesc = model.OrderByDesc;

            if (queryStringValues.HasKeys() && queryStringValues.AllKeys.Contains("sortBy"))
            {
                var currentSortBy = queryStringValues["sortBy"];
                active = currentSortBy.Equals(name, StringComparison.OrdinalIgnoreCase);

                if (queryStringValues.AllKeys.Contains("sortDescending"))
                {
                    orderByDesc = queryStringValues["sortDescending"].ToBoolean();
                }
            }
            else
            {
                active = model.DefaultOrderBy.Equals(name, StringComparison.OrdinalIgnoreCase);
            }

            return new Tuple<bool, bool>(active, orderByDesc);
        }
    }
}
