using Cadastro.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Core.Domain.Services
{
    public interface ICepService
    {
        Task<IEnumerable<Cep>> ObterAsync();
        Task<Cep> ObterAsync(long? cep);
        Task InsereAsync(Cep cep);
    }
}
