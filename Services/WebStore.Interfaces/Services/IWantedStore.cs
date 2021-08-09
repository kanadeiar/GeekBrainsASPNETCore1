using WebStore.Domain.Models;

namespace WebStore.Interfaces.Services
{
    public interface IWantedStore
    {
        Wanted Wanted { get; set; }
    }
}