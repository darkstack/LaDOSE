using System.Collections.Generic;

namespace LaDOSE.Business.Interface
{
    public interface IBaseService<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        T Create(T entity);
        bool Update(T entity);
        bool Delete(int id);
    }
}