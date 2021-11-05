using System.IO;
using MediatR;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Handlers.Files
{
    public class UploadAvatarFullCommand: UploadAvatarParam, IRequest
    {
        public UploadAvatarFullCommand()
        {

        }
        public UploadAvatarFullCommand(string name, long fileLength,Stream content) : base(name, fileLength,content)
        {
        }
        
    }
}