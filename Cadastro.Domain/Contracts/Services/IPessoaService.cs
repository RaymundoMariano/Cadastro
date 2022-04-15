using Cadastro.Domain.Entities;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Services
{
    public interface IPessoaService : IService<Pessoa>
    {
        Task ManterEnderecoAsync(int pessoaId, Endereco endereco);
    }
}
