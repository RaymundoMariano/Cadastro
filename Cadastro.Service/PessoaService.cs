using Acessorio.Util;
using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadastro.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IPessoaFisicaRepository _pessoaFisicaRepository;

        public PessoaService(
            IPessoaRepository pessoaRepository, IPessoaFisicaRepository pessoaFisicaRepository)
        {
            _pessoaRepository = pessoaRepository;
            _pessoaFisicaRepository = pessoaFisicaRepository;
        }

        #region ObterSemVinculos
        public async Task<IEnumerable<Pessoa>> ObterSemVinculos(int empresaId)
        {
            try
            {
                return await FormateCPF(_pessoaRepository.GetPessoasSemVinculos(empresaId));
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region ObterAsync
        public async Task<IEnumerable<Pessoa>> ObterAsync()
        {
            try
            {
                return await FormateCPF(await _pessoaRepository.GetFullAsync());
            }
            catch (Exception) { throw; }
        }

        public async Task<Pessoa> ObterAsync(int pessoaId)
        {
            try
            {
                var pessoa = await _pessoaRepository.GetFullAsync(pessoaId);
                if (pessoa == null) throw new ServiceException(
                    $"Pessoa com {pessoaId} não foi encontrada");

                var pf = pessoa.PessoaFisicas.FirstOrDefault(pf => pf.PessoaId == pessoa.PessoaId);
                if (pf != null) pessoa.Cpf = Formate.CPF(pf.Cpf);

                return pessoa;
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(Pessoa pessoa)
        {
            try
            {
                _pessoaRepository.Insere(pessoa);

                if (pessoa.Cpf != null)
                {
                    var pf = (new PessoaFisica()
                    {
                        Cpf = pessoa.Cpf,
                        PessoaId = pessoa.PessoaId
                    });
                    _pessoaFisicaRepository.Insere(pf);
                }
                await _pessoaRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int pessoaId, Pessoa pessoa)
        {
            try
            {
                if (pessoaId != pessoa.PessoaId) throw new ServiceException(
                    $"Id informado {pessoaId} é Diferente do Id da pessoa {pessoa.PessoaId}");

                pessoa.Cpf = Remove.Mascara(pessoa.Cpf);

                if (pessoa.Cpf != null
                    && pessoa.PessoaFisicas.FirstOrDefault(pf => pf.Cpf == pessoa.Cpf) == null)
                {
                    var pf = (new PessoaFisica()
                    {
                        Cpf = pessoa.Cpf,
                        PessoaId = pessoa.PessoaId
                    });
                    _pessoaFisicaRepository.Insere(pf);
                }
                _pessoaRepository.Update(pessoa);
                await _pessoaRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(int pessoaId)
        {
            try
            {
                var pessoa = await ObterAsync(pessoaId);

                var pf = new PessoaFisica();

                var pendencias = GetPendencias(pessoa, out pf);
                if (pendencias != null) throw new ServiceException(pendencias);

                _pessoaFisicaRepository.Remove(pf);

                _pessoaRepository.Remove(pessoa);
                await _pessoaRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region FormateCPF
        public async Task<IEnumerable<Pessoa>> FormateCPF(IEnumerable<Pessoa> pessoas)
        {
            foreach (var pessoa in pessoas)
            {
                var pf = await _pessoaFisicaRepository.GetFullAsync(pessoa.PessoaId);

                if (pf == null) continue;

                pessoa.Cpf = Formate.CPF(pf.Cpf);
            }
            return pessoas;
        }
        #endregion

        #region GetPendencias
        private string GetPendencias(Pessoa pessoa, out PessoaFisica pessoaFisica)
        {
            pessoaFisica = new PessoaFisica();

            string pendencias = null;

            if (pessoa.EnderecoPessoas.Count != 0)
            {
                pendencias += ($"\n {pessoa.Nome} com endereço(s) associado(s)! Remova o(s) endereço(s).");
            }

            if (pessoa.PessoaFisicas.Count != 0)
            {
                var pf = pessoa.PessoaFisicas.FirstOrDefault(pf => pf.PessoaId == pessoa.PessoaId);
                if (pf != null)
                {
                    if (pf.Socios.Count != 0)
                    {
                        var socio = pf.Socios.FirstOrDefault(s => s.Cpf == pf.Cpf);
                        if (socio != null)
                        {
                            pendencias += ($"\n {pessoa.Nome} é sócio da empresa {socio.Empresa.Nome}! Remova o sócio.");
                        }
                    }
                }
                pessoaFisica = pf;
            }

            return pendencias;
        }
        #endregion
    }
}
