using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Entities;
using Acessorio.Util;

namespace Cadastro.Services
{
    public class CepService : ICepService
    {
        private readonly ICepRepository _cepRepository;

        public CepService(ICepRepository cepRepository)
        {
            _cepRepository = cepRepository;
        }

        #region ObterAsync
        public async Task<IEnumerable<Cep>> ObterAsync()
        {
            try
            {
                return await _cepRepository.ObterAsync(); 
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Cep> ObterAsync(string cep)
        {
            try
            {
                cep = Remove.Mascara(cep);

                if (!Validacao.CEPValido(cep))
                    throw new ServiceException($"Cep inválido - { cep }");

                var Cep = await _cepRepository.ObterAsync(cep);
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
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch(Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(Cep cep)
        {
            try
            {
                if (!Validacao.CEPValido(cep.CEP))
                    throw new ServiceException($"Cep inválido - {cep.CEP}");

                _cepRepository.Insere(cep);
                await _cepRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
