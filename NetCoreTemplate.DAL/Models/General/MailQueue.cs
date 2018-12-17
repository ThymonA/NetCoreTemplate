namespace NetCoreTemplate.DAL.Models.General
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using NetCoreTemplate.DAL.Models.Base;
    using NetCoreTemplate.DAL.Models.Interfaces;

    public class MailQueue : TrackableEntity, IKeyModel
    {
        [Key]
        public int Id { get; set; }

        public string To { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public DateTime AddedOn { get; set; }

        public DateTime? SentOn { get; set; }

        public int NumberOfTimesFailed { get; set; }
    }
}
