using Cadastro.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Core.Domain.Services
{
    public interface IEnderecoService
    {
        Task<IEnumerable<Endereco>> ObterAsync();
        Task<Endereco> ObterAsync(int enderecoId);
        Task InsereAsync(Endereco endereco);
        Task UpdateAsync(Endereco endereco);
        Task RemoveAsync(int enderecoId);
    }
}
