using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Contracts.UnitOfWorks;
using System;
using System.Threading.Tasks;

namespace Cadastro.Data.EFC
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CadastroContextEFC _context;
        public ICepRepository Ceps { get; }
        public IEmpresaRepository Empresas { get; }
        public IEnderecoPessoaRepository EnderecosPessoa { get; }
        public IEnderecoRepository Enderecos { get; }
        public IFilialRepository Filiais { get; }
        public IPessoaFisicaRepository PessoasFisicas { get; }
        public IPessoaRepository Pessoas { get; }
        public ISocioRepository Socios { get; }

        public UnitOfWork(CadastroContextEFC context
            , ICepRepository cepRepository
            , IEmpresaRepository empresaRepository
            , IEnderecoPessoaRepository enderecoPessoaRepository
            , IEnderecoRepository enderecoRepository
            , IFilialRepository filialRepository
            , IPessoaFisicaRepository pessoaFisicaRepository
            , IPessoaRepository pessoaRepository
            , ISocioRepository socioRepository)
        {
            _context = context;

            Ceps = cepRepository;
            Empresas = empresaRepository;
            EnderecosPessoa = enderecoPessoaRepository;
            Enderecos = enderecoRepository;
            Filiais = filialRepository;
            PessoasFisicas = pessoaFisicaRepository;
            Pessoas = pessoaRepository;
            Socios = socioRepository;
        }
        public async Task<int?> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
