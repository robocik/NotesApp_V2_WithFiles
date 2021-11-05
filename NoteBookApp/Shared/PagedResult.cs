using System.Collections.Generic;

namespace NoteBookApp.Shared
{
    public class PagedResult<T>
    {
        public PagedResult()
        {
            Items = new List<T>();
        }
        public PagedResult(IList<T> items, int allItemsCount, int pageIndex, int pageSize)
        {
            Items = items;
            AllItemsCount = allItemsCount;
            PageIndex = pageIndex;
            PageSize = pageSize;
        }

        public int PageIndex { get; set; }

        public IList<T> Items { get; set; }

        public int AllItemsCount { get; set; }
        public int PageSize { get; set; }
    }
}