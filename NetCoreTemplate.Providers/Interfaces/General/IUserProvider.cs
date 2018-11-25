namespace NetCoreTemplate.Providers.Interfaces.General
{
    using NetCoreTemplate.DAL.Models.General;

    public interface IUserProvider : IBaseProvider<User>
    {
        User GetUserById(int userId);

        User GetUserByEmail(string email);
    }
}
