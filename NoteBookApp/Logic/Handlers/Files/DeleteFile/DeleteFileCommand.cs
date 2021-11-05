using System;
using MediatR;

namespace NoteBookApp.Logic.Handlers.Files.DeleteFile
{
    public class DeleteFileCommand: IRequest
    {
        public DeleteFileCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
        
    }
}