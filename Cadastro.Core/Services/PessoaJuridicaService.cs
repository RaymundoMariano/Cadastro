using Cadastro.Core.Domain.Models;
using Cadastro.Core.Domain.Repositories;
using Cadastro.Core.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Core.Services
{
    public class PessoaJuridicaService : IPessoaJuridicaService
    {
        private readonly IPessoaJuridicaRepository _pessoaJuridicaRepository;
        private readonly IPessoaFisicaService _pessoaFisicaService;
        private readonly IEnderecoService _enderecoService;

        public PessoaJuridicaService(IPessoaJuridicaRepository pessoaJuridicaRepository
            , IPessoaFisicaService pessoaFisicaService
            , IEnderecoService enderecoService)
        {
            _pessoaJuridicaRepository = pessoaJuridicaRepository;
            _pessoaFisicaService = pessoaFisicaService;
            _enderecoService = enderecoService;
        }

        #region ObterAsync
        public async Task<IEnumerable<PessoaJuridica>> ObterAsync()
        {
            try { return await _pessoaJuridicaRepository.ObterAsync(); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<PessoaJuridica> ObterAsync(long? cgc)
        {
            try
            {
                var pessoaJuridica = await _pessoaJuridicaRepository.ObterAsync(cgc);
                if (pessoaJuridica == null)
                {
                    throw new ServiceException("PessoaJuridica não cadastrada - " + cgc);
                }
                return pessoaJuridica;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(PessoaJuridica pessoaJuridica)
        {
            try
            {
                if(pessoaJuridica.Socio != null)
                {
                    foreach(var socio in pessoaJuridica.Socio)
                    {
                       var pessoaFisica = await _pessoaFisicaService.ObterAsync(socio.Cpf);
                    }
                }

                if (pessoaJuridica.Endereco != null)
                {
                    await _enderecoService.InsereAsync(pessoaJuridica.Endereco);
                }
                _pessoaJuridicaRepository.Insere(pessoaJuridica);
                await _pessoaJuridicaRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(PessoaJuridica pessoaJuridica)
        {
            try
            {
                if (pessoaJuridica.Endereco != null)
                {
                    await _enderecoService.UpdateAsync(pessoaJuridica.Endereco);
                }
                _pessoaJuridicaRepository.Update(pessoaJuridica);
                await _pessoaJuridicaRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task UpdateAsync(long? cgc, PessoaJuridica pessoaJuridica)
        {
            try
            {
                if (cgc != pessoaJuridica.Cgc) 
                    { throw new ServiceException(cgc + " Diferente " + pessoaJuridica.Cgc); }

                if (pessoaJuridica.Endereco != null)
                {
                    await _enderecoService.UpdateAsync(pessoaJuridica.Endereco);
                }

                _pessoaJuridicaRepository.Update(pessoaJuridica);
                try { await _pessoaJuridicaRepository.UnitOfWork.SaveChangesAsync(); }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_pessoaJuridicaRepository.Exists(cgc))
                    {
                        throw new ServiceException("Empresa não encontrada - " + cgc);
                    }
                    throw;
                }
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(long? cgc)
        {
            try
            {
                var pessoaJuridica = await ObterAsync(cgc);
                if (pessoaJuridica == null)
                    throw new ServiceException("Empresa não cadastrada - " + cgc);

                _pessoaJuridicaRepository.Remove(pessoaJuridica);

                await _enderecoService.RemoveAsync(pessoaJuridica.Endereco.EnderecoId);

                await _pessoaJuridicaRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
