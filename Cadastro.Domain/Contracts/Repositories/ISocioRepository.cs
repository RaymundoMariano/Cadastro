using Cadastro.Domain.Entities;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Repositories
{
    public interface ISocioRepository : IRepository<Socio>
    {
        Task<Socio> GetFullAsync(int empresaId, string cpf);
    }
}
