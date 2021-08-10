using WebStore.Domain.Models;

namespace WebStore.Interfaces.Services
{
    public interface ICompareStore
    {
        /// <summary> Сравниваемые товары </summary>
        Compare Compare { get; set; }
    }
}