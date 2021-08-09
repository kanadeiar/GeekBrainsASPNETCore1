using System.Threading.Tasks;
using WebStore.Domain.WebModels;

namespace WebStore.Interfaces.Services
{
    public interface IWantedService
    {
        void Add(int id);
        void Remove(int id);
        void Clear();
        Task<WantedWebModel> GetWebModel();
    }
}