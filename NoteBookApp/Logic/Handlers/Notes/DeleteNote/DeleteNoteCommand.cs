using System;
using MediatR;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Handlers.Notes
{
    
    public class DeleteNoteCommand: IRequest
    {
        public DeleteNoteCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
        
    }
}