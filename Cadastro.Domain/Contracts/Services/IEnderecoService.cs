using Cadastro.Domain.Entities;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Services
{
    public interface IEnderecoService : IService<Endereco>
    {
        Task ManterEnderecoPessoaAsync(int pessoaId, Endereco endereco);
        Task ManterEnderecoEmpresaAsync(int empresaId, Endereco endereco);
    }
}
