namespace NetCoreTemplate.FileManager.Interfaces
{
    using System.Collections.Generic;
    using System.IO;

    using NetCoreTemplate.DAL.Models.Enums;
    using NetCoreTemplate.DAL.Models.FileManager;

    using Microsoft.AspNetCore.Http;

    public interface IFileManager
    {
        /// <summary>
        /// Determines whether the file is unique.
        /// </summary>
        /// <param name="directoryId">The directory identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="extension">The extension.</param>
        /// <returns>
        ///   <c>true</c> if the file name and extension are unique; otherwise, <c>false</c>.
        /// </returns>
        bool IsFileUnique(int directoryId, string name, string extension);

        /// <summary>
        /// Return existing file
        /// </summary>
        /// <param name="directoryId">The directory identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="extension">The extension.</param>
        /// <returns>
        ///   <c>FileManagerFile</c> if the file name and extension existing; otherwise, <c>null</c>.
        /// </returns>
        FileManagerFile GetExistingFile(int directoryId, string name, string extension);

        /// <summary>
        /// Determines whether the directory is unique.
        /// </summary>
        /// <param name="parentDirectoryId">The parent directory identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if the directory name is unique; otherwise, <c>false</c>.
        /// </returns>
        bool IsDirectoryUnique(int parentDirectoryId, string name);

        /// <summary>
        /// Gets the file count in the specified directory.
        /// </summary>
        /// <param name="directoryId">The directory identifier.</param>
        /// <returns></returns>
        int GetFileCount(int directoryId);

        /// <summary>
        /// Gets the directory count in the specified directory.
        /// </summary>
        /// <param name="parentDirectoryId">The parent directory identifier.</param>
        /// <returns></returns>
        int GetDirectoryCount(int parentDirectoryId);

        /// <summary>
        /// Gets the root directory identifier.
        /// </summary>
        /// <returns></returns>
        int GetRootDirectoryId();

        /// <summary>
        /// Gets the directory.
        /// </summary>
        /// <param name="directoryId">The directory identifier.</param>
        /// <returns></returns>
        FileManagerDirectory GetDirectory(int directoryId);

        FileManagerDirectory GetDirectoryByPath(string path);

        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <returns></returns>
        FileManagerFile GetFile(int fileId);

        /// <summary>
        /// Downloads the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        MemoryStream DownloadFile(FileManagerFile file);

        /// <summary>
        /// Gets a list of files.
        /// </summary>
        /// <param name="directoryId">The directory identifier.</param>
        /// <returns></returns>
        List<FileManagerFile> GetFiles(int directoryId);

        /// <summary>
        /// Gets a list of directories.
        /// </summary>
        /// <param name="parentDirectoryId">The parent directory identifier.</param>
        /// <returns></returns>
        List<FileManagerDirectory> GetDirectories(int parentDirectoryId);

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        void DeleteFile(int fileId);

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="directoryId">The directory identifier.</param>
        void DeleteDirectory(int directoryId);

        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="mimeType">Type of the MIME.</param>
        /// <param name="file">The file.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="identifier">The identifier</param>
        /// <returns></returns>
        FileManagerFile CreateFile(
            FileManagerDirectory directory,
            string mimeType,
            IFormFile file,
            int userId,
            int? identifier);

        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="mimeType">Type of the MIME.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="fileContent">The file content.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="identifier">The identifier</param>
        /// <returns></returns>
        FileManagerFile CreateFile(
            FileManagerDirectory directory,
            string mimeType,
            string fileName,
            string fileContent,
            int userId,
            int? identifier);

        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="mimeType">Type of the MIME.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="fileContent">The file content.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="identifier">The identifier.</param>
        /// <returns></returns>
        FileManagerFile CreateFile(
            FileManagerDirectory directory,
            string mimeType,
            string fileName,
            byte[] fileContent,
            int userId,
            int? identifier);

        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="mimeType">Type of the MIME.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="fileContent">The file content.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="identifier">The identifier.</param>
        /// <param name="overrideFile">If exists, will be overridden</param>
        /// <returns></returns>
        FileManagerFile CreateFile(
            FileManagerDirectory directory,
            string mimeType,
            string fileName,
            byte[] fileContent,
            int userId,
            int? identifier,
            bool overrideFile);

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="parentDirectory">The parent directory.</param>
        /// <param name="type">The directory type.</param>
        /// <param name="userId">The created user id.</param>
        /// <returns></returns>
        FileManagerDirectory CreateDirectory(
            FileManagerDirectory parentDirectory,
            FileManagerDirectoryType type,
            int userId);

        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="parentDirectory">The parent directory.</param>
        /// <param name="type">The directory type.</param>
        /// <param name="userId">The created user id.</param>
        /// <param name="identifier">The identifier.</param>
        /// <returns></returns>
        FileManagerDirectory CreateDirectory(
            FileManagerDirectory parentDirectory,
            FileManagerDirectoryType type,
            int userId,
            int? identifier);

        /// <summary>
        /// Gets the contact directory.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        List<FileManagerFile> GetContactFiles(int contactId);

        /// <summary>
        /// Edits the name of the file.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="T:System.NotImplementedException"></exception>
        FileManagerFile EditFileName(int fileId, string name);

        /// <summary>
        /// Edits the name of the directory.
        /// </summary>
        /// <param name="directoryId">The directory identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="T:System.NotImplementedException"></exception>
        FileManagerDirectory EditDirectoryName(int directoryId, string name);

        /// <summary>
        /// Creates the directory recursively by directoryType and id.
        /// </summary>
        /// <param name="directoryType">Type of the directory.</param>
        /// <param name="userId">The current user.</param>
        /// <param name="identifier">The identifier.</param>
        /// <returns>The created directory</returns>
        FileManagerDirectory GetOrCreateDirectoryByDirectoryType(
            FileManagerDirectoryType directoryType,
            int userId,
            int? identifier);

        /// <summary>
        /// Creates the directory recursively by directoryType and id.
        /// </summary>
        /// <param name="directoryType">Type of the directory.</param>
        /// <param name="contactId">The identifier.</param>
        /// <returns>The created directory</returns>
        string GetDirectoryPathByDirectoryType(FileManagerDirectoryType directoryType, int contactId);
    }
}
