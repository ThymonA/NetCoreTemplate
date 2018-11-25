namespace NetCoreTemplate.Authentication.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Authentication.Cookies;

    using NetCoreTemplate.Authentication.Exceptions;
    using NetCoreTemplate.Authentication.Models.Response;
    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.Hashing;

    public class AuthenticationClient : IAuthenticationClient
    {
        private readonly IUserProvider userProvider;
        private readonly IPermissionProvider permissionProvider;

        public AuthenticationClient(
            IUserProvider userProvider,
            IPermissionProvider permissionProvider)
        {
            this.userProvider = userProvider;
            this.permissionProvider = permissionProvider;
        }

        public GetUserResponse GetUserInformation(string email)
        {
            var user = userProvider.GetUserByEmail(email);

            if (user.IsNullOrDefault())
            {
                throw new Exception();
            }

            return GetUserInformation(user);
        }

        public GetUserResponse GetUserInformation(User user)
        {
            return new GetUserResponse
            {
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname
            };
        }

        public ClaimsPrincipal SignIn(string email, string password)
        {
            var user = userProvider.GetUserByEmail(email);

            if (user.IsNullOrDefault() || !PBFDF2Hash.Verify(password, user.Password))
            {
                throw new WrongPasswordException();
            }

            if (!user.Active)
            {
                throw new DeactivatedException();
            }

            var permissions = permissionProvider.GetPermissions(user);
            var permissionClaims = permissions.Select(x => new Claim($"{Claims.Permission}{x.Action}", x.Id.ToString()));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.GivenName, user.Firstname),
                new Claim(ClaimTypes.Surname, user.Lastname),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(Claims.UserId, user.Id.ToString()),
            };

            claims.AddRange(permissionClaims);

            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(userIdentity);

            return principal;
        }
    }
}
