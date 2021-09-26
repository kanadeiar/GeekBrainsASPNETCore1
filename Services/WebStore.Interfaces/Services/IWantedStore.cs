using WebStore.Domain.Models;

namespace WebStore.Interfaces.Services
{
    /// <summary> Хранилище для желаемых товаров </summary>
    public interface IWantedStore
    {
        /// <summary> Желаемые товары </summary>
        Wanted Wanted { get; set; }
    }
}