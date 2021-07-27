using Cadastro.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Services
{
    public interface IPessoaFisicaService
    {
        Task<IEnumerable<PessoaFisica>> ObterAsync();
        Task<PessoaFisica> ObterAsync(string cpf);
        Task InsereAsync(PessoaFisica pf);
        Task RemoveAsync(string cpf);
    }
}
