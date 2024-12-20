﻿using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Data.EFC.Repositories
{
    public class FilialRepositoryEFC(CadastroContextEFC cadastroContext) : RepositoryEFC<Filial>(cadastroContext), IFilialRepository
    {
        #region GetFullAsync
        public async Task<IEnumerable<Filial>> GetFullAsync()
        {
            return await _cadastroContext.Filiais
                    .AsNoTracking()
                    .Include(p => p.Empresa)
                    .ToListAsync();
        }

        public async Task<Filial?> GetFullAsync(int filialId)
        {
            return await _cadastroContext.Filiais
                    .AsNoTracking()
                    .Include(p => p.Empresa)
                    .FirstOrDefaultAsync(p => p.FilialId == filialId);
        }

        public async Task<Filial?> GetFullAsync(string cgc)
        {
            return await _cadastroContext.Filiais
                    .AsNoTracking()
                    .Include(p => p.Empresa)
                    .FirstOrDefaultAsync(p => p.Cgc == cgc);
        }
        #endregion        
    }
}
