using Cadastro.Domain.Contracts.Repositories;
using System;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        ICepRepository Ceps { get; }
        IEmpresaRepository Empresas { get; }
        IEnderecoPessoaRepository EnderecosPessoa { get; }
        IEnderecoRepository Enderecos { get; }
        IFilialRepository Filiais { get; }
        IPessoaFisicaRepository PessoasFisicas { get; }
        IPessoaRepository Pessoas { get; }
        ISocioRepository Socios { get; }
        Task<int?> SaveChangesAsync();
    }
}
