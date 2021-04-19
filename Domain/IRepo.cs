using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public interface IRepo<T>
    {
        Task<List<T>> Get();
        Task<T> Get(int? id);
        Task Delete(int? id);
        Task Add(T obj);
        Task Update(T obj);
        T GetById(int? id);
    }
}
