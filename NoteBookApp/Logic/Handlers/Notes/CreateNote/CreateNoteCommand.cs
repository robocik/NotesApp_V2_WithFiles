using MediatR;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Handlers.Notes
{
    
    public class CreateNoteCommand: CreateNoteParam, IRequest
    {
        public CreateNoteCommand(string content):base(content)
        {
        }
    }
}