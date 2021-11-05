using System;

namespace NoteBookApp.Shared
{
    public class UpdateNoteParam
    {
        public UpdateNoteParam(Guid id,string content)
        {
            Id = id;
            Content = content;
        }

        public Guid Id { get; set; }
        public string Content { get; set; }
    }
}