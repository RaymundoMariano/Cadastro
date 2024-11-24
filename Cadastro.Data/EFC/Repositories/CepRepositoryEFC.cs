using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Entities;
using System.Threading.Tasks;

namespace Cadastro.Data.EFC.Repositories
{
    public class CepRepositoryEFC(CadastroContextEFC cadastroContext) : RepositoryEFC<Cep>(cadastroContext), ICepRepository
    {
        #region ObterAsync
        public async Task<Cep> ObterAsync(string cep)
        {
            return await _cadastroContext.Ceps.FindAsync(cep);
        }
        #endregion        
    }
}
