using Cadastro.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Core.Domain.Repositories
{
    public interface IPessoaRepository : IRepository
    {
        Task<IEnumerable<Pessoa>> ObterAsync();
        Task<Pessoa> ObterAsync(int pessoaId);
        void Insere (Pessoa pessoa);
        void Update(Pessoa pessoa);
        void Remove(Pessoa pessoa);
        bool Exists(int pessoaId);
    }
}
