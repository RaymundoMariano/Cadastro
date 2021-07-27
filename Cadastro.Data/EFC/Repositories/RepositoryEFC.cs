using Microsoft.EntityFrameworkCore;
using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Contracts.Repositories.Seedwork;
using Cadastro.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Data.EFC.Repositories
{
    public class RepositoryEFC<T> : IRepository<T> where T : _Entity
    {
        protected readonly CadastroContextEFC _cadastroContext;
        public IUnitOfWork UnitOfWork => _cadastroContext;
        public RepositoryEFC(CadastroContextEFC cadastroContext)
        {
            _cadastroContext = cadastroContext;
        }
                        
        #region ObterAsync
        public async Task<IEnumerable<T>> ObterAsync()
        {
            return await _cadastroContext.Set<T>().ToListAsync();
        }

        public async Task<T> ObterAsync(int id)
        {
            return await _cadastroContext.Set<T>().FindAsync(id);
        }
        #endregion

        #region Insere
        public void Insere(T entity)
        {
            _cadastroContext.Set<T>().Add(entity);
        }
        #endregion

        #region Update
        public void Update(T entity)
        {
            _cadastroContext.Entry(entity).State = EntityState.Modified;
        }
        #endregion

        #region Remove
        public void Remove(T entity)
        {
            _cadastroContext.Set<T>().Remove(entity);
        }
        #endregion        
    }
}
