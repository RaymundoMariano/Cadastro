using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Cadastro.Data.EFC.Repositories
{
    public class CepRepositoryEFC : RepositoryEFC<Cep>, ICepRepository
    {
        public CepRepositoryEFC(CadastroContextEFC cadastroContext) : base(cadastroContext)
        {
        }

        #region ObterAsync
        public async Task<Cep> ObterAsync(string cep)
        {
            return await _cadastroContext.Ceps.FindAsync(cep);
        }
        #endregion        
    }
}
