using System;
using System.Collections.Generic;
using NoteBookApp.Shared;

namespace NoteBookApp.Client.Pages.Notes
{
    public class NoteViewModel
    {
        private readonly NoteDto _note;

        public NoteViewModel(NoteDto note)
        {
            _note = note;
        }

        public Guid Id => _note.Id;
        public DateTime CreatedDateTime => _note.CreatedDateTime.ToLocalTime();

        public string? Content => _note.Content;
    }
    public class IndexViewModel
    {
        public IList<NoteViewModel>? Notes { get; set; }
        public int Count { get; set; }
    }
}