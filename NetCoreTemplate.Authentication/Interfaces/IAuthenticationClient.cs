namespace NetCoreTemplate.Authentication
{
    using System.Security.Claims;

    using NetCoreTemplate.Authentication.Models.Response;
    using NetCoreTemplate.DAL.Models.General;

    public interface IAuthenticationClient
    {
        ClaimsPrincipal SignIn(string email, string password);

        GetUserResponse GetUserInformation(string email);

        GetUserResponse GetUserInformation(User user);
    }
}
