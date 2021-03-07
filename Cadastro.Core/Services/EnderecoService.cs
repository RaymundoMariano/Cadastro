using Cadastro.Core.Domain.Models;
using Cadastro.Core.Domain.Repositories;
using Cadastro.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Core.Services
{
    public class EnderecoService : IEnderecoService
    {
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly ICepService _cepService;

        public EnderecoService(IEnderecoRepository enderecoRepository, ICepService cepService)
        {
            _enderecoRepository = enderecoRepository;
            _cepService = cepService;
        }

        #region ObterAsync
        public async Task<IEnumerable<Endereco>> ObterAsync()
        {
            try { return await _enderecoRepository.ObterAsync(); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Endereco> ObterAsync(int enderecoId)
        {
            try
            {
                var endereco = await _enderecoRepository.ObterAsync(enderecoId);
                if (endereco == null)
                {
                    throw new ServiceException("Endereco não cadastrado - " + enderecoId);
                }
                return endereco;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(Endereco endereco)
        {
            try
            {
                if (endereco.CEP != null)
                {
                    var cep = await _cepService.ObterAsync(endereco.CEP);
                    MontarCep(endereco, cep);
                }
                _enderecoRepository.Insere(endereco);
                await _enderecoRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(Endereco endereco)
        {
            try
            {
                var cep = await _cepService.ObterAsync(endereco.CEP);
                MontarCep(endereco, cep);
                _enderecoRepository.Update(endereco);
                await _enderecoRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(int enderecoId)
        {
            try
            {
                var endereco = await ObterAsync(enderecoId);
                _enderecoRepository.Remove(endereco);
                await _enderecoRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region MontarCep
        private void MontarCep(Endereco endereco, Cep cep)
        {
            endereco.Logradouro = cep.Logradouro;
            endereco.Bairro = cep.Bairro;
            endereco.Cidade = cep.Cidade;
            endereco.Uf = cep.Uf;
        }
        #endregion
    }
}
