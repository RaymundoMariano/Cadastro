using Cadastro.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Repositories
{
    public interface IPessoaFisicaRepository : IRepository<PessoaFisica>
    {
        Task<IEnumerable<PessoaFisica>> GetFullAsync();
        Task<PessoaFisica> GetFullAsync(string cpf);
        Task<PessoaFisica> GetFullAsync(int pessoaId);
    }
}
