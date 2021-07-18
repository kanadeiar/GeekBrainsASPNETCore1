using System;

namespace WebStore.Domain.WebModels.Shared
{
    /// <summary> Вебмодель пагинации </summary>
    public class PageWebModel
    {
        /// <summary> Номер страницы </summary>
        public int PageNumber { get; set; }
        /// <summary> Всего страниц </summary>
        public int TotalPages { get; set; }
        /// <summary> Начальный номер, для отображения порядковых номеров </summary>
        public int StartNumber { get; set; }

        /// <summary> Есть первая страница </summary>
        public bool HasFirstPage => PageNumber > 3;
        /// <summary> Есть ранее предидущей страница </summary>
        public bool HasPrevPreviousPage => PageNumber > 2;
        /// <summary> Есть предидущая страница </summary>
        public bool HasPreviousPage => PageNumber > 1;
        /// <summary> Есть следущая страница </summary>
        public bool HasNextPage => (PageNumber < TotalPages);
        /// <summary> Есть позднее следущей страница </summary>
        public bool HasNextNextPage => (PageNumber < TotalPages - 1);
        /// <summary> Есть последняя страница </summary>
        public bool HasLastPage => (PageNumber < TotalPages - 2);

        /// <summary> Конструктор </summary>
        /// <param name="count">Количество товаров</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <param name="pageSize">Размерность страницы</param>
        public PageWebModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int) Math.Ceiling(count / (double)pageSize);
            StartNumber = (pageNumber - 1) * pageSize + 1;
        }

    }
}
