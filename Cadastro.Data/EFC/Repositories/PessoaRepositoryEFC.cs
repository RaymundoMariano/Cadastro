using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Data.EFC.Repositories
{
    public class PessoaRepositoryEFC(CadastroContextEFC cadastroContext) : RepositoryEFC<Pessoa>(cadastroContext), IPessoaRepository
    {
        #region GetFullAsync
        public async Task<IEnumerable<Pessoa>> GetFullAsync()
        {
            return await _cadastroContext.Pessoas
                    .AsNoTracking()
                    .Include(p => p.PessoaFisicas)
                        .ThenInclude(p => p.Socios)
                            .ThenInclude(e => e.Empresa)
                    .Include(p => p.EnderecoPessoas)
                        .ThenInclude(p => p.Endereco)
                    .ToListAsync();
         }

        public async Task<Pessoa> GetFullAsync(int pessoaId)
        {
            return await _cadastroContext.Pessoas
                    .AsNoTracking()
                    .Include(p => p.PessoaFisicas)
                        .ThenInclude(p => p.Socios)
                            .ThenInclude(e => e.Empresa)
                    .Include(p => p.EnderecoPessoas)
                        .ThenInclude(p => p.Endereco)
                    .FirstOrDefaultAsync(p => p.PessoaId == pessoaId);
        }
        #endregion        
    }
}
