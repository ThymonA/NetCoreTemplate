namespace NetCoreTemplate.DAL.Models.Enums
{
    using System.ComponentModel;

    public enum FileManagerDirectoryType
    {
        [Description("Privately")]
        Privately = 0,

        [Description("Shared")]
        Shared = 1,

        [Description("Public")]
        Public = 2,

        [Description("Others")]
        Others = 3,
    }
}
