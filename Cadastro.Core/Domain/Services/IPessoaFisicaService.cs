using Cadastro.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Core.Domain.Services
{
    public interface IPessoaFisicaService
    {
        Task<IEnumerable<PessoaFisica>> ObterAsync();
        Task<PessoaFisica> ObterAsync(long? cpf);
    }
}
