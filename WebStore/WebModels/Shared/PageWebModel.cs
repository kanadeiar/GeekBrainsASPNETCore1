using System;

namespace WebStore.WebModels.Shared
{
    public class PageWebModel
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int StartNumber { get; set; }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => (PageNumber < TotalPages);


        public PageWebModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int) Math.Ceiling(count / (double)pageSize);
            StartNumber = (pageNumber - 1) * pageSize + 1;
        }

    }
}
