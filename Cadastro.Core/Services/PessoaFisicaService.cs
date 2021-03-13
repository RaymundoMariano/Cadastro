using Acessorio.Util;
using Cadastro.Core.Domain.Models;
using Cadastro.Core.Domain.Repositories;
using Cadastro.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Core.Services
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
            try { return await _pessoaFisicaRepository.ObterAsync(); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        public async Task<PessoaFisica> ObterAsync(long? cpf)
        {
            try
            {
                if (!Validacao.CPFValido(cpf.ToString()))
                    throw new ServiceException("CPF inválido - " + cpf);

                var pessoaFisica = await _pessoaFisicaRepository.ObterAsync(cpf);
                if (pessoaFisica == null)
                    throw new ServiceException("Pessoa fisica não cadastrada - " + cpf);
                return pessoaFisica;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
