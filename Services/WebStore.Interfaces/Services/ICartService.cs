using WebStore.Domain.WebModels.Cart;

namespace WebStore.Interfaces.Services
{
    public interface ICartService
    {
        void Add(int id);
        void Subtract(int id);
        void Remove(int id);
        void Clear();
        CartWebModel GetWebModel();
    }
}
