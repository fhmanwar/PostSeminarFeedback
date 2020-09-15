using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Interface
{
    public interface IRepo<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> GetID(int Id);
        Task<int> Create(T entity);
        Task<int> Update(T entity);
        Task<int> Delete(int Id);
    }
}
