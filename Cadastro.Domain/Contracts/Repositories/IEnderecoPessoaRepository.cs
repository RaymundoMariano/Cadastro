using Cadastro.Domain.Entities;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Repositories
{
    public interface IEnderecoPessoaRepository : IRepository<EnderecoPessoa>
    {
        Task<EnderecoPessoa> ObterAsync(int enderecoId, int pessoaId);
    }
}
