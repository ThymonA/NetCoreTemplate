namespace NetCoreTemplate.DAL.Models.Contact
{
    using System.ComponentModel.DataAnnotations;

    using NetCoreTemplate.DAL.Models.Base;
    using NetCoreTemplate.DAL.Models.Interfaces;

    public class Contact : TrackableEntity, IKeyModel
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Street { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string Company { get; set; }

        public string CoCNumber { get; set; }

        public string TaxNumber { get; set; }
    }
}
