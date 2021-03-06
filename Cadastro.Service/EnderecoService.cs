using Acessorio.Util;
using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Services
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
            try
            { 
                return await _enderecoRepository.GetFullAsync();
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<Endereco> ObterAsync(int enderecoId)
        {
            try
            {
                var endereco = await _enderecoRepository.GetFullAsync(enderecoId);
                if (endereco == null) throw new ServiceException(
                    $"Endereco com Id {enderecoId} não foi encontrado");
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
                endereco.CEP = Remove.Mascara(endereco.CEP);

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
        public async Task UpdateAsync(int enderecoId, Endereco endereco)
        {
            try
            {
                endereco.CEP = Remove.Mascara(endereco.CEP);

                if (enderecoId != endereco.EnderecoId) throw new ServiceException(
                   $"Id informado {enderecoId} é Diferente do Id d endereço {endereco.EnderecoId}");

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
