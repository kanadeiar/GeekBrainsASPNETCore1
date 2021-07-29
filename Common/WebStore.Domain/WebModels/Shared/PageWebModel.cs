using System;

namespace WebStore.Domain.WebModels.Shared
{
    /// <summary> Вебмодель пагинации </summary>
    public class PageWebModel
    {
        /// <summary> Номер страницы </summary>
        public int Page { get; set; }
        /// <summary> Размер страницы </summary>
        public int PageSize { get; set; }
        /// <summary> Всего отобранных товаров </summary>
        public int TotalItems { get; set; }
        /// <summary> Начальный номер, для отображения порядковых номеров </summary>
        public int StartNumber { get; set; }

        /// <summary> Конструктор </summary>
        /// <param name="count">Количество товаров</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Размерность страницы</param>
        public PageWebModel(int count, int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
            TotalItems = count;
            StartNumber = (page - 1) * pageSize + 1;
        }
        /// <summary> Всего страниц </summary>
        public int TotalPages => PageSize == 0 ? 0 : (int) Math.Ceiling((double) TotalItems / PageSize);
        /// <summary> Есть первая страница </summary>
        public bool HasFirstPage => Page > 3;
        /// <summary> Есть ранее предидущей страница </summary>
        public bool HasPrevPreviousPage => Page > 2;
        /// <summary> Есть предидущая страница </summary>
        public bool HasPreviousPage => Page > 1;
        /// <summary> Есть следущая страница </summary>
        public bool HasNextPage => (Page < TotalPages);
        /// <summary> Есть позднее следущей страница </summary>
        public bool HasNextNextPage => (Page < TotalPages - 1);
        /// <summary> Есть последняя страница </summary>
        public bool HasLastPage => (Page < TotalPages - 2);
    }
}
