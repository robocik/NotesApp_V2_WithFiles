using System;
using System.Linq;
using System.Threading.Tasks;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Microsoft.VisualBasic.CompilerServices;
using NoteBookApp.Client.Common;
using NoteBookApp.Client.Services;
using NoteBookApp.Shared;
using NoteBookApp.Shared.Exceptions;
using Radzen;

namespace NoteBookApp.Client.Pages.Files
{
    public partial class Index
    {
        public IndexViewModel Model { get; set; } = new IndexViewModel();
        [Inject] 
        private IFileDataService FileDataService { get; set; } = null!;
        [Inject]
        public IJSRuntime JSRuntime { get; set; } = null!;
        [Parameter]
        public string? NoteId { get; set; }

        private async Task refreshFiles(LoadDataInfo? info = null)
        {
            info ??= LoadDataInfo.EmptyAsc;
            var param = new GetFilesParams();
            param.PageIndex = info.PageIndex;
            if (!string.IsNullOrEmpty(info.SortBy))
            {
                param.SortBy = Enum.Parse<FilesSortColumn>(info.SortBy);
            }

            param.PageSize = NoteBookApp.Client.Constants.PageSize;

            if (NoteId != null)
            {
                param.ObjectId = Guid.Parse(NoteId);
            }
            
            param.SortAsc = info.SortAsc;
            var items = await FileDataService.GetFiles(param);
            Model.Count = items.AllItemsCount;
            Model.Files = items.Items.Select(x => new FileViewModel(x)).ToList();

            StateHasChanged();
        }

        private Task LoadData(LoadDataArgs arg)
        {
            var info = arg.GetLoadDataInfo(NoteBookApp.Client.Constants.PageSize);
            return refreshFiles(info);
        }

        private async Task DownloadFileAsync(Guid fileId)
        {
            try
            {
                var url = await FileDataService.GetFileDirectUrl(fileId);
                await JSRuntime.InvokeVoidAsync("DownloadFile", url);
            }
            catch (ObjectNotFoundException ex)
            {
                ShowError("Plik już nie istnieje.", ex);
                await refreshFiles();
            }
            catch (Exception ex)
            {
                ShowError("Nie udało się pobrać pliku. Proszę spróbować ponownie.", ex);
            }
        }

        private async Task DeleteFile(Guid fileId)
        {
            var result = await DialogService.Confirm("Czy na pewno chcesz usunąć wybrany plik?", "NoteBookApp", new ConfirmOptions()
            {
                OkButtonText = "Tak",
                CancelButtonText = "Nie"
            });
            if (result == true)
            {
                try
                {
                    await FileDataService.DeleteFile(fileId);
                }
                catch (ObjectNotFoundException)
                {
                }
                catch (Exception ex)
                {
                    ShowError("Nie udało się usunąć pliku. Proszę spróbować ponownie.", ex);
                }
                await refreshFiles();
            }
        }

        private async Task HandleFileInputChange(IFileListEntry[] files)
        {
            foreach (var file in files)
            {
                try
                {
                    
                    if (file.Size > NoteBookApp.Shared.Constants.MaxFileSize)
                    {
                        ShowError($"Max rozmiar pliku to {NoteBookApp.Shared.Constants.MaxFileSize} B", null);
                        continue;
                    }
                    var param = new FileMetaData(file.Name, file.Size, Guid.Parse(NoteId!));
                    await FileDataService.UploadFile(param, file.Data,null);

                }
                catch (InvalidOperationException ex)
                {
                    ShowError("Nie masz wystarczającej ilości wolnego miejsca", ex);
                    break;
                }
                catch (Exception ex)
                {
                    ShowError("Nie udało się wysłać pliku",ex);
                }
            }

            await refreshFiles();
        }

        private Task HandleDragEnter(DragEventArgs arg)
        {
            return Task.CompletedTask;
        }

        private Task HandleDragLeave(DragEventArgs arg)
        {
            return Task.CompletedTask;
        }
    }
}