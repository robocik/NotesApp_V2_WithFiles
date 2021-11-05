using System;

namespace NoteBookApp.Shared
{
    public class NoteDto
    {
        public Guid Id { get; set; }

        public string? Content { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}