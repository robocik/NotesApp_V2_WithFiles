using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using NoteBookApp.Logic.Domain;
using NoteBookApp.Logic.Interfaces;
using NoteBookApp.Shared;
using NoteBookApp.Shared.Exceptions;

namespace NoteBookApp.FileSystems.AzureBlobStorage
{
    public class AzureBlobStorageProvider:IFileSystemProvider
    {
        private string _connectionString;
        private BlobServiceClient _blobServiceClient;
        public AzureBlobStorageProvider(string connectionString)
        {
            _connectionString = connectionString;
            _blobServiceClient = new BlobServiceClient(_connectionString);

        }

        public string GetFileUrl(string container, string file, string? param = null)
        {
            return $"{_blobServiceClient.Uri}{container}/{file}?{param}";
        }
        public async Task<FileMetaInfo> Upload(Stream stream, FileIdentifier data)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(data.Container);
            if (FileMetaInfo.ProfileAwatarsFolder == data.Container)
            {
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
            }
            else
            {
                await containerClient.CreateIfNotExistsAsync();
            }
            var blob = containerClient.GetBlobClient(data.File);
            var reponse = await blob.UploadAsync(stream, overwrite: true);
            return new FileMetaInfo(data.File, 0);
        }


        public async Task<FileAccessToken> GenerateUploadToken(string companyId, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(companyId);
            //if (FileMetaInfo.ProfileAwatarsFolder == companyId)
            //{
            //    await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
            //}
            //else
            {
                await containerClient.CreateIfNotExistsAsync();
            }
            var fileClient = containerClient.GetBlobClient(fileName);
            var blobBuilder = new BlobSasBuilder(BlobSasPermissions.Write, DateTimeOffset.UtcNow.AddMinutes(20));
            blobBuilder.BlobContainerName = companyId;
            blobBuilder.BlobName = fileName;
            var uri = fileClient.GenerateSasUri(blobBuilder);
            string blob = uri.GetLeftPart(UriPartial.Path);
            string token = uri.Query;
            return new FileAccessToken(blob, token, new Guid(fileName));
        }

        public Task Delete(FileIdentifier data)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(data.Container);
            if (containerClient == null)
            {
                throw new ObjectNotFoundException("Azure container not found");
            }
            var blob = containerClient.GetBlobClient(data.File);
            return blob.DeleteIfExistsAsync();
        }

        public string GenerateReadToken(string companyId, string fileId, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(companyId);
            var fileClient = containerClient.GetBlobClient(fileId);
            var blobBuilder = new BlobSasBuilder(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddMinutes(20));
            blobBuilder.BlobContainerName = companyId;
            blobBuilder.BlobName = fileId;
            blobBuilder.ContentDisposition = $"attachment; filename={fileName}";
            var uri = fileClient.GenerateSasUri(blobBuilder);
            return uri.ToString();
        }
    }
}