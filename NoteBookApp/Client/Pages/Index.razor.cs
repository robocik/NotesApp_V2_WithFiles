using System;
using System.Threading.Tasks;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using NoteBookApp.Client.Services;
using NoteBookApp.Shared;

namespace NoteBookApp.Client.Pages
{
    public partial class Index
    {
        public IndexViewModel? Model { get; set; }
        [Inject]
        private IFileDataService FileDataService { get; set; } = null!;
        [Inject]
        private IAccountDataService AccountDataService { get; set; } = null!;

        private async Task DeleteAvatar()
        {
            await FileDataService.DeleteAvatar().ConfigureAwait(false);
            await RefreshView().ConfigureAwait(false);
        }

        protected override async Task OnInitializedAsync()
        {
            await RefreshView().ConfigureAwait(false);
            await base.OnInitializedAsync().ConfigureAwait(false);
        }

        async Task RefreshView()
        {
            try
            {
                var profile = await AccountDataService.GetMyProfile().ConfigureAwait(false);
                Model = new IndexViewModel(profile);
            }
            catch (Exception e)
            {
                ShowError("Nie udało się pobrać informacji o profilu",e);
            }
            
        }

        private async Task UploadAvatar(IFileListEntry file)
        {
            var param = new UploadFileParam(file.Name, file.Size);
            await FileDataService.UploadAvatarFull(param, file.Data).ConfigureAwait(false);
            await RefreshView().ConfigureAwait(false);
        }
    }
}