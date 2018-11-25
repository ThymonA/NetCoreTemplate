namespace NetCoreTemplate.FileManager.Interfaces
{
    using System.IO;

    using Microsoft.AspNetCore.Http;

    public interface IFileWriter
    {
        /// <summary>
        /// Reads the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Return memory stream</returns>
        MemoryStream ReadFile(string path);

        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="file">The file.</param>
        void WriteFile(string directory, string filename, IFormFile file);

        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="fileContent">The file content.</param>
        void WriteFile(string directory, string filename, string fileContent);

        /// <summary>
        /// Writes the file.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="fileContent">The file content.</param>
        void WriteFile(string directory, string filename, byte[] fileContent);

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        void DeleteFile(string path);

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        void CreateDirectory(string path);

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        void DeleteDirectory(string path);
    }
}
