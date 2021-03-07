using Cadastro.Core.Domain.Models;
using Cadastro.Core.Domain.Repositories;
using Cadastro.Core.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadastro.Core.Persistence.Repositories
{
    public class FilialRepository : IFilialRepository
    {
        private readonly CadastroContext _cadastroContext;
        public IUnitOfWork UnitOfWork => _cadastroContext;

        public FilialRepository(CadastroContext cadastroContext)
        {
            _cadastroContext = cadastroContext;
        }

        #region ObterAsync
        public async Task<IEnumerable<Filial>> ObterAsync()
        {
            return await _cadastroContext.Filial
                    .AsNoTracking()
                    .Include(p => p.CgcNavigation)
                        .ThenInclude(p => p.Socio)
                        .ThenInclude(p => p.CpfNavigation)
                        .ThenInclude(p => p.Pessoa)
                        .ThenInclude(p => p.EnderecoPessoa)
                        .ThenInclude(p => p.Endereco)
                    .Include(p => p.CgcNavigation.Endereco)

                    .Include(p => p.CgcMatrizNavigation)
                        .ThenInclude(p => p.Socio)
                        .ThenInclude(p => p.CpfNavigation)
                        .ThenInclude(p => p.Pessoa)
                        .ThenInclude(p => p.EnderecoPessoa)
                        .ThenInclude(p => p.Endereco)
                    .Include(p => p.CgcMatrizNavigation.Endereco).ToListAsync();
        }

        public async Task<Filial> ObterAsync(long? cgc)
        {
            return await _cadastroContext.Filial
                    .AsNoTracking()
                    .Include(p => p.CgcNavigation)
                        .ThenInclude(p => p.Socio)
                        .ThenInclude(p => p.CpfNavigation)
                        .ThenInclude(p => p.Pessoa)
                        .ThenInclude(p => p.EnderecoPessoa)
                        .ThenInclude(p => p.Endereco)
                    .Include(p => p.CgcNavigation.Endereco)

                    .Include(p => p.CgcMatrizNavigation)
                        .ThenInclude(p => p.Socio)
                        .ThenInclude(p => p.CpfNavigation)
                        .ThenInclude(p => p.Pessoa)
                        .ThenInclude(p => p.EnderecoPessoa)
                        .ThenInclude(p => p.Endereco)
                    .Include(p => p.CgcMatrizNavigation.Endereco)
                    .FirstAsync(p => p.Cgc == cgc);
        }
        #endregion

        #region Insere
        public void Insere(Filial filial)
        {
            _cadastroContext.Filial.Add(filial);
        }
        #endregion

        #region Update
        public void Update(Filial filial)
        {
            _cadastroContext.Filial.Update(filial);
        }
        #endregion

        #region Remove
        public void Remove(Filial filial)
        {
            _cadastroContext.Filial.Remove(filial);
        }
        #endregion

        #region Exists
        public bool Exists(long? cgc)
        {
            return _cadastroContext.Filial.Any(e => e.Cgc == cgc);
        }
        #endregion
    }
}
