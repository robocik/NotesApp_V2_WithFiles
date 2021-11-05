using System.IO;
using System.Threading.Tasks;
using NoteBookApp.Logic.Domain;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Interfaces
{
    public interface IFileSystemProvider
    {
        Task<FileMetaInfo> Upload(Stream stream, FileIdentifier data);
        Task<FileAccessToken> GenerateUploadToken(string userId, string fileName);

        Task Delete(FileIdentifier data);

        string GenerateReadToken(string companyId, string fileId, string fileName);

        string GetFileUrl(string container, string file, string? param = null);
    }
}