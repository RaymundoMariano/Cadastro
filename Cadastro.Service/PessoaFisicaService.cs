using Acessorio.Util;
using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Services
{
    public class PessoaFisicaService : IPessoaFisicaService
    {
        private readonly IPessoaFisicaRepository _pessoaFisicaRepository;

        public PessoaFisicaService(IPessoaFisicaRepository pessoaFisicaRepository)
        {
            _pessoaFisicaRepository = pessoaFisicaRepository;
        }

        #region ObterAsync
        public async Task<IEnumerable<PessoaFisica>> ObterAsync()
        {
            try 
            { 
                var pfs = await _pessoaFisicaRepository.GetFullAsync();

                foreach (var pf in pfs) { pf.Cpf = Formate.CPF(pf.Cpf); }

                return pfs;
            }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<PessoaFisica> ObterAsync(string cpf)
        {
            try
            {
                cpf = Remove.Mascara(cpf);

                if (!Validacao.CPFValido(cpf)) throw new ServiceException(
                    $"CPF inválido - {cpf}");

                var pf = await _pessoaFisicaRepository.GetFullAsync(cpf);
                if (pf == null) throw new ServiceException(
                    $"CPF informado {cpf} não foi encontrado");

                pf.Cpf = Formate.CPF(pf.Cpf);
                return pf;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(PessoaFisica pf)
        {
            try
            {
                pf.Cpf = Remove.Mascara(pf.Cpf);

                if (!Validacao.CPFValido(pf.Cpf)) throw new ServiceException(
                    $"CPF inválido - {pf.Cpf}");

                _pessoaFisicaRepository.Insere(pf);
                await _pessoaFisicaRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(string cpf)
        {
            try
            {
                var pf = await ObterAsync(cpf);

                pf.Cpf = Remove.Mascara(pf.Cpf);

                _pessoaFisicaRepository.Remove(pf);
                await _pessoaFisicaRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        
        #endregion
    }
}
