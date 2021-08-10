using System.Threading.Tasks;
using WebStore.Domain.WebModels;

namespace WebStore.Interfaces.Services
{
    public interface ICompareService
    {
        bool IsMoreOne { get; }
        void Add(int id);
        void Clear();
        Task<CompareWebModel> GetWebModel();
    }
}