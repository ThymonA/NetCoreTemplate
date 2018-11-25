namespace NetCoreTemplate.DAL.Models.General
{
    using System;
    using System.Collections.Generic;

    using NetCoreTemplate.DAL.Models.Base;
    using NetCoreTemplate.DAL.Models.Interfaces;

    using System.ComponentModel.DataAnnotations;

    public class User : TrackableEntity, IKeyModel
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string ResetToken { get; set; }

        public DateTime? ResetTokenDate { get; set; }

        public bool Active { get; set; }

        public virtual List<UserRole> UserRoles { get; set; }
    }
}
