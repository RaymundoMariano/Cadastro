using Cadastro.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Repositories
{
    public interface IFilialRepository : IRepository<Filial>
    {
        Task<IEnumerable<Filial>> GetFullAsync();
        Task<Filial> GetFullAsync(int filialId);
        Task<Filial> GetFullAsync(string cgc);
    }
}
