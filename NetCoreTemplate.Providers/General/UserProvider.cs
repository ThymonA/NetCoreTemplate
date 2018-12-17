namespace NetCoreTemplate.Providers.General
{
    using System;
    using System.Linq;

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Base;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;

    public sealed class UserProvider : BaseProvider<User>, IUserProvider
    {
        public UserProvider(IPersistenceLayer persistence)
            : base(persistence)
        {
        }

        public User GetUserById(int userId)
        {
            return Persistence.Get<User>().FirstOrDefault(user => user.Id == userId);
        }

        public User GetUserByEmail(string email)
        {
            return Persistence.Get<User>().FirstOrDefault(user =>
                user.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

        public User GetUserByToken(string token)
        {
            return Persistence.Get<User>().FirstOrDefault(user => user.ResetToken.Equals(token));
        }
    }
}
