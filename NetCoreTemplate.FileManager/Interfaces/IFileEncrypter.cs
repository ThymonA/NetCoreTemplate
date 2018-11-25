namespace NetCoreTemplate.FileManager.Interfaces
{
    using System.IO;

    public interface IFileEncrypter
    {
        /// <summary>
        /// Encrypts the specified file.
        /// </summary>
        /// <param name="path">The location of the file.</param>
        void Encrypt(string path);

        /// <summary>
        /// Decrypts the specified file.
        /// </summary>
        /// <param name="path">The location of the file.</param>
        /// <returns>Return memory stream</returns>
        MemoryStream Decrypt(string path);
    }
}
