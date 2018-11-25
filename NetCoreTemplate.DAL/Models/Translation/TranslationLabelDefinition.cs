namespace NetCoreTemplate.DAL.Models.Translation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using NetCoreTemplate.DAL.Attributes;
    using NetCoreTemplate.DAL.Models.Base;
    using NetCoreTemplate.DAL.Models.Interfaces;

    [Serializable]
    public class TranslationLabelDefinition : TrackableEntity, IKeyModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        [Index("IX_Module_Type_Key_Unique", 0, IsClustered = false, IsUnique = true)]
        public string Module { get; set; }

        [Required]
        [MaxLength(128)]
        [Index("IX_Module_Type_Key_Unique", 1, IsClustered = false, IsUnique = true)]
        public string Type { get; set; }

        [Required]
        [MaxLength(128)]
        [Index("IX_Module_Type_Key_Unique", 2, IsClustered = false, IsUnique = true)]
        public string Key { get; set; }

        public virtual List<TranslationLabel> TranslationLabels { get; set; }
    }
}
