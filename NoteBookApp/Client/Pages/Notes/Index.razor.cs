using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NoteBookApp.Client.Common;
using NoteBookApp.Client.Services;
using NoteBookApp.Shared;
using Radzen;

namespace NoteBookApp.Client.Pages.Notes
{
    public partial class Index
    {
        public IndexViewModel Model { get; set; } = new IndexViewModel();

        [Inject]
        private INoteDataService NoteDataService { get; set; } = null!;

        private Task LoadData(LoadDataArgs arg)
        {
            var info = arg.GetLoadDataInfo(Constants.PageSize);
            return RefreshView(info);
        }

        private async Task RefreshView(LoadDataInfo? info = null)
        {
            info ??= LoadDataInfo.EmptyAsc;
            var param = new GetNotesParam();
            param.PageIndex = info.PageIndex;
            param.PageSize = Constants.PageSize;
            param.SortAsc = info.SortAsc;
            
            if (!string.IsNullOrEmpty(info.SortBy))
            {
                param.SortBy = Enum.Parse<NoteSort>(info.SortBy);
            }

            var notes = await NoteDataService.GetNotes(param).ConfigureAwait(false);
            Model.Notes = notes.Items.Select(x=>new NoteViewModel(x)).ToList();
            Model.Count = notes.AllItemsCount;
        }

        private async Task DeleteNote(Guid noteId)
        {
            var result = await DialogService.Confirm("Czy na pewno chcesz usunąć notatkę?", "MyNotes", new ConfirmOptions()
            {
                OkButtonText = "Tak",
                CancelButtonText = "Nie"
            }).ConfigureAwait(false);
            if (result == true)
            {
                await NoteDataService.DeleteNote(noteId).ConfigureAwait(false);
                await RefreshView().ConfigureAwait(false);
                StateHasChanged();
            }
        }
    }
}