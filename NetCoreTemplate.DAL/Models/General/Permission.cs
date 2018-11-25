namespace NetCoreTemplate.DAL.Models.General
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using NetCoreTemplate.DAL.Models.Base;
    using NetCoreTemplate.DAL.Models.Interfaces;

    public class Permission : EntityTranslation, IKeyModel
    {
        [Key]
        public int Id { get; set; }

        public string Action { get; set; }

        public virtual List<RolePermission> RolePermissions { get; set; }
    }
}
