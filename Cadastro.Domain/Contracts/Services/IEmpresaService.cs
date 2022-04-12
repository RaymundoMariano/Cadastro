using Cadastro.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Services
{
    public interface IEmpresaService : IService<Empresa>
    {
        Task<Empresa> ObterAsync(string cgc);
        Task<IEnumerable<Filial>> GetFiliais(int empresaId);
        Task ManterFiliaisAsync(int empresaId, List<Filial> filiais);
        Task<IEnumerable<Pessoa>> GetSocios(int empresaId);
        Task ManterSociosAsync(int empresaId, List<Pessoa> pessoas);
    }
}
