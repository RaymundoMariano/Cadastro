using Cadastro.Core.Domain.Enums;
using Cadastro.Core.Domain.Models;
using Cadastro.Core.Domain.Repositories;
using Cadastro.Core.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Core.Services
{
    public class FilialService : IFilialService
    {
        private readonly IFilialRepository _filialRepository;
        private readonly IPessoaJuridicaService _pessoaJuridicaService;

        public FilialService(IFilialRepository filialRepository, IPessoaJuridicaService pessoaJuridicaService)
        {
            _filialRepository = filialRepository;
            _pessoaJuridicaService = pessoaJuridicaService;
        }

        #region ObterAsync
        public async Task<IEnumerable<Filial>> ObterAsync()
        {
            try { return await _filialRepository.ObterAsync(); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Filial> ObterAsync(long? cgc)
        {
            try
            {
                var filial = await _filialRepository.ObterAsync(cgc);
                if (filial == null)
                {
                    throw new ServiceException("Filial não cadastrada - " + cgc);
                }
                return filial;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(Filial filial)
        {
            try
            {
                var PJ = await _pessoaJuridicaService.ObterAsync(filial.Cgc);
                if(PJ.TpEmpresa == (int)ETpEmpresa.Matriz)
                {
                    throw new ServiceException("A empresa é matriz! Não pode ser registrada como filial - " + filial.Cgc);
                }

                PJ = await _pessoaJuridicaService.ObterAsync(filial.CgcMatriz);
                if (PJ.TpEmpresa == (int)ETpEmpresa.Filial)
                {
                    throw new ServiceException("A empresa é filial! Não pode ser registrada como matriz - " + filial.CgcMatriz);
                }

                _filialRepository.Insere(filial);
                await _filialRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(long? cgc, Filial filial)
        {
            try
            {
                if (cgc != filial.Cgc)
                { 
                    throw new ServiceException(cgc + " Diferente " + filial.Cgc); 
                }

                _filialRepository.Update(filial);
                try { await _filialRepository.UnitOfWork.SaveChangesAsync(); }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_filialRepository.Exists(cgc))
                    {
                        throw new ServiceException("Filial não encontrada - " + cgc);
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
                var filial = await ObterAsync(cgc);
                if (filial == null)
                    throw new ServiceException("Filial não cadastrada - " + cgc);

                _filialRepository.Remove(filial);
                await _filialRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
