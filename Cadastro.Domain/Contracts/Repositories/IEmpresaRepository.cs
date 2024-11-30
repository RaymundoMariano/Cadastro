using Cadastro.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Repositories
{
    public interface IEmpresaRepository : IRepository<Empresa>
    {
        Task<Empresa> GetFullAsync(string cgc);
        IEnumerable<Empresa> GetFiliaisSemVinculos();
    }
}
