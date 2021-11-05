using System;
using MediatR;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Handlers.Notes
{
    
    public class UpdateNoteCommand: UpdateNoteParam, IRequest
    {
        public UpdateNoteCommand(Guid id,string content):base(id,content)
        {
        }
    }
}