namespace NetCoreTemplate.DAL.Initializers.General
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using Microsoft.EntityFrameworkCore;

    using NetCoreTemplate.DAL.Models.General;

    public sealed class PermissionInitializer : EntityWithTranslationInitializer<Permission>
    {
        public PermissionInitializer(DatabaseContext context)
            : base(context)
        {
        }

        public override Expression<Func<DatabaseContext, DbSet<Permission>>> Expression => x => x.Permission;

        public override Expression<Func<Permission, int>> KeyExpression => x => x.Id;

        public override List<Permission> SeedMetaData()
        {
            return new List<Permission>
            {
            };
        }
    }
}
