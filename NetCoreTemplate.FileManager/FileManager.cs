namespace NetCoreTemplate.FileManager
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using NetCoreTemplate.DAL.Models.Enums;
    using NetCoreTemplate.DAL.Models.FileManager;
    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.DAL.PersistenceLayer;
    using NetCoreTemplate.FileManager.Interfaces;
    using NetCoreTemplate.FileManager.Models;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.Interfaces.PersistenceLayer;
    using NetCoreTemplate.SharedKernel.ServiceContainer;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;

    using NetCoreTemplate.DAL.Models.Contact;

    public sealed class FileManager : IFileManager
    {
        private static char DS => Path.DirectorySeparatorChar;

        private static IConfiguration _configuration { get; set; }

        private IConfiguration Configuration
        {
            get
            {
                if (_configuration.IsNullOrDefault())
                {
                    _configuration = ServiceContainer.Current.GetService<IConfiguration>();
                }

                return _configuration;
            }

            set => _configuration = value;
        }

        private static readonly FileManagerDirectory RootDirectory = GetRootDirectory();

        /// <summary>
        /// The encryption key
        /// </summary>
        private string BasePath => Configuration["FileManager:BasePath"];

        /// <summary>
        /// The file writer
        /// </summary>
        private readonly IFileWriter fileWriter;
        
        /// <summary>
        /// The persistence
        /// </summary>
        private readonly IPersistenceLayer persistence;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FileManager"/> class.
        /// </summary>
        /// <param name="persistence">The persistence.</param>
        /// <param name="fileWriter">The file writer.</param>
        /// <param name="configuration">.NET Core configuration</param>
        public FileManager(IPersistenceLayer persistence, IFileWriter fileWriter, IConfiguration configuration)
        {
            this.persistence = persistence;
            this.fileWriter = fileWriter;
            Configuration = configuration;
        }
        
        /// <summary>
        /// Gets the root directory.
        /// </summary>
        /// <returns></returns>
        private static FileManagerDirectory GetRootDirectory()
        {
            using (var persistence = new PersistenceLayer())
            {
                return persistence.Get<FileManagerDirectory>()
                    .First(fileManagerDirectory => fileManagerDirectory.FileManagerDirectory_Id == null);
            }
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Determines whether the file is unique.
        /// </summary>
        /// <param name="directoryId">The directory identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="extension">The extension.</param>
        /// <returns>
        ///   <c>true</c> if the file name and extension are unique; otherwise, <c>false</c>.
        /// </returns>
        public bool IsFileUnique(int directoryId, string name, string extension)
        {
            return !persistence.Get<FileManagerFile>()
                .Any(fileManagerFile => 
                    fileManagerFile.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && 
                        (fileManagerFile.Extension.Equals(name, StringComparison.OrdinalIgnoreCase) || 
                        fileManagerFile.Extension.Equals("." + extension, StringComparison.OrdinalIgnoreCase)) && 
                    fileManagerFile.FileManagerDirectory_Id == directoryId);
        }

        /// <inheritdoc />
        /// <summary>
        /// Return existing file
        /// </summary>
        /// <param name="directoryId">The directory identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="extension">The extension.</param>
        /// <returns>
        ///   <c>FileManagerFile</c> if the file name and extension existing; otherwise, <c>null</c>.
        /// </returns>
        public FileManagerFile GetExistingFile(int directoryId, string name, string extension)
        {
            return persistence.Get<FileManagerFile>()
                .FirstOrDefault(fileManagerFile =>
                    fileManagerFile.Name.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                        (fileManagerFile.Extension.Equals(name, StringComparison.OrdinalIgnoreCase) ||
                        fileManagerFile.Extension.Equals("." + extension, StringComparison.OrdinalIgnoreCase)) &&
                    fileManagerFile.FileManagerDirectory_Id == directoryId);
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Determines whether the directory is unique.
        /// </summary>
        /// <param name="parentDirectoryId">The parent directory identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if the directory name is unique; otherwise, <c>false</c>.
        /// </returns>
        public bool IsDirectoryUnique(int parentDirectoryId, string name)
        {
            return !persistence.Get<FileManagerDirectory>().Any(fileManagerDirectory => fileManagerDirectory.Name == name
                && fileManagerDirectory.FileManagerDirectory_Id == parentDirectoryId);
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Gets the file count in the specified directory.
        /// </summary>
        /// <param name="directoryId">The directory identifier.</param>
        /// <returns></returns>
        public int GetFileCount(int directoryId)
        {
            return GetFileQuery(directoryId).Count();
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Gets the directory count in the specified directory.
        /// </summary>
        /// <param name="parentDirectoryId">The parent directory identifier.</param>
        /// <returns></returns>
        public int GetDirectoryCount(int parentDirectoryId)
        {
            return GetDirectoryQuery(parentDirectoryId).Count();
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Gets the root directory identifier.
        /// </summary>
        /// <returns></returns>
        public int GetRootDirectoryId()
        {
            return RootDirectory?.Id ?? default(int);
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Gets the directory.
        /// </summary>
        /// <param name="directoryId">The directory identifier.</param>
        /// <returns></returns>
        public FileManagerDirectory GetDirectory(int directoryId)
        {
            return persistence.Get<FileManagerDirectory>().FirstOrDefault(fileManagerDirectory => fileManagerDirectory.Id == directoryId);
        }

        public FileManagerDirectory GetDirectoryByPath(string path)
        {
            return persistence.Get<FileManagerDirectory>()
                .FirstOrDefault(x => x.Location.Equals(path, StringComparison.OrdinalIgnoreCase));
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the file.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <returns></returns>
        public FileManagerFile GetFile(int fileId)
        {
            return persistence.Get<FileManagerFile>().FirstOrDefault(fileManagerFile => fileManagerFile.Id == fileId);
        }

        /// <inheritdoc />
        /// <summary>
        /// Downloads the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public MemoryStream DownloadFile(FileManagerFile file)
        {
            return fileWriter.ReadFile(file.Location + $"{DS}" + file.Guid);
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Gets a list of files.
        /// </summary>
        /// <param name="directoryId">The directory identifier.</param>
        /// <returns></returns>
        public List<FileManagerFile> GetFiles(int directoryId)
        {
            return GetFileQuery(directoryId).ToList();
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Gets a list of directories.
        /// </summary>
        /// <param name="parentDirectoryId">The parent directory identifier.</param>
        /// <returns></returns>
        public List<FileManagerDirectory> GetDirectories(int parentDirectoryId)
        {
            return GetDirectoryQuery(parentDirectoryId).ToList();
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        public void DeleteFile(int fileId)
        {
            var fileManagerFile = GetFile(fileId);

            fileWriter.DeleteFile(fileManagerFile.Location + DS + fileManagerFile.Guid);

            persistence.Delete(fileManagerFile);
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="directoryId">The directory identifier.</param>
        public void DeleteDirectory(int directoryId)
        {
            var fileManagerFiles = GetFiles(directoryId);

            DeleteFiles(fileManagerFiles);

            var fileManagerDirectories = GetDirectories(directoryId);

            DeleteDirectories(fileManagerDirectories);

            var fileManagerDirectory = GetDirectory(directoryId);

            fileWriter.DeleteDirectory(fileManagerDirectory.Location);

            persistence.Delete(fileManagerDirectory);
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates the file.
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="mimeType">Type of the MIME.</param>
        /// <param name="file">The file.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="identifier">The identifier</param>
        /// <returns></returns>
        public FileManagerFile CreateFile(
            FileManagerDirectory directory,
            string mimeType,
            IFormFile file,
            int userId,
            int? identifier)
        {
            var guid = Guid.NewGuid().ToString();
            var location = BasePath + directory.Location;
            var name = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            var user = FindUser(userId);

            if (user.IsNullOrDefault())
            {
                return null;
            }

            var fileManagerFile = new FileManagerFile
            {
                Name = name,
                Location = location,
                Guid = guid,
                Extension = extension,
                MimeType = mimeType,
                Size = file.Length,
                FileManagerDirectory_Id = directory.Id,
                CreatedBy_User_Id = user.Id,
                Identifier = identifier,
                User = user,
                Created_On = DateTimeOffset.UtcNow
            };

            fileWriter.CreateDirectory(location);
            fileWriter.WriteFile(location, guid, file);

            persistence.Add(fileManagerFile);

            return GetFile(fileManagerFile.Id);
        }

        /// <inheritdoc />
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
        public FileManagerFile CreateFile(
            FileManagerDirectory directory,
            string mimeType,
            string fileName,
            string fileContent,
            int userId,
            int? identifier)
        {
            var guid = Guid.NewGuid().ToString();
            var location = BasePath + directory.Location;
            var name = Path.GetFileNameWithoutExtension(fileName);
            var extension = Path.GetExtension(fileName);
            var user = FindUser(userId);

            if (user.IsNullOrDefault())
            {
                return null;
            }

            var fileManagerFile = new FileManagerFile
            {
                Name = name,
                Location = location,
                Guid = guid,
                Extension = extension,
                MimeType = mimeType,
                Size = 0,
                FileManagerDirectory_Id = directory.Id,
                CreatedBy_User_Id = user.Id,
                Identifier = identifier,
                User = user,
                Created_On = DateTimeOffset.UtcNow
            };

            fileWriter.CreateDirectory(location);
            fileWriter.WriteFile(location, guid, fileContent);

            persistence.Add(fileManagerFile);

            return GetFile(fileManagerFile.Id);
        }

        /// <inheritdoc />
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
        public FileManagerFile CreateFile(
            FileManagerDirectory directory,
            string mimeType,
            string fileName,
            byte[] fileContent,
            int userId,
            int? identifier)
        {
            return CreateFile(
                directory,
                mimeType,
                fileName,
                fileContent,
                userId,
                identifier,
                false);
        }

        /// <inheritdoc />
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
        public FileManagerFile CreateFile(
            FileManagerDirectory directory,
            string mimeType,
            string fileName,
            byte[] fileContent,
            int userId,
            int? identifier,
            bool overrideFile)
        {
            var guid = Guid.NewGuid().ToString();
            var location = BasePath + directory.Location;
            var name = Path.GetFileNameWithoutExtension(fileName);
            var extension = Path.GetExtension(fileName);
            var user = FindUser(userId);

            if (user.IsNullOrDefault())
            {
                return null;
            }

            var uniqueFile = IsFileUnique(directory.Id, name, extension);

            FileManagerFile fileManagerFile;

            if (uniqueFile)
            {
                fileManagerFile = new FileManagerFile
                {
                    Name = name,
                    Location = location,
                    Guid = guid,
                    Extension = extension,
                    MimeType = mimeType,
                    Size = 0,
                    FileManagerDirectory_Id = directory.Id,
                    CreatedBy_User_Id = user.Id,
                    Identifier = identifier,
                    User = user,
                    Created_On = DateTimeOffset.UtcNow
                };

                fileWriter.CreateDirectory(location);
                fileWriter.WriteFile(location, guid, fileContent);

                persistence.Add(fileManagerFile);
            }
            else
            {
                fileManagerFile = GetExistingFile(directory.Id, name, extension);

                fileWriter.DeleteFile(location + $"{DS}" + fileManagerFile.Guid);

                fileManagerFile.Guid = guid;
                fileManagerFile.MimeType = mimeType;
                fileManagerFile.CreatedBy_User_Id = user.Id;
                fileManagerFile.Identifier = identifier;
                fileManagerFile.User = user;
                fileManagerFile.Created_On = DateTimeOffset.UtcNow;

                fileWriter.CreateDirectory(location);
                fileWriter.WriteFile(location, guid, fileContent);

                persistence.AddOrUpdate(fileManagerFile);
            }

            return GetFile(fileManagerFile.Id);
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="parentDirectory">The parent directory.</param>
        /// <param name="type">The directory type.</param>
        /// <param name="userId">The created user id.</param>
        /// <returns></returns>
        public FileManagerDirectory CreateDirectory(
            FileManagerDirectory parentDirectory,
            FileManagerDirectoryType type,
            int userId)
        {
            var path = TypeToPath(parentDirectory, type);
            var createdUser = FindUser(userId);
            var name = TypeToName(type);

            if (createdUser.IsNullOrDefault())
            {
                return null;
            }

            var fileManagerDirectory = new FileManagerDirectory
            {
                Name = name,
                Location = path,
                FileManagerDirectory_Id = parentDirectory.Id,
                Parent = parentDirectory,
                User_Id = createdUser.Id,
                CreatedUser = createdUser,
                Identifier = null,
                Created_On = DateTime.Now,
                Updated_On = DateTime.Now,
                Type = type
            };

            fileWriter.CreateDirectory(path);
            persistence.Add(fileManagerDirectory);

            return GetDirectory(fileManagerDirectory.Id);
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="parentDirectory">The parent directory.</param>
        /// <param name="type">The directory type.</param>
        /// <param name="userId">The created user id.</param>
        /// <param name="identifier">The identifier.</param>
        /// <returns></returns>
        public FileManagerDirectory CreateDirectory(
            FileManagerDirectory parentDirectory,
            FileManagerDirectoryType type,
            int userId,
            int? identifier)
        {
            var path = TypeToPath(parentDirectory, type);
            var createdUser = FindUser(userId);
            var name = TypeToName(type);

            if (createdUser.IsNullOrDefault())
            {
                return null;
            }

            if (type != FileManagerDirectoryType.Public && !identifier.HasValue)
            {
                return null;
            }

            if (identifier.HasValue && type != FileManagerDirectoryType.Public)
            {
                path += $"{DS}" + identifier;
            }

            switch (type)
            {
                case FileManagerDirectoryType.Privately:
                    var user = FindUser(identifier ?? default(int));
                    if (user.IsNullOrDefault()) { return null; }

                    name = user.Id.ToString();

                    break;
                case FileManagerDirectoryType.Shared:
                    var contact = FindContact(identifier ?? default(int));
                    if (contact.IsNullOrDefault()) { return null; }

                    name = contact.Id.ToString();

                    break;
            }

            var fileManagerDirectory = new FileManagerDirectory
            {
                Name = name,
                Location = path,
                FileManagerDirectory_Id = parentDirectory.Id,
                Parent = parentDirectory,
                User_Id = createdUser.Id,
                CreatedUser = createdUser,
                Identifier = identifier,
                Created_On = DateTime.Now,
                Updated_On = DateTime.Now,
                Type = type
            };

            fileWriter.CreateDirectory(path);
            persistence.Add(fileManagerDirectory);

            return GetDirectory(fileManagerDirectory.Id);
        }

        /// <summary>
        /// Gets the contact directory.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public List<FileManagerFile> GetContactFiles(int contactId)
        {
            return persistence.Get<FileManagerFile>()
                .Where(file => file.FileManagerDirectory.Type == FileManagerDirectoryType.Shared && file.Identifier == contactId)
                .ToList();
        }

        /// <inheritdoc />
        /// <summary>
        /// Edits the name of the file.
        /// </summary>
        /// <param name="fileId">The file identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="T:System.NotImplementedException"></exception>
        public FileManagerFile EditFileName(int fileId, string name)
        {
            throw new NotImplementedException();
        }
        
        /// <inheritdoc />
        /// <summary>
        /// Edits the name of the directory.
        /// </summary>
        /// <param name="directoryId">The directory identifier.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="T:System.NotImplementedException"></exception>
        public FileManagerDirectory EditDirectoryName(int directoryId, string name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates the directory recursively by directoryType and id.
        /// </summary>
        /// <param name="directoryType">Type of the directory.</param>
        /// <param name="userId">The current user.</param>
        /// <param name="identifier">The identifier.</param>
        /// <returns>The created directory</returns>
        public FileManagerDirectory GetOrCreateDirectoryByDirectoryType(FileManagerDirectoryType directoryType, int userId, int? identifier)
        {
            var directoryDetails = GetDetailsByDirectoryType(directoryType, identifier);
            var path = TypeToPath(RootDirectory, directoryType);

            if (identifier.HasValue && directoryType != FileManagerDirectoryType.Public)
            {
                path += $"{DS}" + identifier;
            }

            var contactDirectory = persistence.Get<FileManagerDirectory>()
                .FirstOrDefault(x => x.Location.Equals(path, StringComparison.InvariantCultureIgnoreCase));

            if (contactDirectory.IsNullOrDefault())
            {
                var directoryTypeDirectory = FindDirectory(RootDirectory, directoryDetails.Type) ?? CreateDirectory(RootDirectory, directoryType, userId);
                contactDirectory = CreateDirectory(directoryTypeDirectory, directoryDetails.Type, userId, directoryDetails.Identifier);
            }

            contactDirectory.Name = TypeToName(directoryDetails.Type);

            return contactDirectory;
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates the directory recursively by directoryType and id.
        /// </summary>
        /// <param name="directoryType">Type of the directory.</param>
        /// <param name="contactId">The identifier.</param>
        /// <returns>The created directory</returns>
        public string GetDirectoryPathByDirectoryType(FileManagerDirectoryType directoryType, int contactId)
        {
            return RootDirectory.Location + directoryType.GetDescription() + $"{DS}" + contactId;
        }

        /// <summary>
        /// Gets the file query.
        /// </summary>
        /// <param name="directoryId">The directory identifier.</param>
        /// <returns></returns>
        private IQueryable<FileManagerFile> GetFileQuery(int directoryId)
        {
            return persistence.Get<FileManagerFile>().Where(fmf => fmf.FileManagerDirectory_Id == directoryId);
        }

        /// <summary>
        /// Gets the directory query.
        /// </summary>
        /// <param name="parentDirectoryId">The parent directory identifier.</param>
        /// <returns></returns>
        private IQueryable<FileManagerDirectory> GetDirectoryQuery(int parentDirectoryId)
        {
            return persistence.Get<FileManagerDirectory>().Where(fmd => fmd.FileManagerDirectory_Id == parentDirectoryId);
        }
        
        /// <summary>
        /// Deletes multiple files.
        /// </summary>
        /// <param name="files">The files.</param>
        private void DeleteFiles(List<FileManagerFile> files)
        {
            files.ForEach(file => DeleteFile(file.Id));
        }
        
        /// <summary>
        /// Deletes multiple directories.
        /// </summary>
        /// <param name="directories">The directories.</param>
        private void DeleteDirectories(List<FileManagerDirectory> directories)
        {
            directories.ForEach(directory => DeleteDirectory(directory.Id));
        }
        
        /// <summary>
        /// Gets the details of the directory by directoryType and id.
        /// </summary>
        /// <param name="directoryType">Type of the directory.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private DirectorySetting GetDetailsByDirectoryType(FileManagerDirectoryType directoryType, int? id)
        {
            if (directoryType != FileManagerDirectoryType.Public && !id.HasValue)
            {
                return null;
            }

            switch (directoryType)
            {
                case FileManagerDirectoryType.Privately:
                    var user = persistence.Get<User>().FirstOrDefault(x => x.Id == id);
                    return !user.IsNullOrDefault() ? new DirectorySetting(user.Id, directoryType) : null;
                case FileManagerDirectoryType.Shared:
                    var contact = persistence.Get<Contact>().FirstOrDefault(x => x.Id == id);
                    return !contact.IsNullOrDefault() ? new DirectorySetting(contact.Id, directoryType) : null;
                case FileManagerDirectoryType.Public:
                    return new DirectorySetting(directoryType);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Finds the directory by name.
        /// </summary>
        /// <param name="parentDirectory">The parent directory.</param>
        /// <param name="type">The directory type.</param>
        /// <returns>The found directory or null</returns>
        private FileManagerDirectory FindDirectory(FileManagerDirectory parentDirectory, FileManagerDirectoryType type)
        {
            return persistence.Get<FileManagerDirectory>()
                .FirstOrDefault(fileManagerDirectory => fileManagerDirectory.Name.Equals(TypeToName(type), StringComparison.OrdinalIgnoreCase) && fileManagerDirectory.FileManagerDirectory_Id == parentDirectory.Id);
        }

        private Contact FindContact(int contactId)
        {
            return persistence.Get<Contact>()
                .FirstOrDefault(x => x.Id == contactId);
        }

        private User FindUser(int userId)
        {
            return persistence.Get<User>()
                .FirstOrDefault(x => x.Id == userId);
        }

        private string TypeToPath(FileManagerDirectory parentDirectory, FileManagerDirectoryType type)
        {
            var path = parentDirectory.Location + DS + TypeToName(type);
            return path.Replace($"{DS}{DS}", $"{DS}");
        }

        private string TypeToName(FileManagerDirectoryType type)
        {
            switch (type)
            {
                case FileManagerDirectoryType.Privately:
                    return "Users";
                case FileManagerDirectoryType.Shared:
                    return "Contacts";
                case FileManagerDirectoryType.Public:
                    return "Public";
                default:
                    return "Others";
            }
        }
    }
}
