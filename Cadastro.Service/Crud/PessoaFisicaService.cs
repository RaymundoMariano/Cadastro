using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Entities;
using Cadastro.Service.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Services.Crud
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

                foreach (var pf in pfs) { pf.Cpf = pf.Cpf.FormateCPF(); }

                return pfs;
            }
            catch (Exception) { throw; }
        }

        public async Task<PessoaFisica> ObterAsync(string cpf)
        {
            try
            {
                cpf = cpf.RemoveMascara();

                if (!cpf.CPFValido()) throw new ServiceException(
                    $"CPF inválido - {cpf}");

                var pf = await _pessoaFisicaRepository.GetFullAsync(cpf);
                if (pf == null) throw new ServiceException(
                    $"CPF informado {cpf} não foi encontrado");

                pf.Cpf = pf.Cpf.FormateCPF();
                return pf;
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(PessoaFisica pf)
        {
            try
            {
                pf.Cpf = pf.Cpf.RemoveMascara();

                if (!pf.Cpf.CPFValido()) throw new ServiceException(
                    $"CPF inválido - {pf.Cpf}");

                _pessoaFisicaRepository.Insere(pf);
                await _pessoaFisicaRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(string cpf)
        {
            try
            {
                var pf = await ObterAsync(cpf);

                pf.Cpf = pf.Cpf.RemoveMascara();

                _pessoaFisicaRepository.Remove(pf);
                await _pessoaFisicaRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }        
        #endregion
    }
}
