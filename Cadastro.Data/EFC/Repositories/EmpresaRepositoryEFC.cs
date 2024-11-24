using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadastro.Data.EFC.Repositories
{
    public class EmpresaRepositoryEFC(CadastroContextEFC cadastroContext) : RepositoryEFC<Empresa>(cadastroContext), IEmpresaRepository
    {
        #region GetFullAsync
        public async Task<IEnumerable<Empresa>> GetFullAsync()
        {
            return await _cadastroContext.Empresas
                    .AsNoTracking()
                    .Include(p => p.Endereco)
                    .Include(p => p.Socios)
                        .ThenInclude(pf => pf.PessoaFisica)
                            .ThenInclude(p => p.Pessoa)
                    .Include(p => p.Filiais)
                    .ToListAsync();
        }

        public async Task<Empresa> GetFullAsync(int empresaId)
        {
            return await _cadastroContext.Empresas
                    .AsNoTracking()
                    .Include(p => p.Endereco)
                    .Include(p => p.Socios)
                        .ThenInclude(pf => pf.PessoaFisica)
                            .ThenInclude(p => p.Pessoa)
                    .Include(p => p.Filiais)
                    .FirstAsync(p => p.EmpresaId == empresaId);
        }

        public async Task<Empresa> GetFullAsync(string cgc)
        {
            return await _cadastroContext.Empresas
                    .AsNoTracking()
                    .Include(p => p.Endereco)
                    .Include(p => p.Socios)
                        .ThenInclude(pf => pf.PessoaFisica)
                            .ThenInclude(p => p.Pessoa)
                    .Include(p => p.Filiais)
                    .FirstAsync(p => p.Cgc == cgc);
        }
        #endregion

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
