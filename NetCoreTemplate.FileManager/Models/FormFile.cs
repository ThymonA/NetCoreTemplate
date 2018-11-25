namespace NetCoreTemplate.FileManager.Models
{
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;

    public sealed class FormFile : IFormFile
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

        public FormFile(string name, string fileName, long length, string contentType, Stream stream)
        {
            Name = name;
            FileName = fileName;
            Length = length;
            ContentType = contentType;
            Stream = stream;
        }

        public FormFile(string name, string fileName, long length, string contentType, Stream stream, IConfiguration configuration)
        {
            Name = name;
            FileName = fileName;
            Length = length;
            ContentType = contentType;
            Stream = stream;
            Configuration = configuration;
        }

        public Stream OpenReadStream()
        {
            return Stream;
        }

        public void CopyTo(Stream target)
        {
            Stream.CopyTo(target);
        }

        public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = new CancellationToken())
        {
            await Stream.CopyToAsync(target, cancellationToken);
        }

        public string ContentType { get; }

        public string ContentDisposition => string.Empty;

        public IHeaderDictionary Headers => default(HeaderDictionary);

        public long Length { get; }

        public string Name { get; }

        public string FileName { get; }

        public Stream Stream { get; set; }
    }
}
