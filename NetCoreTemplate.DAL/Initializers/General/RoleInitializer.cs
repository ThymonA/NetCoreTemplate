namespace NetCoreTemplate.DAL.Initializers.General
{
    using System.Collections.Generic;
    using System.Linq;

    using NetCoreTemplate.DAL.Extensions;
    using NetCoreTemplate.DAL.Models.General;

    public sealed class RoleInitializer : BaseInitializer<Role>
    {
        private readonly IQueryable<Permission> permissions;
        private readonly IQueryable<User> users;

        public RoleInitializer(
            IQueryable<Permission> permissions,
            IQueryable<User> users,
            DatabaseContext context)
            : base(context)
        {
            this.permissions = permissions;
            this.users = users;
        }

        public override List<Role> SeedData()
        {
            var list = GetRoles();

            Context.AddOrUpdateRange(
                x => x.Role,
                x => x.Id,
                list);

            AddAllPermissionsToAdmin();
            AddAllRolesToAdmin();

            return list;
        }

        private List<Role> GetRoles()
        {
            return new List<Role>
            {
                new Role
                {
                   Id = 1,
                   Name = "Admin",
                   Active = true
                }
            };
        }

        private void AddAllPermissionsToAdmin()
        {
            var rolePermissions = GetAllRolePermissions();
            var existingRolePermissions = Context.RolePermission.ToList();
            var addRolePermissions = rolePermissions.Where(
                x => !existingRolePermissions.Any(
                    y => y.Permission_Id == x.Permission_Id &&
                    y.Role_Id == x.Role_Id));

            Context.RolePermission.AddRange(addRolePermissions);

            Context.SaveChanges();
        }

        private void AddAllRolesToAdmin()
        {
            var userRoles = GetAllUserRoles();
            var existingUserRoles = Context.UserRole.ToList();
            var addUserRoles = userRoles.Where(
                x => !existingUserRoles.Any(
                    y => y.User_Id == x.User_Id &&
                    y.Role_Id == x.Role_Id));

            Context.UserRole.AddRange(addUserRoles);

            Context.SaveChanges();
        }

        private List<RolePermission> GetAllRolePermissions()
        {
            var allPermissions = permissions.ToList();
            var adminRole = GetRoles().First(x => x.Id == 1);

            return allPermissions.Select(
                 x => new RolePermission
                 {
                     Permission = x,
                     Permission_Id = x.Id,
                     Role = adminRole,
                     Role_Id = adminRole.Id
                 })
                .ToList();
        }

        private List<UserRole> GetAllUserRoles()
        {
            var firstUser = users.First(x => x.Id == 1);
            var allRoles = GetRoles();

            var userRoles = new List<UserRole>();

            foreach (var role in allRoles)
            {
                userRoles.Add(new UserRole
                {
                    Role = role,
                    Role_Id = role.Id,
                    User = firstUser,
                    User_Id = firstUser.Id
                });
            }

            return userRoles;
        }
    }
}
