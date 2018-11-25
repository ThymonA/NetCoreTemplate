namespace NetCoreTemplate.FileManager
{
    using System;
    using System.IO;

    using NetCoreTemplate.FileManager.Interfaces;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;

    public sealed class FileWriter : IFileWriter
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

        /// <summary>
        /// The encryption key
        /// </summary>
        private string BasePath => Configuration["FileManager:BasePath"];

        /// <summary>
        /// The file security
        /// </summary>
        private readonly IFileEncrypter fileEncrypter;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FileWriter"/> class.
        /// </summary>
        /// <param name="fileEncrypter">The file security.</param>
        /// <param name="configuration">.NET Core configuration</param>
        public FileWriter(IFileEncrypter fileEncrypter, IConfiguration configuration)
        {
            this.fileEncrypter = fileEncrypter;
            Configuration = configuration;
        }

        /// <inheritdoc />
        /// <summary>
        /// Writes the file, encrypts the file afterwards
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="file">The file.</param>
        public void WriteFile(string directory, string filename, IFormFile file)
        {
            try
            {
                var path = directory + DS + filename;
                var fileBytes = GetFileBytes(file);
                File.WriteAllBytes(Path.Combine(BasePath, path).Replace("\\", $"{DS}"), fileBytes);
                fileEncrypter.Encrypt(Path.Combine(BasePath, path).Replace("\\", $"{DS}"));
            }
            catch (Exception ex)
            {
                DeleteFile(directory + filename);
                throw ex.GetInnermostException();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Writes the file, encrypts the file afterwards
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="fileContent">The file content.</param>
        public void WriteFile(string directory, string filename, string fileContent)
        {
            try
            {
                var path = directory + DS + filename;
                File.WriteAllText(Path.Combine(BasePath, path).Replace("\\", $"{DS}"), fileContent);
                fileEncrypter.Encrypt(Path.Combine(BasePath, path).Replace("\\", $"{DS}"));
            }
            catch (Exception ex)
            {
                DeleteFile(directory + filename);
                throw ex.GetInnermostException();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Writes the file, encrypts the file afterwards
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="fileContent">The file content.</param>
        public void WriteFile(string directory, string filename, byte[] fileContent)
        {
            try
            {
                var path = directory + DS + filename;
                File.WriteAllBytes(Path.Combine(BasePath, path).Replace("\\", $"{DS}"), fileContent);
                fileEncrypter.Encrypt(Path.Combine(BasePath, path).Replace("\\", $"{DS}"));
            }
            catch (Exception ex)
            {
                DeleteFile(directory + filename);
                throw ex.GetInnermostException();
            }
        }

        /// <summary>
        /// Gets the file bytes.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>The file bytes</returns>
        private static byte[] GetFileBytes(IFormFile file)
        {
            var fileBytes = new byte[file.OpenReadStream().Length];
            file.OpenReadStream().Read(fileBytes, 0, fileBytes.Length);
            return fileBytes;
        }

        /// <inheritdoc />
        /// <summary>
        /// Reads the file.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Return memory stream</returns>
        public MemoryStream ReadFile(string path)
        {
            return fileEncrypter.Decrypt(Path.Combine(BasePath, path).Replace("\\", $"{DS}"));
        }

        /// <inheritdoc />
        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="path">The path.</param>
        public void DeleteFile(string path)
        {
            try
            {
                if (!path.StartsWith(BasePath, StringComparison.OrdinalIgnoreCase))
                {
                    path = BasePath + DS + path;
                }

                File.Delete(path.Replace("\\", $"{DS}"));
            }
            catch (Exception ex)
            {
                throw ex.GetInnermostException();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Creates the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        public void CreateDirectory(string path)
        {
            try
            {
                if (!path.Contains(BasePath, StringComparison.InvariantCultureIgnoreCase))
                {
                    path = BasePath + path.Substring(1);
                }

                Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                throw ex.GetInnermostException();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="path">The path.</param>
        public void DeleteDirectory(string path)
        {
            try
            {
                Directory.Delete(Path.Combine(BasePath, path).Replace("\\", $"{DS}"), true);
            }
            catch (Exception ex)
            {
                throw ex.GetInnermostException();
            }
        }
    }
}
