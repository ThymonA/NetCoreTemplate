namespace NetCoreTemplate.DAL.Initializers.General
{
    using System.Collections.Generic;

    using NetCoreTemplate.DAL.Extensions;
    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.SharedKernel.Hashing;

    public sealed class UserInitializer : BaseInitializer<User>
    {
        public UserInitializer(DatabaseContext context)
            : base(context)
        {
        }

        public override List<User> SeedData()
        {
            var list = GetUsers();

            Context.AddOrUpdateRange(
                x => x.User,
                x => x.Id,
                list);

            return list;
        }

        private List<User> GetUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    Email = "contact@thymonarens.nl",
                    Password = PBFDF2Hash.Hash("0Py6WVIPi5JZycVK"),
                    Firstname = "Thymon",
                    Lastname = "Arens",
                    ResetToken = string.Empty,
                    ResetTokenDate = null,
                    Active = true
                },
                new User
                {
                    Id = 2,
                    Email = "system",
                    Password = string.Empty,
                    Firstname = "System",
                    Lastname = "System",
                    ResetToken = string.Empty,
                    ResetTokenDate = null,
                    Active = false
                }
            };
        }
    }
}
