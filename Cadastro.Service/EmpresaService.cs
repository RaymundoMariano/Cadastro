using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Contracts.UnitOfWorks;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Service
{
    public class EmpresaService(IUnitOfWork unitOfWork, IEnderecoService enderecoService) : IEmpresaService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IEnderecoService _enderecoService = enderecoService;

        #region ObterAsync
        public async Task<IEnumerable<Empresa>> ObterAsync()
        {
            try
            {
                return await _unitOfWork.Empresas.GetFullAsync();
            }
            catch (Exception) { throw; }
        }

        public async Task<Empresa> ObterAsync(int empresaId)
        {
            try
            {
                var empresa = await _unitOfWork.Empresas.GetFullAsync(empresaId);

                if (empresa == null) throw new ServiceException(
                    $"Empresa com Id {empresaId} não foi encontrada");

                return empresa;
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }

        public async Task<Empresa> ObterAsync(string cgc)
        {
            try
            {
                if (!cgc.CNPJValido()) throw new ServiceException(
                    $"O CGC informado {cgc} é inválido!");

                var empresa = await _unitOfWork.Empresas.GetFullAsync(cgc.RemoveMascara());

                if (empresa == null) throw new ServiceException(
                    $"Empresa com CGC {cgc} não foi encontrada!");

                return empresa;
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region InsereAsync
        public async Task InsereAsync(Empresa empresa)
        {
            try
            {
                if (!empresa.Cgc.CNPJValido()) throw new ServiceException(
                    $"O CGC informado {empresa.Cgc} é inválido!");

                await _unitOfWork.Empresas.InsereAsync(empresa);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region UpdateAsync
        public async Task UpdateAsync(int empresaId, Empresa empresa)
        {
            try
            {
                if (empresaId != empresa.EmpresaId) throw new ServiceException(
                    $"Id informado {empresaId} é Diferente do Id da empresa {empresa.EmpresaId}");

                if (!empresa.Cgc.CNPJValido()) throw new ServiceException(
                    $"O CGC informado {empresa.Cgc} é inválido!");

                _unitOfWork.Empresas.Update(empresa);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region RemoveAsync
        public async Task RemoveAsync(int empresaId)
        {
            try
            {
                var empresa = await ObterAsync(empresaId);

                var pendencias = GetPendencias(empresa);
                if (pendencias != null) throw new ServiceException(pendencias);

                _unitOfWork.Empresas.Remove(empresa);

                await _unitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region GetSocios
        public async Task<IEnumerable<Pessoa>> GetSocios(int empresaId)
        {
            try
            {
                var pessoas = new List<Pessoa>();

                var pfs = await _unitOfWork.PessoasFisicas.GetFullAsync();

                foreach (var pf in pfs)
                {
                    foreach (var socio in pf.Socios)
                    {
                        if (socio.EmpresaId == empresaId)
                        {
                            pf.Pessoa.Selected = true;
                            break;
                        }
                        pf.Pessoa.Selected = false;
                    }
                    pessoas.Add(pf.Pessoa);
                }

                return pessoas;
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region ManterSociosAsync
        public async Task ManterSociosAsync(int empresaId, List<Pessoa> pessoas)
        {
            try
            {
                foreach (var pessoa in pessoas)
                {
                    var pf = await _unitOfWork.PessoasFisicas.GetFullAsync(pessoa.Cpf);

                    var socio = await _unitOfWork.Socios.GetFullAsync(empresaId, pf.PessoaFisicaId);

                    if (socio == null)
                    {
                        if (pessoa.Selected)
                        {
                            socio = new Socio()
                            {
                                PessoaFisicaId = pf.PessoaFisicaId,
                                EmpresaId = empresaId
                            };
                            await _unitOfWork.Socios.InsereAsync(socio);
                        }
                    }
                    else
                    {
                        if (!pessoa.Selected) { _unitOfWork.Socios.Remove(socio); }
                    }
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region GetFiliais
        public async Task<IEnumerable<Filial>> GetFiliais(int empresaId)
        {
            try
            {
                var filiais = new List<Filial>();

                var empresa = await ObterAsync(empresaId);

                foreach (var filial in empresa.Filiais)
                {
                    filial.Selected = true;
                    filiais.Add(filial);
                }

                var empresas = _unitOfWork.Empresas.GetFiliaisSemVinculos();

                foreach (var emp in empresas)
                {
                    var filial = new Filial()
                    {
                        Cgc = emp.Cgc,
                        EmpresaId = empresaId,
                        Selected = false
                    };

                    filiais.Add(filial);
                }

                return filiais;
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion        

        #region ManterFiliaisAsync
        public async Task ManterFiliaisAsync(int empresaId, List<Filial> filiais)
        {
            try
            {
                foreach (var empresa in filiais)
                {
                    var filial = await _unitOfWork.Filiais.GetFullAsync(empresa.Cgc);

                    if (filial == null)
                    {
                        if (empresa.Selected)
                        {
                            filial = new Filial()
                            {
                                Cgc = empresa.Cgc,
                                EmpresaId = empresaId
                            };
                            await _unitOfWork.Filiais.InsereAsync(filial);
                        }
                    }
                    else
                    {
                        if (!empresa.Selected)
                        {
                            _unitOfWork.Filiais.Remove(filial);
                        }
                    }
                }
                await _unitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region ManterEnderecoAsync
        public async Task ManterEnderecoAsync(int empresaId, Endereco endereco)
        {
            await _enderecoService.ManterEnderecoEmpresaAsync(empresaId, endereco);
        }
        #endregion

        #region GetPendencias
        private static string GetPendencias(Empresa empresa)
        {
            string pendencias = null;

            if (empresa.Endereco != null)
            {
                pendencias += $"\n {empresa.Nome} com endereço associado! Remova o endereço.";
            }

            if (empresa.Socios.Count != 0)
            {
                pendencias += $"\n {empresa.Nome} contém associado(s)! Remova os sócio(s).";
            }

            if (empresa.Filiais.Count != 0)
            {
                pendencias += $"\n {empresa.Nome} com filiais associadas! Remova as filiais.";
            }

            return pendencias;
        }
        #endregion
    }
}
