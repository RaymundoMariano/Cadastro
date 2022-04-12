using Cadastro.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Repositories
{
    public interface IRepository<T> where T : _Entity
    {
        Task<IEnumerable<T>> ObterAsync();
        Task<T> ObterAsync(int id);
        Task InsereAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
