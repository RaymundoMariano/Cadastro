using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Data.EFC.Repositories
{
    public class EmpresaRepositoryEFC(CadastroContextEFC cadastroContext) : RepositoryEFC<Empresa>(cadastroContext), IEmpresaRepository
    {
        public override async Task<IEnumerable<Empresa>> ObterAsync()
        {
            return await _cadastroContext.Empresas
                    .AsNoTracking()
                    .Include(p => p.Endereco)
                    .Include(p => p.Socios).ThenInclude(pf => pf.PessoaFisica).ThenInclude(p => p.Pessoa)
                    .Include(p => p.Filiais)
                    .ToListAsync();
        }

        public override async Task<Empresa?> ObterAsync(int empresaId)
        {
            return await _cadastroContext.Empresas
                    .AsNoTracking().Include(p => p.Endereco)
                    .Include(p => p.Socios).ThenInclude(pf => pf.PessoaFisica).ThenInclude(p => p.Pessoa)
                    .Include(p => p.Filiais)
                    .FirstAsync(p => p.EmpresaId == empresaId);
        }

        public async Task<Empresa> GetFullAsync(string cgc)
        {
            return await _cadastroContext.Empresas
                    .AsNoTracking()
                    .Include(p => p.Endereco)
                    .Include(p => p.Socios) .ThenInclude(pf => pf.PessoaFisica).ThenInclude(p => p.Pessoa)
                    .Include(p => p.Filiais)
                    .FirstAsync(p => p.Cgc == cgc);
        }

        #region GetFiliaisSemVinculos
        public IEnumerable<Empresa> GetFiliaisSemVinculos()
        {
            return _cadastroContext.Empresas.ToList()
                .Where(e => _cadastroContext.Filiais.All(f => f.EmpresaId == e.EmpresaId)
                    && (ETipoEmpresa)e.Tipo == ETipoEmpresa.Filial)
                .ToList();
        }
        #endregion        
    }
}
