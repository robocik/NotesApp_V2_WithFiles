using System;
using MediatR;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Handlers.Files
{
    public class UploadFileCommand: FileMetaData,IRequest<FileAccessToken>
    {
        public UploadFileCommand()
        {

        }
        public UploadFileCommand(string name, long fileLength, Guid objectId) : base(name, fileLength, objectId)
        {
        }
    }
}