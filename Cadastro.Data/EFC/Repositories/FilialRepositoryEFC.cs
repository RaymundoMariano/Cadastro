using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Data.EFC.Repositories
{
    public class FilialRepositoryEFC : RepositoryEFC<Filial>, IFilialRepository
    {
        public FilialRepositoryEFC(CadastroContextEFC cadastroContext) : base(cadastroContext)
        {
        }

        #region GetFullAsync
        public async Task<IEnumerable<Filial>> GetFullAsync()
        {
            return await _cadastroContext.Filiais
                    .AsNoTracking()
                    .Include(p => p.Empresa)
                    .ToListAsync();
        }

        public async Task<Filial> GetFullAsync(int filialId)
        {
            return await _cadastroContext.Filiais
                    .AsNoTracking()
                    .Include(p => p.Empresa)
                    .FirstOrDefaultAsync(p => p.FilialId == filialId);
        }

        public async Task<Filial> GetFullAsync(string cgc)
        {
            return await _cadastroContext.Filiais
                    .AsNoTracking()
                    .Include(p => p.Empresa)
                    .FirstOrDefaultAsync(p => p.Cgc == cgc);
        }
        #endregion        
    }
}
