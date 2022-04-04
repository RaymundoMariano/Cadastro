using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Enums;
using Cadastro.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Services.Crud
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
                return await _filialRepository.GetFullAsync();
            }
            catch (Exception) { throw; }
        }

        public async Task<Filial> ObterAsync(int filialId)
        {
            try
            {
                var filial = await _filialRepository.GetFullAsync(filialId);
                
                if (filial == null) throw new ServiceException(
                    $"Empresa com Id {filialId} não foi encontrada");

                return filial;
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }

        public async Task<Filial> ObterAsync(string cgc)
        {
            try
            {
                cgc = cgc.FormateCGC();

                var filial = await _filialRepository.GetFullAsync(cgc);
                if (filial == null) throw new ServiceException(
                    $"Empresa com Cgc {cgc} não foi encontrada");

                filial.Cgc = filial.Cgc.FormateCGC();
                return filial;
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
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

                _filialRepository.Insere(filial);
                await _filialRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int filialId, Filial filial)
        {
            try
            {
                if (filialId != filial.FilialId) throw new ServiceException(
                    $"Id informado {filialId} é Diferente do Id da empresa {filial.FilialId}");

                _filialRepository.Update(filial);
                await _filialRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(int filialId)
        {
            try
            {
                var filial = await ObterAsync(filialId);

                _filialRepository.Remove(filial);
                await _filialRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion        
    }
}
