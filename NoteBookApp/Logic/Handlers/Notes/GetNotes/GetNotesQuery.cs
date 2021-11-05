using MediatR;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Handlers.Notes
{
    
    public class GetNotesQuery: GetNotesParam, IRequest<PagedResult<NoteDto>>
    {
        
    }
}