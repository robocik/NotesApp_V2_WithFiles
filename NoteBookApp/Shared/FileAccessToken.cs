using System;

namespace NoteBookApp.Shared
{
    public class FileAccessToken
    {
        public FileAccessToken(string blobUrl, string token, Guid fileId)
        {
            BlobUrl = blobUrl;
            Token = token;
            FileId = fileId;
        }
        public string BlobUrl { get; set; } = null!;

        public string Token { get; set; } = null!;

        public Guid FileId { get; set; }
        
    }
}