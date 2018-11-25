namespace NetCoreTemplate.DAL.Models.FileManager
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using NetCoreTemplate.DAL.Models.Base;
    using NetCoreTemplate.DAL.Models.Contact;
    using NetCoreTemplate.DAL.Models.Enums;
    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.DAL.Models.Interfaces;

    public class FileManagerFile : TrackableEntity, IKeyModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public string Guid { get; set; }

        public string Extension { get; set; }

        public string MimeType { get; set; }

        public long Size { get; set; }

        public int FileManagerDirectory_Id { get; set; }

        [ForeignKey(nameof(FileManagerDirectory_Id))]
        public virtual FileManagerDirectory FileManagerDirectory { get; set; }

        public int CreatedBy_User_Id { get; set; }

        [ForeignKey(nameof(CreatedBy_User_Id))]
        public virtual User User { get; set; }

        public int? Identifier { get; set; }

        public DateTimeOffset Created_On { get; set; }

        public DateTimeOffset? Updated_On { get; set; }
    }
}
