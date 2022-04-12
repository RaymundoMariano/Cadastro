using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Contracts.UnitOfWorks;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Services
{
    public class EnderecoService : IEnderecoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICepService _cepService;
        public EnderecoService(IUnitOfWork unitOfWork, ICepService cepService)
        {
            _unitOfWork = unitOfWork;
            _cepService = cepService;
        }

        #region ObterAsync
        public async Task<IEnumerable<Endereco>> ObterAsync()
        {
            try
            { 
                return await _unitOfWork.Enderecos.GetFullAsync();
            }
            catch (Exception) { throw; }
        }

        public async Task<Endereco> ObterAsync(int enderecoId)
        {
            try
            {
                var endereco = await _unitOfWork.Enderecos.GetFullAsync(enderecoId);

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
                await _unitOfWork.Enderecos.InsereAsync(endereco);
                await _unitOfWork.SaveChangesAsync();
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

                _unitOfWork.Enderecos.Update(endereco);
                await _unitOfWork.SaveChangesAsync();
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

                _unitOfWork.Enderecos.Remove(endereco);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region ManterEnderecoPessoaAsync
        public async Task ManterEnderecoPessoaAsync(int pessoaId, Endereco endereco)
        {
            try
            {
                var pessoa = await _unitOfWork.Pessoas.ObterAsync(pessoaId);

                if (endereco.EnderecoId == 0)
                {
                    await InsereAsync(endereco);

                    var ep = (new EnderecoPessoa()
                    {
                        EnderecoId = endereco.EnderecoId,
                        PessoaId = pessoa.PessoaId
                    });

                    await _unitOfWork.EnderecosPessoa.InsereAsync(ep);
                }
                else
                {
                    switch ((EEvento)endereco.Evento)
                    {
                        case EEvento.Alterar:
                            await UpdateAsync(endereco.EnderecoId, endereco);
                            break;

                        case EEvento.Excluir:
                            var ep = await _unitOfWork.EnderecosPessoa
                                .ObterAsync(endereco.EnderecoId, pessoa.PessoaId);

                            _unitOfWork.EnderecosPessoa.Remove(ep);

                            await RemoveAsync(endereco.EnderecoId);
                            break;

                        default:
                            break;
                    }
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region ManterEnderecoEmpresaAsync
        public async Task ManterEnderecoEmpresaAsync(int empresaId, Endereco endereco)
        {
            try
            {
                var empresa = await _unitOfWork.Empresas.ObterAsync(empresaId);

                if (endereco.EnderecoId == 0)
                {
                    await InsereAsync(endereco);

                    empresa.EnderecoId = endereco.EnderecoId;

                    _unitOfWork.Empresas.Update(empresa);
                }
                else
                {
                    switch ((EEvento)endereco.Evento)
                    {
                        case EEvento.Alterar:
                            await UpdateAsync(endereco.EnderecoId, endereco);
                            break;

                        case EEvento.Excluir:
                            empresa.EnderecoId = null;
                            _unitOfWork.Empresas.Update(empresa);

                            await RemoveAsync(endereco.EnderecoId);
                            break;

                        default:
                            break;
                    }
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
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
