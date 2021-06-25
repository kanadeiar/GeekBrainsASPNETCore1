using System.Collections.Generic;

namespace WebStore.Interfaces.WebAPI
{
    public interface IValuesService
    {
        IEnumerable<string> GetAll();
        string GetById(int id);
        void Add(string str);
        void Edit(int id, string str);
        bool Delete(int id);
    }
}
