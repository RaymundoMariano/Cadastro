using Cadastro.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Core.Domain.Repositories
{
    public interface IFilialRepository : IRepository
    {
        Task<IEnumerable<Filial>> ObterAsync();
        Task<Filial> ObterAsync(long? cgc);
        void Insere(Filial filial);
        void Update(Filial filial);
        void Remove(Filial filial);
        bool Exists(long? cgc);
    }
}
