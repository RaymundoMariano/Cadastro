using Cadastro.Domain.Entities;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Services
{
    public interface ISocioService : IService<Socio>
    {
        Task<Socio> ObterAsync(int empresaId, string cpf);
    }
}
