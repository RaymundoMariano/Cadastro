using Cadastro.Core.Domain.Models;
using Cadastro.Core.Domain.Repositories;
using Cadastro.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadastro.Core.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IEnderecoService _enderecoService;

        public PessoaService(IPessoaRepository pessoaRepository, IEnderecoService enderecoService)
        {
            _pessoaRepository = pessoaRepository;
            _enderecoService = enderecoService;
        }

        #region ObterAsync
        public async Task<IEnumerable<Pessoa>> ObterAsync()
        {
            try { return await _pessoaRepository.ObterAsync(); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Pessoa> ObterAsync(int pessoaId)
        {
            try
            {
                var pessoa = await _pessoaRepository.ObterAsync(pessoaId);
                if (pessoa == null)
                    throw new ServiceException("Pessoa não cadastrada - " + pessoaId);
                return pessoa;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(Pessoa pessoa)
        {
            try
            {
                if(pessoa.EnderecoPessoa != null)
                {
                    foreach(var endPessoa in pessoa.EnderecoPessoa.ToList())
                    {
                       await _enderecoService.InsereAsync(endPessoa.Endereco); 
                    }
                }
                _pessoaRepository.Insere(pessoa);
                await _pessoaRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(Pessoa pessoa)
        {
            try
            {
                if (pessoa.EnderecoPessoa != null)
                {
                    foreach (var endPessoa in pessoa.EnderecoPessoa.ToList())
                    {
                        await _enderecoService.UpdateAsync(endPessoa.Endereco);
                    }
                }
                _pessoaRepository.Update(pessoa);
                await _pessoaRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(int pessoaId)
        {
            try
            {
                var pessoa = await ObterAsync(pessoaId);

                List<int> end = new List<int>();

                if (pessoa.EnderecoPessoa != null)
                {
                    foreach (var endPessoa in pessoa.EnderecoPessoa.ToList())
                    {
                        end.Add(endPessoa.EnderecoId);
                    }
                }
                _pessoaRepository.Remove(pessoa);

                foreach(var enderecoId in end)
                {
                    await _enderecoService.RemoveAsync(enderecoId);
                }

                await _pessoaRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
