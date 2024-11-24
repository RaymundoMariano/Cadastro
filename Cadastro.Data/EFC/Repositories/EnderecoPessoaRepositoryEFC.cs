using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Cadastro.Data.EFC.Repositories
{
    public class EnderecoPessoaRepositoryEFC(CadastroContextEFC cadastroContext) : RepositoryEFC<EnderecoPessoa>(cadastroContext), IEnderecoPessoaRepository
    {
        #region ObterAsync
        public async Task<EnderecoPessoa> ObterAsync(int enderecoId, int pessoaId)
        {
            return await _cadastroContext.EnderecoPessoas.FirstOrDefaultAsync(ep =>
                ep.EnderecoId == enderecoId &&
                ep.PessoaId == pessoaId);
        }
        #endregion        
    }
}
