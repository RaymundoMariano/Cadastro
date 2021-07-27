using Cadastro.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Services
{
    public interface IEmpresaService : IService<Empresa>
    {
        Task<Empresa> ObterAsync(string cgc);
        Task ManterEnderecoAsync(int empresaId, Endereco endereco);
        Task<IEnumerable<Empresa>> GetFiliais(int empresaId);
        Task<IEnumerable<Pessoa>> GetSocios(int empresaId);
        IEnumerable<Empresa> FormateCGC(IEnumerable<Empresa> empresas);
    }
}
