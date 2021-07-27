using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadastro.Data.EFC.Repositories
{
    public class PessoaRepositoryEFC : RepositoryEFC<Pessoa>, IPessoaRepository
    {
        public PessoaRepositoryEFC(CadastroContextEFC cadastroContext) : base(cadastroContext)
        {
        }

        #region GetFullAsync
        public async Task<IEnumerable<Pessoa>> GetFullAsync()
        {
            return await _cadastroContext.Pessoas
                    .AsNoTracking()
                    .Include(p => p.PessoaFisicas)
                        .ThenInclude(p => p.Socios)
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
                    .Include(p => p.EnderecoPessoas)
                        .ThenInclude(p => p.Endereco)
                    .FirstOrDefaultAsync(p => p.PessoaId == pessoaId);
        }
        #endregion

        #region GetPessoasSemVinculos
        public IEnumerable<Pessoa> GetPessoasSemVinculos(int empresaId)
        {
            return _cadastroContext.Pessoas.ToList()
                .Where(p => _cadastroContext.Socios.All(s => 
                    s.EmpresaId != empresaId &&
                    s.PessoaFisica.PessoaId != p.PessoaId))
                .ToList();
        }
        #endregion
    }
}
