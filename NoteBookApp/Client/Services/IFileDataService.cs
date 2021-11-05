using System;
using System.IO;
using System.Threading.Tasks;
using NoteBookApp.Shared;

namespace NoteBookApp.Client.Services
{
    public interface IFileDataService
    {
        Task<Guid> UploadFile(FileMetaData fileParam, Stream fileContent, Action<long, long>? progressCallback);

        Task<PagedResult<FileDto>> GetFiles(GetFilesParams param);

        Task DeleteFile(Guid fileId);

        Task<string> GetFileDirectUrl(Guid fileId);

        Task DeleteAvatar();

        Task UploadAvatarFull(UploadFileParam fileParam, Stream fileContent);
    }
}