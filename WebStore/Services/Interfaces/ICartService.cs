using WebStore.WebModels;

namespace WebStore.Services.Interfaces
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
