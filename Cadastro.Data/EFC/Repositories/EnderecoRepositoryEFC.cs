using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Data.EFC.Repositories
{
    public class EnderecoRepositoryEFC(CadastroContextEFC cadastroContext) : RepositoryEFC<Endereco>(cadastroContext), IEnderecoRepository
    {
        #region GetFullAsync
        public async Task<IEnumerable<Endereco>> GetFullAsync()
        {
            return await _cadastroContext.Enderecos
                    .AsNoTracking()
                    .Include(e => e.EnderecoPessoas)
                        .ThenInclude(e => e.Pessoa)
                    .ToListAsync();
        }

        public async Task<Endereco?> GetFullAsync(int enderecoId)
        {
            return await _cadastroContext.Enderecos
                    .AsNoTracking()
                    .Include(e => e.EnderecoPessoas)
                        .ThenInclude(e => e.Pessoa)
                    .FirstOrDefaultAsync(e => e.EnderecoId == enderecoId);
        }
        #endregion
    }
}
