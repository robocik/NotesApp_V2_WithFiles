namespace NoteBookApp.Shared
{
    public enum NoteSort
    {
        CreatedDateTime,
        Content
    }

    public class GetNotesParam:PartialRetrievingInfo
    {
        public bool SortAsc { get; set; }

        public NoteSort SortBy { get; set; }
    }
}