namespace NetCoreTemplate.DAL.Models.Translation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using NetCoreTemplate.DAL.Models.Base;
    using NetCoreTemplate.DAL.Models.General;

    [Serializable]
    public class EntityLabel : TrackableEntity
    {
        [Key, Column(Order = 0)]
        public int EntityLabelDefinition_Id { get; set; }

        [ForeignKey(nameof(EntityLabelDefinition_Id))]
        public virtual EntityLabelDefinition EntityLabelDefinition { get; set; }

        [Key, Column(Order = 1)]
        public int Language_Id { get; set; }

        [ForeignKey(nameof(Language_Id))]
        public virtual Language Language { get; set; }

        public string Label { get; set; }
    }
}
