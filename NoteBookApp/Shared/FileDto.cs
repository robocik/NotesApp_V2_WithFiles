using System;

namespace NoteBookApp.Shared
{
    public class FileDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = null!;

        public long Length { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}