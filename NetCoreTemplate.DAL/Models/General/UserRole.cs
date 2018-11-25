namespace NetCoreTemplate.DAL.Models.General
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using NetCoreTemplate.DAL.Models.Base;

    public class UserRole : TrackableEntity
    {
        [Key, Column(Order = 0)]
        public int User_Id { get; set; }

        [ForeignKey(nameof(User_Id))]
        public virtual User User { get; set; }

        [Key, Column(Order = 1)]
        public int Role_Id { get; set; }

        [ForeignKey(nameof(Role_Id))]
        public virtual Role Role { get; set; }
    }
}
