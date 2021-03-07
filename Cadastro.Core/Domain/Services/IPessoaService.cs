using Cadastro.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Core.Domain.Services
{
    public interface IPessoaService
    {
        Task<IEnumerable<Pessoa>> ObterAsync();
        Task<Pessoa> ObterAsync(int pessoaId);
        Task InsereAsync(Pessoa pessoa);
        Task UpdateAsync(Pessoa pessoa);
        Task RemoveAsync(int pessoaId);
    }
}
