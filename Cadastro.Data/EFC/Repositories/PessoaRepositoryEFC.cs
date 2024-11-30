using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Data.EFC.Repositories
{
    public class PessoaRepositoryEFC(CadastroContextEFC cadastroContext) : RepositoryEFC<Pessoa>(cadastroContext), IPessoaRepository
    {
        #region ObterAsync
        public override async Task<IEnumerable<Pessoa>> ObterAsync()
        {
            return await _cadastroContext.Pessoas
                    .AsNoTracking()
                    .Include(p => p.PessoaFisicas).ThenInclude(p => p.Socios).ThenInclude(e => e.Empresa)
                    .Include(p => p.EnderecoPessoas).ThenInclude(p => p.Endereco)
                    .ToListAsync();
         }

        public override async Task<Pessoa?> ObterAsync(int pessoaId)
        {
            return await _cadastroContext.Pessoas
                    .AsNoTracking()
                    .Include(p => p.PessoaFisicas).ThenInclude(p => p.Socios).ThenInclude(e => e.Empresa)
                    .Include(p => p.EnderecoPessoas).ThenInclude(p => p.Endereco)
                    .FirstOrDefaultAsync(p => p.PessoaId == pessoaId);
        }
        #endregion        
    }
}
