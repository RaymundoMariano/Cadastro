using Microsoft.EntityFrameworkCore;
using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Entities;

namespace Cadastro.Data.EFC.Repositories
{
    public class RepositoryEFC<T>(CadastroContextEFC cadastroContext) : IRepository<T> where T : _Entity
    {
        protected readonly CadastroContextEFC _cadastroContext = cadastroContext;

        #region ObterAsync
        public virtual async Task<IEnumerable<T>> ObterAsync()
        {
            return await _cadastroContext.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> ObterAsync(int id)
        {
            return await _cadastroContext.Set<T>().FindAsync(id);
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(T entity)
        {
            await _cadastroContext.Set<T>().AddAsync(entity);
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
