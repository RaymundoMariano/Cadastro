using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadastro.Services.Crud
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

        #region ObterAsync
        public async Task<IEnumerable<Pessoa>> ObterAsync()
        {
            try
            {
                return await _pessoaRepository.GetFullAsync();
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

                var pf = await _pessoaFisicaRepository.GetFullAsync(pessoaId);

                if (pessoa.Cpf != null && pf == null)
                {
                    pf = (new PessoaFisica()
                    {
                        Cpf = pessoa.Cpf,
                        PessoaId = pessoa.PessoaId
                    });
                    _pessoaFisicaRepository.Insere(pf);
                }
                else
                {
                    if (pessoa.Cpf == null && pf != null)
                    {
                        _pessoaFisicaRepository.Remove(pf);
                    }
                    else
                    {
                        throw new ServiceException(
                            $"Cpf informado {pessoa.Cpf} é Diferente do cpf da pessoa {pf.Cpf}");
                    }
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
