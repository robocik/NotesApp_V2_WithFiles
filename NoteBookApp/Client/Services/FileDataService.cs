using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using NoteBookApp.Shared;

namespace NoteBookApp.Client.Services
{
    public class FileDataService:DataServiceBase,IFileDataService
    {
        private readonly IFileUploader _fileUploader;
        public FileDataService(HttpClient httpClient, IFileUploader fileUploader) : base(httpClient)
        {
            _fileUploader = fileUploader;
        }

        public async Task<Guid> UploadFile(FileMetaData fileParam, Stream fileContent, Action<long, long>? progressCallback)
        {
            return await Execute(async httpClient =>
            {
                var length = fileParam.FileLength;
                //first send info about a new file to the WebAPI
                var result = await httpClient.PostAsJsonAsync("api/files", fileParam, CreateOptions()).ConfigureAwait(false);
                if (!result.IsSuccessStatusCode)
                {
                    await ConvertToException(result).ConfigureAwait(false);
                }
                result.EnsureSuccessStatusCode();
                var fileAccessToken = await result.Content.ReadAsAsync<FileAccessToken>().ConfigureAwait(false);
                try
                {
                    var progress = new Progress<long>((progress) =>
                    {
                        progressCallback?.Invoke(progress, length);
                    });
                    //now send a content of the file to Azure Blob Storage
                    await _fileUploader.Upload(fileContent, progress, fileAccessToken).ConfigureAwait(false);
                    return fileAccessToken.FileId;
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        await DeleteFile(fileAccessToken.FileId).ConfigureAwait(false);
                    }
                    throw;
                }
            }).ConfigureAwait(false);
        }


        public async Task<PagedResult<FileDto>> GetFiles(GetFilesParams param)
        {
            var url = GetUrl("api/files", param);
            return await Execute(async httpClient =>
            {
                var res = await httpClient.GetFromJsonAsync<PagedResult<FileDto>>(url, CreateOptions()).ConfigureAwait(false);
                return res!;
            }).ConfigureAwait(false);
        }

        public async Task DeleteFile(Guid fileId)
        {
            await Execute(httpClient =>
            {
                return httpClient.DeleteAsync($"api/files/{fileId}");
            }).ConfigureAwait(false);
        }

        public async Task<string> GetFileDirectUrl(Guid fileId)
        {
            var url = $"api/files/fileDirectUrl/{fileId}";
            return await Execute(async httpClient =>
            {
                var res = await httpClient.GetStringAsync(url).ConfigureAwait(false);
                return res!;
            });
        }

        public async Task UploadAvatarFull(UploadFileParam fileParam, Stream fileContent)
        {
            await Execute(async httpClient =>
            {
                var content = new StreamContent(fileContent);
                
                var multipartContent = new MultipartFormDataContent();
                multipartContent.Add(content, "File", fileParam.FileName);
                var infoJson = JsonContent.Create(fileParam);
                multipartContent.Add(infoJson, "Meta");
                var result = await httpClient.PostAsync("api/files/uploadAvatarFull", multipartContent).ConfigureAwait(false);
                return result;
            }).ConfigureAwait(false);
        }

        public async Task DeleteAvatar()
        {
            var url = "api/files/deleteAvatar";

            await Execute(httpClient =>
            {
                return httpClient.DeleteAsync(url);
            }).ConfigureAwait(false);
        }
    }
}