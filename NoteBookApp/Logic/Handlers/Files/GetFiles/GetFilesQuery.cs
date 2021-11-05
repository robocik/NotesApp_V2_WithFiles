using MediatR;
using NoteBookApp.Shared;

namespace NoteBookApp.Logic.Handlers.Files
{
    
    public class GetFilesQuery: GetFilesParams, IRequest<PagedResult<FileDto>>
    {
    }
}