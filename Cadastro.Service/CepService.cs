using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Extensions;
using Cadastro.Domain.Contracts.UnitOfWorks;
using ViaCep;

namespace Cadastro.Service
{
    public class CepService(IUnitOfWork unitOfWork, IViaCepClient viaCepClient) : ICepService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IViaCepClient _viaCepClient = viaCepClient;

        #region ObterAsync
        public async Task<IEnumerable<Cep>> ObterAsync()
        {
            try
            {
                return await _unitOfWork.Ceps.ObterAsync();
            }
            catch (Exception) { throw; }
        }

        public async Task<Cep> ObterAsync(string cep)
        {
            try
            {
                if (!cep.CEPValido())
                    throw new ServiceException($"Cep inválido - {cep}");

                var Cep = await _unitOfWork.Ceps.ObterAsync(cep);
                if (Cep != null)
                    return Cep;
                 
                var result = _viaCepClient.Search(cep);
                if (result is null)
                    throw new ServiceException($"Cep informado {cep} não foi encontrado!");

                Cep = new Cep() {
                    CEP = cep,
                    Logradouro = result.Street,
                    Bairro = result.Neighborhood,
                    Cidade = result.City,
                    Uf = result.StateInitials
                };
                await InsereAsync(Cep);
                return Cep;
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(Cep cep)
        {
            try
            {
                if (!cep.CEP.CEPValido())
                    throw new ServiceException($"Cep inválido - {cep.CEP}");

                await _unitOfWork.Ceps.InsereAsync(cep);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion
    }
}
