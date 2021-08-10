using System.Threading.Tasks;
using WebStore.Domain.WebModels;

namespace WebStore.Interfaces.Services
{
    public interface ICompareService
    {
        int Add(int id);
        (bool, CompareWebModel) AddAndGetWebModel(int id);
        void Clear();
        Task<CompareWebModel> GetWebModel();
    }
}