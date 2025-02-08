using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;


namespace ApiGateway.BlobStorage
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            var containerName = configuration["AzureBlobStorage:ContainerName"];

            _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists(PublicAccessType.Blob);
        }

        public BlobStorageService()
        {
            string connectionString = "UseDevelopmentStorage=true;";
            string containerName = "student-uploads";

            _containerClient = new BlobContainerClient(connectionString, containerName);
            _containerClient.CreateIfNotExists(PublicAccessType.Blob);
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);

            var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };

            await blobClient.UploadAsync(fileStream, blobHttpHeaders);

            return blobClient.Uri.ToString();
        }
    }
}
