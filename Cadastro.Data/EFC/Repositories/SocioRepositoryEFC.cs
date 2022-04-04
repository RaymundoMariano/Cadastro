using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Cadastro.Data.EFC.Repositories
{
    public class SocioRepositoryEFC : RepositoryEFC<Socio>, ISocioRepository
    {
        public SocioRepositoryEFC(CadastroContextEFC cadastroContext) : base(cadastroContext)
        {
        }

        #region GetFullAsync
        public async Task<Socio> GetFullAsync(int empresaId, string cpf)
        {
            return await _cadastroContext.Socios
                    .AsNoTracking()
                    .Include(s => s.PessoaFisica)
                        .ThenInclude(s => s.Pessoa)
                    .FirstOrDefaultAsync(s => s.EmpresaId == empresaId && s.Cpf == cpf);
        }
        #endregion
    }
}
