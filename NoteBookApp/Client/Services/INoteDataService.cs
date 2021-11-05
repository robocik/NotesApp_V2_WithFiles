using System;
using System.Threading.Tasks;
using NoteBookApp.Shared;

namespace NoteBookApp.Client.Services
{
    public interface INoteDataService
    {
        Task<PagedResult<NoteDto>> GetNotes(GetNotesParam param);

        Task<NoteDto> GetNoteDetails(Guid id);

        Task CreateNote(CreateNoteParam param);

        Task UpdateNote(UpdateNoteParam param);

        Task DeleteNote(Guid id);
    }
}