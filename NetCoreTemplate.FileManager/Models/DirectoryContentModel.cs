namespace NetCoreTemplate.FileManager.Models
{
    using System.Collections.Generic;

    using NetCoreTemplate.DAL.Models.FileManager;

    public sealed class DirectoryContentModel
    {
        public FileManagerDirectory FileManagerDirectory { get; set; }

        public List<FileManagerDirectory> FileManagerSubDirectories { get; set; }

        public List<FileManagerFile> FileManagerFile { get; set; }

        public DirectoryContentModel(
            FileManagerDirectory fileManagerDirectory,
            List<FileManagerDirectory> fileManagerSubDirectories,
            List<FileManagerFile> fileManagerFile)
        {
            FileManagerDirectory = fileManagerDirectory;
            FileManagerSubDirectories = fileManagerSubDirectories;
            FileManagerFile = fileManagerFile;
        }

        public DirectoryContentModel()
        {
        }
    }
}
