using Cadastro.Core.Domain.Models;
using Cadastro.Core.Domain.Repositories;
using Cadastro.Core.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadastro.Core.Persistence.Repositories
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly CadastroContext _cadastroContext;
        public IUnitOfWork UnitOfWork => _cadastroContext;

        public EnderecoRepository(CadastroContext cadastroContext)
        {
            _cadastroContext = cadastroContext;
        }

        #region ObterAsync
        public async Task<IEnumerable<Endereco>> ObterAsync()
        {
            return await _cadastroContext.Endereco.ToListAsync();
        }

        public async Task<Endereco> ObterAsync(int enderecoId)
        {
            return await _cadastroContext.Endereco.FindAsync(enderecoId);
        }
        #endregion

        #region Insere
        public void Insere(Endereco endereco)
        {
            _cadastroContext.Endereco.Add(endereco);
        }
        #endregion

        #region Update
        public void Update(Endereco endereco)
        {
            _cadastroContext.Endereco.Update(endereco);
        }
        #endregion

        #region Remove
        public void Remove(Endereco endereco)
        {
            _cadastroContext.Endereco.Remove(endereco);
        }
        #endregion

        #region Exists
        public bool Exists(int enderecoId)
        {
            return _cadastroContext.Endereco.Any(e => e.EnderecoId == enderecoId);
        }
        #endregion
    }
}
