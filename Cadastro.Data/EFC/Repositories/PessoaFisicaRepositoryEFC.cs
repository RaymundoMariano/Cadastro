using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Data.EFC.Repositories
{
    public class PessoaFisicaRepositoryEFC(CadastroContextEFC cadastroContext) : RepositoryEFC<PessoaFisica>(cadastroContext), IPessoaFisicaRepository
    {
        #region GetFullAsync
        public async Task<IEnumerable<PessoaFisica>> GetFullAsync()
        {
            return await _cadastroContext.PessoaFisicas
                    .AsNoTracking()
                    .Include(p => p.Pessoa)
                    .Include(p => p.Socios)
                    .ToListAsync();
        }

        public async Task<PessoaFisica?> GetFullAsync(string cpf)
        {
            return await _cadastroContext.PessoaFisicas
                .AsNoTracking()
                .Include(p => p.Pessoa)
                .Include(p => p.Socios)
                .FirstOrDefaultAsync(p => p.Cpf == cpf);
        }

        public async Task<PessoaFisica?> GetFullAsync(int pessoaId)
        {
            return await _cadastroContext.PessoaFisicas
                .AsNoTracking()
                .Include(p => p.Pessoa)
                .Include(p => p.Socios)
                .FirstOrDefaultAsync(p => p.PessoaId == pessoaId);
        }
        #endregion
    }
}
