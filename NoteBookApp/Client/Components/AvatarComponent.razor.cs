using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;

namespace NoteBookApp.Client.Components
{
    public partial class AvatarComponent
    {
        private string? _avatarUrl;

        [Parameter]
        public RenderFragment? EmptyContent { get; set; }

        [Parameter]
        public string EmptyText { get; set; } = "Brak awatara. Kliknij, aby dodać";

        [Parameter] 
        public bool IsRounded { get; set; } = true;

        [Parameter]
        public string? AvatarUrl
        {
            get => _avatarUrl;
            set
            {
                _avatarUrl = value;
                HasAvatar = _avatarUrl != null;
            }
        }

        [Parameter]
        public EventCallback<IFileListEntry> OnUploadAvatar { get; set; }
        [Parameter]
        public EventCallback OnDeleteAvatar { get; set; }

        private string ImageClass => IsRounded ? "rounded-circle" : string.Empty;
        public bool HasAvatar { get; set; }

        public bool IsAvatarSaving { get; set; }

        private async Task HandleFileInputChange(IFileListEntry[] arg)
        {
            try
            {
                var file = arg.SingleOrDefault();
                if (file != null)
                {
                    List<string> acceptedFileTypes = new List<string>() { "image/png", "image/jpeg", "image/gif" };
                    if (!acceptedFileTypes.Contains(file.Type))
                    {
                        ShowError("Musisz wybrać plik obrazka. Inne typy plików są nieobsługiwane.");
                        return;
                    }
                    if (file.Size > NoteBookApp.Shared.Constants.AvatarSize)
                    {
                        ShowError("Wybrany obrazek jest za duży. Obsługujemy pliki o rozmiarze max. 1 MB.");
                        return;
                    }
                    IsAvatarSaving = true;
                    
                    await OnUploadAvatar.InvokeAsync(file);
                }

            }
            catch (InvalidOperationException ex)
            {
                ShowError("Nie masz wystarczającej ilości wolnego miejsca.", ex);
            }
            catch (Exception ex)
            {
                ShowError("Nie udało się zapisać zmian. Proszę spróbować ponownie.", ex);
            }
            finally
            {
                IsAvatarSaving = false;
            }


        }

        private async Task DeleteAvatar()
        {
            try
            {
                IsAvatarSaving = true;
                await OnDeleteAvatar.InvokeAsync(AvatarUrl);
            }
            catch (Exception ex)
            {
                ShowError("Nie udało się zapisać zmian. Proszę spróbować ponownie.", ex);
            }
            finally
            {
                IsAvatarSaving = false;
            }

        }
    }
}