namespace NetCoreTemplate.DAL.Models.General
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using NetCoreTemplate.DAL.Models.Base;

    public class RolePermission : TrackableEntity
    {
        [Key, Column(Order = 0)]
        public int Role_Id { get; set; }

        [ForeignKey(nameof(Role_Id))]
        public virtual Role Role { get; set; }

        [Key, Column(Order = 0)]
        public int Permission_Id { get; set; }

        [ForeignKey(nameof(Permission_Id))]
        public virtual Permission Permission { get; set; }
    }
}
