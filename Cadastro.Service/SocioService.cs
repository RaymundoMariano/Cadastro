using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Services
{
    public class SocioService : ISocioService
    {
        private readonly ISocioRepository _socioRepository;

        public SocioService(ISocioRepository socioRepository)
        {
            _socioRepository = socioRepository;
        }

        #region ObterAsync
        public async Task<IEnumerable<Socio>> ObterAsync()
        {
            try 
            { 
                return await _socioRepository.ObterAsync();
            }
            catch (Exception) { throw; }
        }

        public async Task<Socio> ObterAsync(int id)
        {
            try
            {
                var socio = await _socioRepository.ObterAsync(id);
                if (socio == null) throw new ServiceException(
                    $"Id informado {id} não foi encontrado");
                return socio;
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(Socio socio)
        {
            try
            {
                _socioRepository.Insere(socio);
                await _socioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int socioId, Socio socio)
        {
            try
            {
                if (socioId != socio.SocioId) throw new ServiceException(
                   $"Id informado {socioId} é Diferente do Id do sócio {socio.SocioId}");

                _socioRepository.Update(socio);
                await _socioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(int id)
        {
            try
            {
                var socio = await ObterAsync(id);
                _socioRepository.Remove(socio);
                await _socioRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion
    }
}
