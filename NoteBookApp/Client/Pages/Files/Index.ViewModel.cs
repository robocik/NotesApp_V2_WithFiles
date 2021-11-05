using System;
using System.Collections.Generic;
using NoteBookApp.Shared;

namespace NoteBookApp.Client.Pages.Files
{
    public class IndexViewModel
    {
        public IList<FileViewModel>? Files { get; set; }
        public int Count { get; set; }
    }

    public class FileViewModel
    {
        public FileViewModel(FileDto file)
        {
            Id = file.Id;
            FileName = file.FileName;
            Length = file.Length.ToString();
        }

        public Guid Id { get; }
        public string FileName { get; set; }

        public string Length { get; set; }
    }
}