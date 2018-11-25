namespace NetCoreTemplate.Web.Controllers.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using NetCoreTemplate.Authentication;
    using NetCoreTemplate.DAL.Permissions;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using Action = DAL.Permissions.Action;
    using Type = DAL.Permissions.Type;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ClaimAuthorizationAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly List<Tuple<Module, Type, Action>> permissions;

        public ClaimAuthorizationAttribute(Module module, Type type, params Action[] actions)
        {
            permissions = new List<Tuple<Module, Type, Action>>();
            permissions.AddRange(actions.Select(x => new Tuple<Module, Type, Action>(module, type, x)));
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userHasClaim = user.Claims.Any(
                x => permissions.Any(
                    y => GetClaimName(y.Item1, y.Item2, y.Item3)
                        .Equals(x.Type)));

            if (!userHasClaim)
            {
                context.Result = new ForbidResult();
            }
        }

        private string GetClaimName(Module module, Type type, Action action)
            => $"{Claims.Permission}{Permissions.GetActionKey(module, type, action)}";
    }
}
