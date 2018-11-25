namespace NetCoreTemplate.DAL.Models.Interfaces
{
    using System.ComponentModel.DataAnnotations;

    public interface IKeyModel
    {
        [Key]
        int Id { get; set; }
    }
}
