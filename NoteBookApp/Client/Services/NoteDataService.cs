using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using NoteBookApp.Shared;

namespace NoteBookApp.Client.Services
{
    public class NoteDataService:DataServiceBase, INoteDataService
    {
        public NoteDataService(HttpClient httpClient) : base(httpClient)
        {
        }

        public async Task<PagedResult<NoteDto>> GetNotes(GetNotesParam param)
        {
            var url = GetUrl("api/notes", param);
            
            return await Execute(async httpClient =>
            {
                var res= await  httpClient.GetFromJsonAsync<PagedResult<NoteDto>>(url, CreateOptions()).ConfigureAwait(false);
                return res!;
            }).ConfigureAwait(false);
        }

        public async Task<NoteDto> GetNoteDetails(Guid id)
        {
            return await Execute(async httpClient =>
            {
                var note=await httpClient.GetFromJsonAsync<NoteDto>($"api/notes/{id}", CreateOptions()).ConfigureAwait(false);
                return note!;
            }); 
        }

        public Task CreateNote(CreateNoteParam param)
        {
            return Execute(httpClient =>
            {
                return httpClient.PostAsJsonAsync("api/notes", param, CreateOptions());
            });
        }
        
        public Task UpdateNote(UpdateNoteParam param)
        {
            return Execute(httpClient =>
            {
                return httpClient.PutAsJsonAsync("api/notes", param, CreateOptions());
            });
        }

        public Task DeleteNote(Guid id)
        {
            return Execute(httpClient =>
            {
                return httpClient.DeleteAsync($"api/notes/{id}");
            });
        }
    }
}