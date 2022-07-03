using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace Persistence.Storage.Files.Azure
{
    public class FileRepository
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _container;

        public FileRepository(string containerName)
        {
            var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("appSettings.json");
            var config = configurationBuilder.Build();
            var connectionString = config["FileStorage:Azure:ConnectionString"];
            _blobServiceClient = new BlobServiceClient(connectionString);

            try
            {
                _container = _blobServiceClient.GetBlobContainerClient(containerName);
            }
            catch (Exception ex)
            {
                _container = _blobServiceClient.CreateBlobContainer(connectionString);
                throw;
            }
        }

        public async Task Add(Stream stream, string fileName)
        {
            BlobClient blobClient = _container.GetBlobClient(fileName);
            await blobClient.UploadAsync(stream);
        }

        public void Delete()
        {

        }
        public Stream Get(string fileName)
        {
            var blob = _container.GetBlobClient(fileName);
            return blob.OpenRead();
        }
    }
}
