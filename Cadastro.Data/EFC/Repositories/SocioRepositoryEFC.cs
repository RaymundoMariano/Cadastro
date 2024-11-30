using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Data.EFC.Repositories
{
    public class SocioRepositoryEFC(CadastroContextEFC cadastroContext) : RepositoryEFC<Socio>(cadastroContext), ISocioRepository
    {
        #region GetFullAsync
        public async Task<Socio?> GetFullAsync(int empresaId, int pessoaFisicaId)
        {
            return await _cadastroContext.Socios
                    .AsNoTracking()
                    .Include(s => s.PessoaFisica)
                        .ThenInclude(s => s.Pessoa)
                    .FirstOrDefaultAsync(s => s.EmpresaId == empresaId && s.PessoaFisicaId == pessoaFisicaId);
        }
        #endregion
    }
}
