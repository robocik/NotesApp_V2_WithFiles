using System;
using System.IO;
using System.Threading.Tasks;
using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using NoteBookApp.Shared;

namespace NoteBookApp.Client.Services
{
    public interface IFileUploader
    {
        Task Upload(Stream fileContent, IProgress<long> progressCallback, FileAccessToken fileAccessToken);
    }
    public class AzureBlobService : IFileUploader
    {
        public async Task Upload(Stream fileContent, IProgress<long> progressCallback, FileAccessToken fileAccessToken)
        {
            var credential = new AzureSasCredential(fileAccessToken.Token);
            var blobClient = new BlobClient(new Uri(fileAccessToken.BlobUrl), credential, new BlobClientOptions()
            {
                Retry = { MaxRetries = 1 }
            });

            await blobClient.UploadAsync(fileContent, new BlobUploadOptions
            {
                TransferOptions = new StorageTransferOptions
                {
                    MaximumTransferSize = NoteBookApp.Shared.Constants.MaxFileSize
                },

                ProgressHandler = progressCallback
            });
        }
    }
}