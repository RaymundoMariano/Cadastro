﻿using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Data.EFC.Repositories
{
    public class PessoaFisicaRepositoryEFC : RepositoryEFC<PessoaFisica>, IPessoaFisicaRepository
    {
        public PessoaFisicaRepositoryEFC(CadastroContextEFC cadastroContext) : base(cadastroContext)
        {
        }

        #region GetFullAsync
        public async Task<IEnumerable<PessoaFisica>> GetFullAsync()
        {
            return await _cadastroContext.PessoaFisicas
                    .AsNoTracking()
                    .Include(p => p.Pessoa)
                    .Include(p => p.Socios)
                    .ToListAsync();
        }

        public async Task<PessoaFisica> GetFullAsync(string cpf)
        {
            return await _cadastroContext.PessoaFisicas
                .AsNoTracking()
                .Include(p => p.Pessoa)
                .Include(p => p.Socios)
                .FirstOrDefaultAsync(p => p.Cpf == cpf);
        }

        public async Task<PessoaFisica> GetFullAsync(int pessoaId)
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
