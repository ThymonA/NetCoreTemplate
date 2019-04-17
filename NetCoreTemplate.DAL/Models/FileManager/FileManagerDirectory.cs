namespace NetCoreTemplate.DAL.Models.FileManager
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using NetCoreTemplate.DAL.Models.Base;
    using NetCoreTemplate.DAL.Models.Enums;
    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.DAL.Models.Interfaces;

    public class FileManagerDirectory : TrackableEntity, IKeyModel
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public int? FileManagerDirectory_Id { get; set; }

        [ForeignKey(nameof(FileManagerDirectory_Id))]
        public virtual FileManagerDirectory Parent { get; set; }

        public int User_Id { get; set; }

        [ForeignKey(nameof(User_Id))]
        public virtual User CreatedUser { get; set; }

        public int? Identifier { get; set; }

        public DateTime Created_On { get; set; }

        public DateTime? Updated_On { get; set; }

        public FileManagerDirectoryType Type { get; set; }

        public virtual List<FileManagerDirectory> FileManagerDirectories { get; set; }
    }
}
