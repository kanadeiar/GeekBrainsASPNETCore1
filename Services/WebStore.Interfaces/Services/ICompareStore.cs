using WebStore.Domain.Models;

namespace WebStore.Interfaces.Services
{
    /// <summary> Хранилище товаров для сравнения </summary>
    public interface ICompareStore
    {
        /// <summary> Сравниваемые товары </summary>
        Compare Compare { get; set; }
    }
}