using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Contracts.UnitOfWorks;
using Cadastro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadastro.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PessoaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region ObterAsync
        public async Task<IEnumerable<Pessoa>> ObterAsync()
        {
            try
            {
                return await _unitOfWork.Pessoas.GetFullAsync();
            }
            catch (Exception) { throw; }
        }

        public async Task<Pessoa> ObterAsync(int pessoaId)
        {
            try
            {
                var pessoa = await _unitOfWork.Pessoas.GetFullAsync(pessoaId);
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
                if (pessoa.Cpf != null)
                {
                    var pf = await _unitOfWork.PessoasFisicas.GetFullAsync(pessoa.Cpf);

                    if (pf != null) throw new ServiceException(
                        $"Já existe uma pessoa cadastrada com o cpf {pessoa.Cpf}");

                    await _unitOfWork.Pessoas.InsereAsync(pessoa);
                    await _unitOfWork.SaveChangesAsync();

                    pf = (new PessoaFisica()
                    {
                        Cpf = pessoa.Cpf,
                        PessoaId = pessoa.PessoaId
                    });
                    await _unitOfWork.PessoasFisicas.InsereAsync(pf);
                }
                else
                {
                    await _unitOfWork.Pessoas.InsereAsync(pessoa);
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
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

                var pfOrigem = await _unitOfWork.PessoasFisicas.GetFullAsync(pessoaId);

                var pf = new PessoaFisica();
                if (pessoa.Cpf == null)
                    pf = pfOrigem;
                else
                    pf = await _unitOfWork.PessoasFisicas.GetFullAsync(pessoa.Cpf);

                if (pf != null)
                {
                    if (pessoa.Cpf != null)
                    {
                        if (pessoaId != pf.PessoaId) throw new ServiceException(
                            $"Já existe pessoa cadastrada com o cpf {pessoa.Cpf}");

                        if (pfOrigem.Cpf != pessoa.Cpf)
                        {
                            pf = (new PessoaFisica()
                            {
                                Cpf = pfOrigem.Cpf,
                                PessoaId = pessoaId
                            });
                            _unitOfWork.PessoasFisicas.Remove(pf);

                            pf.Cpf = pessoa.Cpf;
                            await _unitOfWork.PessoasFisicas.InsereAsync(pf);
                        }
                    }
                    else
                    {
                        if (pf.Socios.Count() != 0)
                        {
                            throw new ServiceException(
                                $"{pf.Cpf} está vinculado como sócio de pelo menos uma empresa." +
                                $" Alteração inválida!");
                        }

                        pf = await _unitOfWork.PessoasFisicas.ObterAsync(pfOrigem.PessoaFisicaId);  
                        
                        _unitOfWork.PessoasFisicas.Remove(pf);
                    }
                }
                else
                {
                    if (pessoa.Cpf != null)
                    {
                        if (pfOrigem != null)
                        {
                            pf = await _unitOfWork.PessoasFisicas.ObterAsync(pfOrigem.PessoaFisicaId);
                            _unitOfWork.PessoasFisicas.Remove(pf);
                        }

                        pf = (new PessoaFisica()
                        {
                            Cpf = pessoa.Cpf,
                            PessoaId = pessoaId
                        });
                        await _unitOfWork.PessoasFisicas.InsereAsync(pf);
                    }
                }
                _unitOfWork.Pessoas.Update(pessoa);
                await _unitOfWork.SaveChangesAsync();
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

                if (pf != null)
                    _unitOfWork.PessoasFisicas.Remove(pf);

                _unitOfWork.Pessoas.Remove(pessoa);
                await _unitOfWork.SaveChangesAsync();
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

            var pf = pessoa.PessoaFisicas.FirstOrDefault(pf => pf.PessoaId == pessoa.PessoaId);
            if (pessoa.PessoaFisicas.Count != 0)
            {
                if (pf != null)
                {
                    if (pf.Socios.Count != 0)
                    {
                        var socio = pf.Socios.FirstOrDefault(s => s.PessoaFisicaId == pf.PessoaFisicaId);
                        if (socio != null)
                        {
                            pendencias += ($"\n {pessoa.Nome} é sócio da empresa {socio.Empresa.Nome}! Remova o sócio.");
                        }
                    }
                }
            }
            pessoaFisica = pf;
            return pendencias;
        }
        #endregion
    }
}
