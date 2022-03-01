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
    public class EmpresaService : IEmpresaService
    {
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IEnderecoService _enderecoService;
        private readonly IPessoaService _pessoaService;
        public EmpresaService(IEmpresaRepository empresaRepository
            , IEnderecoService enderecoService
            , IPessoaService pessoaService)
        {
            _empresaRepository = empresaRepository;
            _enderecoService = enderecoService;
            _pessoaService = pessoaService;
        }

        #region ObterAsync
        public async Task<IEnumerable<Empresa>> ObterAsync()
        {
            try 
            {
                return FormateCGC(await _empresaRepository.GetFullAsync());
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
                
                empresa.Cgc = Formate.CGC(empresa.Cgc);
                return empresa;
            }
            catch (ServiceException) { throw; }
            catch (Exception) { throw; }
        }

        public async Task<Empresa> ObterAsync(string cgc)
        {
            try
            {
                if (!Validacao.CNPJValido(cgc)) throw new ServiceException(
                    $"O CGC informado {cgc} é inválido");

                var empresa = await _empresaRepository.GetFullAsync(Remove.Mascara(cgc));
                if (empresa == null) throw new ServiceException(
                    $"Empresa com CGC {cgc} não foi encontrada");
                
                empresa.Cgc = Formate.CGC(empresa.Cgc);
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
                empresa.Cgc = Remove.Mascara(empresa.Cgc);

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
                empresa.Cgc = Remove.Mascara(empresa.Cgc);

                if (empresaId != empresa.EmpresaId) throw new ServiceException(
                    $"Id informado {empresaId} é Diferente do Id da empresa {empresa.EmpresaId}");

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

        #region GetFiliais
        public async Task<IEnumerable<Empresa>> GetFiliais(int empresaId)
        {
            try
            {
                var empresas = new List<Empresa>();

                var empresa = await ObterAsync(empresaId);

                foreach (var filial in empresa.Filiais) 
                {
                    empresas.Add(filial.Empresa);
                }

                empresas.AddRange(_empresaRepository.GetFiliaisSemVinculos());

                return FormateCGC(empresas);
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

                var empresa = await ObterAsync(empresaId);

                foreach (var socio in empresa.Socios)
                {
                    pessoas.Add(socio.PessoaFisica.Pessoa);
                }

                pessoas.AddRange(await _pessoaService.ObterSemVinculos(empresaId));

                return await _pessoaService.FormateCPF(pessoas);
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region FormateCGC
        public IEnumerable<Empresa> FormateCGC(IEnumerable<Empresa> empresas)
        {
            foreach (var empresa in empresas)
            {
                empresa.Cgc = Formate.CGC(empresa.Cgc);
            }
            return empresas;
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
