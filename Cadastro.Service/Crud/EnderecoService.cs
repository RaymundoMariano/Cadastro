using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Services.Crud
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
            catch (Exception) { throw; }
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
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
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
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int enderecoId, Endereco endereco)
        {
            try
            {
                if (enderecoId != endereco.EnderecoId) throw new ServiceException(
                   $"Id informado {enderecoId} é Diferente do Id d endereço {endereco.EnderecoId}");

                var cep = await _cepService.ObterAsync(endereco.CEP);
                MontarCep(endereco, cep);
                _enderecoRepository.Update(endereco);
                await _enderecoRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
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
            catch (Exception) { throw; }
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
