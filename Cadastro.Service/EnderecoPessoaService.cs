using Acessorio.Util;
using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Services
{
    public class EnderecoPessoaService : IEnderecoPessoaService
    {
        private readonly IEnderecoPessoaRepository _enderecoPessoaRepository;
        private readonly IEnderecoService _enderecoService;
        private readonly IPessoaService _pessoaService;

        public EnderecoPessoaService(IEnderecoPessoaRepository enderecoPessoaRepository
            , IEnderecoService enderecoService
            , IPessoaService pessoaService)
        {
            _enderecoPessoaRepository = enderecoPessoaRepository;
            _enderecoService = enderecoService;
            _pessoaService = pessoaService;
        }

        #region ObterAsync
        public async Task<IEnumerable<EnderecoPessoa>> ObterAsync()
        {
            try
            { 
                return await _enderecoPessoaRepository.ObterAsync();
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<EnderecoPessoa> ObterAsync(int id)
        {
            try
            {
                var ep = await _enderecoPessoaRepository.ObterAsync(id);
                if (ep == null) throw new ServiceException(
                    $"Endereco pessoa com Id {id} não foi encontrado");
                return ep;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(EnderecoPessoa ep)
        {
            try
            {
                _enderecoPessoaRepository.Insere(ep);
                await _enderecoPessoaRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int id, EnderecoPessoa ep)
        {
            try
            {
                if (id != ep.Id) throw new ServiceException(
                   $"Id informado {id} é Diferente do Id do endereço pessoa {ep.Id}");

                _enderecoPessoaRepository.Update(ep);
                await _enderecoPessoaRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(int id)
        {
            try
            {
                var ep = await ObterAsync(id);

                _enderecoPessoaRepository.Remove(ep);

                await _enderecoPessoaRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region ManterAsync
        public async Task ManterAsync(int pessoaId, Endereco endereco)
        {
            try
            {
                var pessoa = await _pessoaService.ObterAsync(pessoaId);
                
                if (endereco.EnderecoId == 0)
                {
                    await _enderecoService.InsereAsync(endereco);

                    var ep = (new EnderecoPessoa()
                    {
                        EnderecoId = endereco.EnderecoId,
                        PessoaId = pessoa.PessoaId
                    });

                    await InsereAsync(ep);
                }
                else
                {
                    switch ((EEvento)endereco.Evento)
                    {
                        case EEvento.Alterar:
                            await _enderecoService.UpdateAsync(endereco.EnderecoId, endereco);
                            break;

                        case EEvento.Excluir:
                            var ep = await _enderecoPessoaRepository
                                .ObterAsync(endereco.EnderecoId, pessoa.PessoaId);

                            await RemoveAsync(ep.Id);

                            await _enderecoService.RemoveAsync(endereco.EnderecoId);
                            break;

                        default:
                            break;
                    }
                }
                await _enderecoPessoaRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
