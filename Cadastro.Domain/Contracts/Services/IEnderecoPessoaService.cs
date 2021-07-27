using Cadastro.Domain.Entities;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Services
{
    public interface IEnderecoPessoaService : IService<EnderecoPessoa>
    {
        Task ManterAsync(int pessoaId, Endereco endereco);
    }
}
