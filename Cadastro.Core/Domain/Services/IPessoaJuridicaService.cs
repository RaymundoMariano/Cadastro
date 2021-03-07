using Cadastro.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Core.Domain.Services
{
    public interface IPessoaJuridicaService
    {
        Task<IEnumerable<PessoaJuridica>> ObterAsync();
        Task<PessoaJuridica> ObterAsync(long? cgc);
        Task InsereAsync(PessoaJuridica pessoaJuridica);
        Task UpdateAsync(PessoaJuridica pessoaJuridica);
        Task UpdateAsync(long? cgc, PessoaJuridica pessoaJuridica);
        Task RemoveAsync(long? cgc);
    }
}
