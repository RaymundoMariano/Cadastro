using Cadastro.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Services
{
    public interface IFilialService : IService<Filial>
    {
        Task<Filial> ObterAsync(string cgc);
    }
}
