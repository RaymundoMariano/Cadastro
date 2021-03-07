using Cadastro.Core.Domain.Models;
using Cadastro.Core.Domain.Repositories;
using Cadastro.Core.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadastro.Core.Persistence.Repositories
{
    public class PessoaFisicaRepository : IPessoaFisicaRepository
    {
        private readonly CadastroContext _cadastroContext;
        public IUnitOfWork UnitOfWork => _cadastroContext;

        public PessoaFisicaRepository(CadastroContext cadastroContext)
        {
            _cadastroContext = cadastroContext;
        }

        #region ObterAsync
        public async Task<IEnumerable<PessoaFisica>> ObterAsync()
        {
            return await _cadastroContext.PessoaFisica
                    .AsNoTracking()
                    .Include(p => p.Pessoa).ToListAsync();
        }

        public async Task<PessoaFisica> ObterAsync(long? cpf)
        {
            return await _cadastroContext.PessoaFisica
                .AsNoTracking()
                .Include(p => p.Pessoa)
                .FirstAsync(p => p.Cpf == cpf);
        }
        #endregion

        #region Exists
        public bool Exists(long? cpf)
        {
            return _cadastroContext.PessoaFisica.Any(e => e.Cpf == cpf);
        }
        #endregion
    }
}
