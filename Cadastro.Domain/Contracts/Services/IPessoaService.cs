using Cadastro.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Services
{
    public interface IPessoaService : IService<Pessoa>
    {
        Task<IEnumerable<Pessoa>> ObterSemVinculos(int empresaId);
        Task<IEnumerable<Pessoa>> FormateCPF(IEnumerable<Pessoa> pessoas);
    }
}
