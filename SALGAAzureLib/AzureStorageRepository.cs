using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SALGAEvidenceRepository
{
    public class AzureStorageRepository : IAzureRepository
    {
        private String _connectionString;

        public void SetConnectionString(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<String> AddFile(String containerName, string fileName, Stream file)
        {
            var containerClient = new BlobContainerClient(_connectionString, containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            file.Position = 0;

            var response =  await blobClient.UploadAsync(file, overwrite: true);
            return blobClient.Uri.ToString();
        }


        public async Task<bool> UpdateMetaData(String containerName, String fileName, IDictionary<String, String> MetaData)
        {
            var containerClient = new BlobContainerClient(_connectionString, containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            var response = await blobClient.SetMetadataAsync(MetaData);
            return true;
        }

        bool IAzureRepository.GetFile(String containerName, string fileName, Stream file)
        {
            var containerClient = new BlobContainerClient(_connectionString, containerName);
            var blobClient= containerClient.GetBlobClient(fileName);
            blobClient.Download();
            return true;
        }
        public async Task<bool> DeleteFile(String containerName, string fileName)
        {
            var containerClient = new BlobContainerClient(_connectionString, containerName);
            var response= await containerClient.DeleteBlobAsync(fileName, DeleteSnapshotsOption.IncludeSnapshots);
            return true;
        }

        public IEnumerable<BlobItem> GetContainerFiles(String containerName)
        {
            var containerClient = new BlobContainerClient(_connectionString, containerName);
            var blobInfos = containerClient.GetBlobs(Azure.Storage.Blobs.Models.BlobTraits.Metadata).ToList();
            return blobInfos;
        }
    }
}
