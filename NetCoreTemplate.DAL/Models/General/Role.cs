namespace NetCoreTemplate.DAL.Models.General
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using NetCoreTemplate.DAL.Models.Base;
    using NetCoreTemplate.DAL.Models.Interfaces;

    public class Role : TrackableEntity, IKeyModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public bool Active { get; set; }

        public virtual List<RolePermission> RolePermissions { get; set; }
    }
}
