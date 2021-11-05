using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NoteBookApp.Client.Services;

namespace NoteBookApp.Client.Pages.Notes
{
    public partial class Edit
    {
        public EditViewModel? Model { get; set; }
        
        [Inject] 
        private INoteDataService NoteDataService { get; set; } = null!;

        [Inject] 
        private NavigationManager NavManager { get; set; } = null!;
        [Parameter]
        public string? NoteId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (NoteId == null)
            {
                Model = new EditViewModel();
            }
            else
            {
                var note=await NoteDataService.GetNoteDetails(new Guid(NoteId)).ConfigureAwait(false);
                Model = new EditViewModel(note);
            }
            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        private async Task OnSubmitForm()
        {
            if (Model == null)
            {
                return;
            }
            try
            {
                Model.IsSaving = true;
                if (NoteId != null)
                {
                    var param = Model.GetUpdateParam();
                    await NoteDataService.UpdateNote(param).ConfigureAwait(false);
                }
                else
                {
                    var param = Model.GetCreateParam();
                    await NoteDataService.CreateNote(param).ConfigureAwait(false);
                }
                
                NavManager.NavigateTo("/Notes");
            }
            finally
            {
                Model.IsSaving = false;
            }
        }
    }
}