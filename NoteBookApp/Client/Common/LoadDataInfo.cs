namespace NoteBookApp.Client.Common
{
    public class LoadDataInfo
    {
        public static readonly LoadDataInfo EmptyAsc = new LoadDataInfo(0, true, null);
        public static readonly LoadDataInfo EmptyDesc = new LoadDataInfo(0, false, null);
        public LoadDataInfo(int pageIndex, bool sortAsc, string? sortBy)
        {
            PageIndex = pageIndex;
            SortAsc = sortAsc;
            SortBy = sortBy;
        }
        public int PageIndex { get; }

        public bool SortAsc { get; }

        public string? SortBy { get; }
    }
}