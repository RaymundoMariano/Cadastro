using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Enums;
using Cadastro.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Services.Crud
{
    public class EmpresaService : IEmpresaService
    {
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IEnderecoService _enderecoService;
        private readonly ISocioService _socioService;
        private readonly IPessoaFisicaService _pessoaFisicaService;
        private readonly IFilialRepository _filialRepository;
        public EmpresaService(IEmpresaRepository empresaRepository
            , IEnderecoService enderecoService
            , ISocioService socioService
            , IPessoaFisicaService pessoaFisicaService
            , IFilialRepository filialRepository)
        {
            _empresaRepository = empresaRepository;
            _enderecoService = enderecoService;
            _socioService = socioService;
            _pessoaFisicaService = pessoaFisicaService;
            _filialRepository = filialRepository;
        }

        #region ObterAsync
        public async Task<IEnumerable<Empresa>> ObterAsync()
        {
            try 
            {
                return await _empresaRepository.GetFullAsync();
            }
            catch (Exception) { throw; }
        }

        public async Task<Empresa> ObterAsync(int empresaId)
        {
            try
            {
                var empresa = await _empresaRepository.GetFullAsync(empresaId);
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

                var empresa = await _empresaRepository.GetFullAsync(cgc.RemoveMascara());
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

                _empresaRepository.Insere(empresa);
                await _empresaRepository.UnitOfWork.SaveChangesAsync();
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

                _empresaRepository.Update(empresa);
                await _empresaRepository.UnitOfWork.SaveChangesAsync();
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
                var empresa = await _empresaRepository.GetFullAsync(empresaId);

                var pendencias = GetPendencias(empresa);
                if (pendencias != null) throw new ServiceException(pendencias);

                _empresaRepository.Remove(empresa);

                await _empresaRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region ManterEnderecoAsync
        public async Task ManterEnderecoAsync(int empresaId, Endereco endereco)
        {
            try
            {
                var empresa = await _empresaRepository.GetFullAsync(empresaId);

                if (endereco.EnderecoId == 0)
                {
                    await _enderecoService.InsereAsync(endereco);

                    empresa.EnderecoId = endereco.EnderecoId;

                    await UpdateAsync(empresaId, empresa);
                }
                else
                {
                    switch ((EEvento)endereco.Evento)
                    {
                        case EEvento.Alterar:
                            await _enderecoService.UpdateAsync(endereco.EnderecoId, endereco);
                            break;

                        case EEvento.Excluir:
                            empresa.EnderecoId = null;
                            await UpdateAsync(empresaId, empresa);

                            await _enderecoService.RemoveAsync(endereco.EnderecoId);
                            break;

                        default:
                            break;
                    }
                }
                await _empresaRepository.UnitOfWork.SaveChangesAsync();
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

                var pfs = await _pessoaFisicaService.ObterAsync();

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
                    var socio = await _socioService.ObterAsync(empresaId, pessoa.Cpf);

                    if (socio == null)
                    {
                        if (pessoa.Selected)
                        {
                            socio = (new Socio()
                            {
                                Cpf = pessoa.Cpf,
                                EmpresaId = empresaId
                            });
                            await _socioService.InsereAsync(socio);
                        }
                    }
                    else
                    {                        
                        if (!pessoa.Selected) { await _socioService.RemoveAsync(socio.SocioId); }
                    }
                }
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

                var empresas = _empresaRepository.GetFiliaisSemVinculos();

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
                    var filial = await _filialRepository.GetFullAsync(empresa.Cgc);

                    if (filial == null)
                    {
                        if (empresa.Selected)
                        {
                            filial = (new Filial()
                            {
                                Cgc = empresa.Cgc,
                                EmpresaId = empresaId
                            });
                            _filialRepository.Insere(filial);
                            _filialRepository.UnitOfWork.SaveChanges();
                        }
                    }
                    else
                    {
                        if (!empresa.Selected) 
                        {
                            _filialRepository.Remove(filial);
                            _filialRepository.UnitOfWork.SaveChanges();
                        }
                    }
                }
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }
        #endregion

        #region GetPendencias
        private string GetPendencias(Empresa empresa)
        {
            string pendencias = null;

            if (empresa.Endereco != null)
            {
                pendencias += ($"\n {empresa.Nome} com endereço associado! Remova o endereço.");
            }

            if (empresa.Socios.Count != 0)
            {
                pendencias += ($"\n {empresa.Nome} contém associado(s)! Remova os sócio(s).");
            }

            if (empresa.Filiais.Count != 0)
            {
                pendencias += ($"\n {empresa.Nome} com filiais associadas! Remova as filiais.");
            }

            return pendencias;
        }
        #endregion
    }
}
