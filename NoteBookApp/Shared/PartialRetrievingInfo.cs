namespace NoteBookApp.Shared
{
    public class PartialRetrievingInfo
    {
        public const int DefaultPageSize = 10;
        public const int AllElementsPageSize = 0;

        public PartialRetrievingInfo()
        {
            PageSize = DefaultPageSize;
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}