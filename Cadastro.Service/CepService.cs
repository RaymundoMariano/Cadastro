using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Extensions;
using Cadastro.Domain.Contracts.UnitOfWorks;

namespace Cadastro.Services
{
    public class CepService : ICepService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CepService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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
                    throw new ServiceException($"Cep inválido - { cep }");

                var Cep = await _unitOfWork.Ceps.ObterAsync(cep);
                if (Cep != null)
                    return Cep;

                var lCep = await new Correios.NET.Services().GetAddressesAsync(cep.ToString());
                if (lCep.Count() == 0)
                    throw new ServiceException($"Cep informado {cep} não foi encontrado!");

                foreach (var eCep in lCep)
                {
                    Cep = new Cep();
                    Cep.CEP = cep;
                    Cep.Logradouro = eCep.Street;
                    Cep.Bairro = eCep.District;
                    Cep.Cidade = eCep.City;
                    Cep.Uf = eCep.State;
                    await InsereAsync(Cep);
                }
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
