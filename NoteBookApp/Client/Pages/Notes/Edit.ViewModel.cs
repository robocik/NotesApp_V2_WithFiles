using System;
using System.Threading.Tasks;
using NoteBookApp.Shared;

namespace NoteBookApp.Client.Pages.Notes
{
    public class EditViewModel
    {
        private readonly NoteDto? _note;

        public EditViewModel(NoteDto note)
        {
            _note = note;
            Content = note.Content ?? String.Empty;
        }

        public EditViewModel()
        {

        }

        public bool IsSaving { get; set; }
        public string Content { get; set; } = string.Empty;

        public CreateNoteParam GetCreateParam()
        {
            var param = new CreateNoteParam(Content);
            return param;
        }

        public UpdateNoteParam GetUpdateParam()
        {
            var param = new UpdateNoteParam(_note!.Id,Content);
            return param;
        }
    }
}