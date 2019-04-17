namespace NetCoreTemplate.DAL.Models.Translation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using NetCoreTemplate.DAL.Models.Base;
    using NetCoreTemplate.DAL.Models.Interfaces;

    [Serializable]
    public class EntityLabelDefinition : TrackableEntity, IKeyModel
    {
        [Key]
        public int Id { get; set; }

        public string Key { get; set; }

        public virtual List<EntityLabel> EntityLabels { get; set; }
    }
}
