using Cadastro.Core.Domain.Models;
using Cadastro.Core.Domain.Repositories;
using Cadastro.Core.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadastro.Core.Persistence.Repositories
{
    public class PessoaJuridicaRepository : IPessoaJuridicaRepository
    {
        private readonly CadastroContext _cadastroContext;
        public IUnitOfWork UnitOfWork => _cadastroContext;

        public PessoaJuridicaRepository(CadastroContext cadastroContext)
        {
            _cadastroContext = cadastroContext;
        }

        #region ObterAsync
        public async Task<IEnumerable<PessoaJuridica>> ObterAsync()
        {
            return await _cadastroContext.PessoaJuridica
                    .AsNoTracking()
                    .Include(p => p.Endereco)
                    .Include(p => p.Socio)
                    .Include(p => p.FilialCgcNavigations)
                    .Include(p => p.FilialCgcMatrizNavigations).ToListAsync();
        }

        public async Task<PessoaJuridica> ObterAsync(long? cgc)
        {
            return await _cadastroContext.PessoaJuridica
                    .AsNoTracking()
                    .Include(p => p.Endereco)
                    .Include(p => p.Socio)
                    .Include(p => p.FilialCgcNavigations)
                    .Include(p => p.FilialCgcMatrizNavigations)
                    .FirstAsync(p => p.Cgc == cgc);
        }
        #endregion

        #region Insere
        public void Insere(PessoaJuridica pessoaJuridica)
        {
            _cadastroContext.PessoaJuridica.Add(pessoaJuridica);
        }
        #endregion

        #region Update
        public void Update(PessoaJuridica pessoaJuridica)
        {
            //_cadastroContext.PessoaJuridica.Update(pessoaJuridica);
            _cadastroContext.Entry(pessoaJuridica).State = EntityState.Modified;
        }
        #endregion

        #region Remove
        public void Remove(PessoaJuridica pessoaJuridica)
        {
            _cadastroContext.PessoaJuridica.Remove(pessoaJuridica);
        }
        #endregion

        #region Exists
        public bool Exists(long? cgc)
        {
            return _cadastroContext.PessoaJuridica.Any(e => e.Cgc == cgc);
        }
        #endregion
    }
}
