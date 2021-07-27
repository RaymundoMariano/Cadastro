using Cadastro.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Services
{
    public interface IService<T> where T : _Entity
    {
        Task<IEnumerable<T>> ObterAsync();
        Task<T> ObterAsync(int id);
        Task InsereAsync(T entity);
        Task UpdateAsync(int id, T entity);
        Task RemoveAsync(int id);
    }
}
