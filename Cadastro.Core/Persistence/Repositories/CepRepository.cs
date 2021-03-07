using Cadastro.Core.Domain.Models;
using Cadastro.Core.Domain.Repositories;
using Cadastro.Core.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadastro.Core.Persistence.Repositories
{
    public class CepRepository : ICepRepository
    {
        private readonly CadastroContext _cadastroContext;
        public IUnitOfWork UnitOfWork => _cadastroContext;

        public CepRepository(CadastroContext cadastroContext)
        {
            _cadastroContext = cadastroContext;
        }

        #region ObterAsync
        public async Task<IEnumerable<Cep>> ObterAsync()
        {
            return await _cadastroContext.Cep.ToListAsync();
        }

        public async Task<Cep> ObterAsync(long? cep)
        {
            return await _cadastroContext.Cep.FindAsync(cep);
        }
        #endregion

        #region Insere
        public void Insere(Cep cep)
        {
            _cadastroContext.Cep.Add(cep);
        }
        #endregion

        #region Exists
        public bool Exists(long? cep)
        {
            return _cadastroContext.Cep.Any(e => e.CEP == cep);
        }
        #endregion
    }
}
