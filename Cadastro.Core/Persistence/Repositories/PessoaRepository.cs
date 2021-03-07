using Cadastro.Core.Domain.Models;
using Cadastro.Core.Domain.Repositories;
using Cadastro.Core.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadastro.Core.Persistence.Repositories
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly CadastroContext _cadastroContext;
        public IUnitOfWork UnitOfWork => _cadastroContext;

        public PessoaRepository(CadastroContext cadastroContext) 
        {
            _cadastroContext = cadastroContext;
        }

        #region ObterAsync
        public async Task<IEnumerable<Pessoa>> ObterAsync()
        {
            //return await _cadastroContext.Pessoa.ToListAsync();
            return await _cadastroContext.Pessoa
                    .AsNoTracking()
                    .Include(p => p.PessoaFisica)
                        .ThenInclude(p => p.Socio)
                            .ThenInclude(p => p.CgcNavigation)
                                .ThenInclude(p => p.Endereco)
                    .Include(p => p.EnderecoPessoa)
                        .ThenInclude(p => p.Endereco).ToListAsync();
         }

        public async Task<Pessoa> ObterAsync(int pessoaId)
        {
            //return await _cadastroContext.Pessoa.FindAsync(pessoaId);
            return await _cadastroContext.Pessoa
                    .AsNoTracking()
                    .Include(p => p.PessoaFisica)
                        .ThenInclude(p => p.Socio)
                            .ThenInclude(p => p.CgcNavigation)
                                .ThenInclude(p => p.Endereco)
                    .Include(p => p.EnderecoPessoa)
                        .ThenInclude(p => p.Endereco)
                    .FirstAsync(p => p.PessoaId == pessoaId);
        }
        #endregion

        #region Insere
        public void Insere(Pessoa pessoa)
        {
            _cadastroContext.Pessoa.Add(pessoa);
        }
        #endregion

        #region Update
        public void Update(Pessoa pessoa)
        {
            _cadastroContext.Pessoa.Update(pessoa);
        }
        #endregion

        #region Remove
        public void Remove(Pessoa pessoa)
        {
            _cadastroContext.Pessoa.Remove(pessoa);
        }
        #endregion

        #region Exists
        public bool Exists(int pessoaId)
        {
            return _cadastroContext.Pessoa.Any(e => e.PessoaId == pessoaId);
        }
        #endregion
    }
}
