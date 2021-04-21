using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain
{
    public interface IRepo<T>
    {
        string ErrorMessage { get; }
        public T EntityInDb { get; }
        Task<List<T>> Get();
        Task<T> Get(int? id);
        Task<bool> Delete(int? id);
        Task<bool> Add(T obj);
        Task <bool> Update(T obj);
        T GetById(int? id);
    }
}
