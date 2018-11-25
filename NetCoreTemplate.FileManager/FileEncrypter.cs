namespace NetCoreTemplate.FileManager
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    using NetCoreTemplate.FileManager.Interfaces;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;

    using Microsoft.Extensions.Configuration;
    using Microsoft.IO;

    public sealed class FileEncrypter : IFileEncrypter
    {
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
        private string EncryptionKey => Configuration["FileManager:EncryptionKey"];

        /// <summary>
        /// Initializes a new instance of the <see cref="FileEncrypter"/> class.
        /// </summary>
        /// <param name="configuration">
        /// .NET Configuration
        /// </param>
        public FileEncrypter(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <inheritdoc />
        /// <summary>
        /// Encrypts the uploaded file into a new .encrypted, then replaces the original file with the encrypted file.
        /// </summary>
        /// <param name="path">The location of the file.</param>
        public void Encrypt(string path)
        {
            var encryptedFilePath = path + ".encrypted";

            try
            {
                using (var in_stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (var out_stream = new FileStream(encryptedFilePath, FileMode.Create, FileAccess.Write))
                    {
                        CryptStream(EncryptionKey, in_stream, out_stream, true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.GetInnermostException();
            }
            finally
            {
                File.Delete(path);
                File.Move(encryptedFilePath, path);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Copies the encrypted contents of a file to a new temporary file, decrypts that file into a new .decrypted file.
        /// Reads the decrypted file into a new RecyclableMemoryStream and returns the contents .
        /// </summary>
        /// <param name="path">The location of the file.</param>
        /// <returns>Reads the decrypted file into a new RecyclableMemoryStream and returns the contents</returns>
        public MemoryStream Decrypt(string path)
        {
            MemoryStream decryptedMemoryStream;

            var tmpFile = path + DateTimeOffset.UtcNow.Millisecond;
            var decryptedFilePath = tmpFile + ".decrypted";

            try
            {
                using (var in_stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (var out_stream = new FileStream(decryptedFilePath, FileMode.Create, FileAccess.Write))
                    {
                        CryptStream(EncryptionKey, in_stream, out_stream, false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.GetInnermostException();
            }
            finally
            {
                File.Delete(tmpFile);

                decryptedMemoryStream = ConvertToMemoryStream(decryptedFilePath);

                File.Delete(decryptedFilePath);
            }

            return decryptedMemoryStream;
        }

        public void CryptStream(string password, Stream in_stream, Stream out_stream, bool encrypt)
        {
            var aes_provider = new AesCryptoServiceProvider();
            var key_size_bits = 0;

            for (var i = 1024; i > 1; i--)
            {
                if (aes_provider.ValidKeySize(i))
                {
                    key_size_bits = i;
                    break;
                }
            }

            var block_size_bits = aes_provider.BlockSize;

            byte[] salt = { 0x0, 0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0xF1, 0xF0, 0xEE, 0x21, 0x22, 0x45 };

            MakeKeyAndIV(password, salt, key_size_bits, block_size_bits, out var key, out var iv);

            var crypto_transform = encrypt ? aes_provider.CreateEncryptor(key, iv) : aes_provider.CreateDecryptor(key, iv);

            try
            {
                using (var crypto_stream = new CryptoStream(
                    out_stream,
                    crypto_transform,
                    CryptoStreamMode.Write))
                {
                    const int Block_size = 1024;

                    var buffer = new byte[Block_size];

                    while (true)
                    {
                        var bytes_read = in_stream.Read(buffer, 0, Block_size);

                        if (bytes_read == 0)
                        {
                            break;
                        }

                        crypto_stream.Write(buffer, 0, bytes_read);
                    }
                }
            }
            catch
            {
                // ignored
            }

            crypto_transform.Dispose();
        }

        private static void MakeKeyAndIV(string password, byte[] salt, int key_size_bits, int block_size_bits, out byte[] key, out byte[] iv)
        {
            var derive_bytes = new Rfc2898DeriveBytes(password, salt, 1000);

            key = derive_bytes.GetBytes(key_size_bits / 8);
            iv = derive_bytes.GetBytes(block_size_bits / 8);
        }

        /// <summary>
        /// Reads a file into a new RecyclableMemoryStream and returns the contents.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Returns the contents of the file as a MemoryStream</returns>
        private static MemoryStream ConvertToMemoryStream(string path)
        {
            try
            {
                var memoryStream = new RecyclableMemoryStream(new RecyclableMemoryStreamManager());

                using (Stream input = File.OpenRead(path))
                {
                    input.CopyTo(memoryStream);
                }

                memoryStream.Position = 0;

                File.Delete(path);

                return memoryStream;
            }
            catch (Exception ex)
            {
                throw ex.GetInnermostException();
            }
        }
    }
}
