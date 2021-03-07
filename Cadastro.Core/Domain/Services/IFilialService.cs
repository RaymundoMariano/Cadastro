using Cadastro.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Core.Domain.Services
{
    public interface IFilialService
    {
        Task<IEnumerable<Filial>> ObterAsync();
        Task<Filial> ObterAsync(long? cgc);
        Task InsereAsync(Filial filial);
        Task UpdateAsync(long? cgc, Filial filial);
        Task RemoveAsync(long? cgc);
    }
}
