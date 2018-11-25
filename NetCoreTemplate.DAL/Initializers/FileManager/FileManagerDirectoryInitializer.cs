namespace NetCoreTemplate.DAL.Initializers.FileManager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using NetCoreTemplate.DAL.Extensions;
    using NetCoreTemplate.DAL.Models.FileManager;
    using NetCoreTemplate.DAL.Models.General;

    public sealed class FileManagerDirectoryInitializer : BaseInitializer<FileManagerDirectory>
    {
        private readonly IQueryable<User> user;

        public FileManagerDirectoryInitializer(
            IQueryable<User> user,
            DatabaseContext context)
            : base(context)
        {
            this.user = user;
        }

        public override List<FileManagerDirectory> SeedData()
        {
            var list = GetFileManagerDirectories();

            Context.AddOrUpdateRange(
                x => x.FileManagerDirectory,
                x => x.Id,
                list);

            return list;
        }

        private List<FileManagerDirectory> GetFileManagerDirectories()
        {
            var sysUser = user.First(x => x.Email.Equals("system", StringComparison.InvariantCultureIgnoreCase));

            return new List<FileManagerDirectory>
            {
                new FileManagerDirectory
                {
                    Id = 1,
                    Name = "Root",
                    Location = $"{Path.DirectorySeparatorChar}",
                    FileManagerDirectory_Id = null,
                    User_Id = sysUser.Id,
                    CreatedUser = sysUser,
                    Identifier = null,
                    Created_On = DateTime.Now,
                    Updated_On = DateTime.Now
                }
            };
        }
    }
}
