using Cadastro.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Core.Domain.Repositories
{
    public interface IPessoaFisicaRepository : IRepository
    {
        Task<IEnumerable<PessoaFisica>> ObterAsync();
        Task<PessoaFisica> ObterAsync(long? cpf);
        bool Exists(long? cpf);
    }
}
