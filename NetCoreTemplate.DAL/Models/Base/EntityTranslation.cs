namespace NetCoreTemplate.DAL.Models.Base
{
    using System.ComponentModel.DataAnnotations.Schema;

    using NetCoreTemplate.DAL.Initializers.Translation;
    using NetCoreTemplate.DAL.Models.Translation;

    public class EntityTranslation : TrackableEntity
    {
        public int EntityLabelDefinition_Id { get; set; }

        [ForeignKey(nameof(EntityLabelDefinition_Id))]
        public virtual EntityLabelDefinition EntityLabelDefinition { get; set; }

        [NotMapped]
        public virtual Translation Translation { get; set; }
    }
}
