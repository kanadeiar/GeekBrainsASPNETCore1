using System;

namespace WebStore.Domain.WebModels.Shared
{
    /// <summary> Вебмодель пагинации </summary>
    public class PageWebModel
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int StartNumber { get; set; }

        public bool HasFirstPage => PageNumber > 3;
        public bool HasPrevPreviousPage => PageNumber > 2;
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => (PageNumber < TotalPages);
        public bool HasNextNextPage => (PageNumber < TotalPages - 1);
        public bool HasLastPage => (PageNumber < TotalPages - 2);

        public PageWebModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int) Math.Ceiling(count / (double)pageSize);
            StartNumber = (pageNumber - 1) * pageSize + 1;
        }

    }
}
