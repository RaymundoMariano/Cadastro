using Cadastro.Domain.Contracts.Repositories.Seedwork;
using Cadastro.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Repositories
{
    public interface IRepository<T> : IUnit where T : _Entity
    {
        Task<IEnumerable<T>> ObterAsync();
        Task<T> ObterAsync(int id);
        void Insere(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
