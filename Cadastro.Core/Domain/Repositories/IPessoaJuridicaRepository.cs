using Cadastro.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Core.Domain.Repositories
{
    public interface IPessoaJuridicaRepository : IRepository
    {
        Task<IEnumerable<PessoaJuridica>> ObterAsync();
        Task<PessoaJuridica> ObterAsync(long? cgc);
        void Insere(PessoaJuridica pessoaJuridica);
        void Update(PessoaJuridica pessoaJuridica);
        void Remove(PessoaJuridica pessoaJuridica);
        bool Exists(long? cgc);
    }
}
