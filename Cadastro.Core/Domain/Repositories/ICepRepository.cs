using Cadastro.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Core.Domain.Repositories
{
    public interface ICepRepository : IRepository
    {
        Task<IEnumerable<Cep>> ObterAsync();
        Task<Cep> ObterAsync(long? cep);
        void Insere(Cep cep);
        bool Exists(long? cep);
    }
}
