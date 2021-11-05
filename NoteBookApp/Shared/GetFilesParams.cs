using System;
using System.Collections.Generic;
using Microsoft.VisualBasic.CompilerServices;

namespace NoteBookApp.Shared
{
    public enum FilesSortColumn
    {
        FileName,
        CreatedDateTime,
        Length
    }

    public class GetFilesParams : PartialRetrievingInfo
    {
        public bool SortAsc { get; set; }

        public FilesSortColumn? SortBy { get; set; } = FilesSortColumn.FileName;

        public Guid? ObjectId { get; set; }

        public string? SearchText { get; set; }
    }
}