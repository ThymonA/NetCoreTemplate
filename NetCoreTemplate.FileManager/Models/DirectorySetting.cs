namespace NetCoreTemplate.FileManager.Models
{
    using NetCoreTemplate.DAL.Models.Enums;

    public sealed class DirectorySetting
    {
        public int? Identifier { get; set; }

        public FileManagerDirectoryType Type { get; set; }

        public DirectorySetting(FileManagerDirectoryType type)
        {
            Type = type;
        }

        public DirectorySetting(int contactId, FileManagerDirectoryType type)
        {
            Type = type;
            Identifier = contactId;
        }
    }
}
