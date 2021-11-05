using System;
using MediatR;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Handlers.Notes
{
    
    public class GetNoteDetailsQuery: IRequest<NoteDto>
    {
        public GetNoteDetailsQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}