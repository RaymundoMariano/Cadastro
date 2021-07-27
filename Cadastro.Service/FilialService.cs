using Acessorio.Util;
using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadastro.Services
{
    public class FilialService : IFilialService
    {
        private readonly IFilialRepository _filialRepository;
        private readonly IEmpresaService _empresaService;

        public FilialService(IFilialRepository filialRepository, IEmpresaService empresaService)
        {
            _filialRepository = filialRepository;
            _empresaService = empresaService;
        }

        #region ObterAsync
        public async Task<IEnumerable<Filial>> ObterAsync()
        {
            try
            {
                var filiais = await _filialRepository.GetFullAsync();

                foreach (var filial in filiais) { filial.Cgc = Formate.CGC(filial.Cgc); }

                return filiais;
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Filial> ObterAsync(int filialId)
        {
            try
            {
                var filial = await _filialRepository.GetFullAsync(filialId);
                if (filial == null) throw new ServiceException(
                    $"Empresa com Id {filialId} não foi encontrada");

                filial.Cgc = Formate.CGC(filial.Cgc);
                return filial;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Filial> ObterAsync(string cgc)
        {
            try
            {
                cgc = Remove.Mascara(cgc);

                var filial = await _filialRepository.GetFullAsync(cgc);
                if (filial == null) throw new ServiceException(
                    $"Empresa com Cgc {cgc} não foi encontrada");

                filial.Cgc = Formate.CGC(filial.Cgc);
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
                var empresa = await _empresaService.ObterAsync(filial.Cgc);
                if (empresa.Tipo == (int)ETipoEmpresa.Matriz) throw new ServiceException(
                     $"A empresa com CGC {filial.Cgc} é matriz! Não pode ser registrada como filial");

                empresa = await _empresaService.ObterAsync(filial.EmpresaId);
                if (empresa.Tipo == (int)ETipoEmpresa.Filial) throw new ServiceException(
                    $"A empresa com CGC {filial.Cgc} é filial! Não pode ser registrada como matriz");

                filial.Cgc = Remove.Mascara(filial.Cgc);
                _filialRepository.Insere(filial);
                await _filialRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int filialId, Filial filial)
        {
            try
            {
                if (filialId != filial.FilialId) throw new ServiceException(
                    $"Id informado {filialId} é Diferente do Id da empresa {filial.FilialId}");

                filial.Cgc = Remove.Mascara(filial.Cgc);
                _filialRepository.Update(filial);
                await _filialRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(int filialId)
        {
            try
            {
                var filial = await ObterAsync(filialId);

                filial.Cgc = Remove.Mascara(filial.Cgc);

                _filialRepository.Remove(filial);
                await _filialRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion        
    }
}
