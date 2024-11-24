using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Cadastro.API.Models.Aplicacao;
using Cadastro.API.Models.Response;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Enums;
using Cadastro.Domain.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Cadastro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresasController(IEmpresaService empresaService, IMapper mapper) : ControllerBase
    {
        private readonly IEmpresaService _empresaService = empresaService;
        private readonly IMapper _mapper = mapper;

        #region GetEmpresa
        // GET: api/Empresas
        [HttpGet]
        public async Task<ActionResult<ResponseModel>> GetEmpresa()
        {
            try
            {
                var empresas = await _empresaService.ObterAsync();

                var empresasModel = _mapper.Map<List<EmpresaModel>>(empresas);

                foreach (var empresa in empresasModel) 
                {
                    empresa.TipoEmpresa = ((ETipoEmpresa)empresa.Tipo).ToString();
                    empresa.Cgc = empresa.Cgc.FormateCGC();
                }

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = empresasModel,
                    Errors = []
                });
            }
            catch (Exception ex) { return Erro(ex.Message, false); }
        }

        // GET: api/Empresas/5
        [HttpGet("{empresaId}")]
        public async Task<ActionResult<ResponseModel>> GetEmpresa(int empresaId)
        {
            try
            {
                var empresa = await _empresaService.ObterAsync(empresaId);

                var empresaModel = _mapper.Map<EmpresaModel>(empresa);

                empresaModel.Cgc = empresaModel.Cgc.FormateCGC();

                foreach(var socio in empresaModel.Socios) 
                {
                    socio.PessoaFisica.Pessoa.Cpf = socio.PessoaFisica.Cpf.FormateCPF();
                }

                foreach(var filial in empresaModel.Filiais) 
                {
                    filial.Cgc = filial.Cgc.FormateCGC();

                    var emp = await _empresaService.ObterAsync(filial.Cgc);

                    filial.Nome = emp.Nome;
                    filial.EmpresaIdFilial = emp.EmpresaId;
                    filial.TipoEmpresa = ((ETipoEmpresa)emp.Tipo).ToString();
                }

                empresaModel.TipoEmpresa = ((ETipoEmpresa)empresaModel.Tipo).ToString();

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = empresaModel,
                    Errors = []
                });
            }
            catch (ServiceException ex) { return Erro(ex.Message, true); }
            catch (Exception ex) { return Erro(ex.Message, false); }
        }
        #endregion

        #region PostEmpresa
        // POST: api/Empresas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ResponseModel>> PostEmpresa(EmpresaModel empresaModel)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var empresa = _mapper.Map<Empresa>(empresaModel);

                empresa.Cgc = empresa.Cgc.RemoveMascara();

                await _empresaService.InsereAsync(empresa);

                empresa.Cgc = empresa.Cgc.FormateCGC();

                CreatedAtAction("GetEmpresa", new { empresaId = empresaModel.EmpresaId }, empresaModel);

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<EmpresaModel>(empresa),
                    Errors = []
                });
            }
            catch (ServiceException ex) { return Erro(ex.Message, true); }
            catch (Exception ex) { return Erro(ex.Message, false); }
        }
        #endregion

        #region PutEmpresa
        // PUT: api/Empresas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{empresaId}")]
        public async Task<ActionResult<ResponseModel>> PutEmpresa(int empresaId, EmpresaModel empresaModel)
        {
            try
            {
                if (empresaId != empresaModel.EmpresaId) return BadRequest();

                var empresa = _mapper.Map<Empresa>(empresaModel);

                empresa.Cgc = empresa.Cgc.RemoveMascara();

                await _empresaService.UpdateAsync(empresaId, empresa);

                CreatedAtAction("GetEmpresa", new { empresaId = empresaModel.EmpresaId }, empresaModel);

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = empresaModel,
                    Errors = []
                });
            }
            catch (ServiceException ex) { return Erro(ex.Message, true); }
            catch (Exception ex) { return Erro(ex.Message, false); }
        }
        #endregion        

        #region DeleteEmpresa
        // DELETE: api/Empresas/5
        [HttpDelete("{empresaId}")]
        public async Task<ActionResult<ResponseModel>> DeleteEmpresa(int empresaId)
        {
            try
            {
                await _empresaService.RemoveAsync(empresaId);
                NoContent();
                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = null,
                    Errors = []
                });
            }
            catch (ServiceException ex) { return Erro(ex.Message, true); }
            catch (Exception ex) { return Erro(ex.Message, false); }
        }
        #endregion

        #region PostEndereco
        [Route("PostEndereco")]
        [HttpPost]
        public async Task<ActionResult<ResponseModel>> PostEndereco(int empresaId, EnderecoModel enderecoModel)
        {
            try
            {
                var endereco = _mapper.Map<Endereco>(enderecoModel);

                endereco.CEP = endereco.CEP.RemoveMascara();

                await _empresaService.ManterEnderecoAsync(empresaId, endereco);

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = enderecoModel,
                    Errors = []
                });
            }
            catch (ServiceException ex) { return Erro(ex.Message, true); }
            catch (Exception ex) { return Erro(ex.Message, false); }
        }
        #endregion

        #region GetFiliais
        [Route("GetFiliais")]
        [HttpGet]
        public async Task<ActionResult<ResponseModel>> GetFiliais(int empresaId)
        {
            try
            {
                var filiais = await _empresaService.GetFiliais(empresaId);

                var filiaisModel = _mapper.Map<List<FilialModel>>(filiais);

                foreach (var filial in filiaisModel)
                {
                    var empresa = await _empresaService.ObterAsync(filial.Cgc);

                    filial.Nome = empresa.Nome;
                    filial.EmpresaIdFilial = empresa.EmpresaId;
                    filial.TipoEmpresa = ((ETipoEmpresa)empresa.Tipo).ToString();

                    filial.Cgc = filial.Cgc.FormateCGC();
                }

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = filiaisModel,
                    Errors = []
                });
            }
            catch (ServiceException ex) { return Erro(ex.Message, true); }
            catch (Exception ex) { return Erro(ex.Message, false); }
        }
        #endregion

        #region PostFiliais
        [Route("PostFiliais")]
        [HttpPost]
        public async Task<ActionResult<ResponseModel>> PostFiliais(int empresaId, List<FilialModel> filiaisModel)
        {
            try
            {
                var filiais = _mapper.Map<List<Filial>>(filiaisModel);

                foreach (var filial in filiais) { filial.Cgc = filial.Cgc.RemoveMascara(); }

                await _empresaService.ManterFiliaisAsync(empresaId, filiais);

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = filiaisModel,
                    Errors = []
                });
            }
            catch (ServiceException ex) { return Erro(ex.Message, true); }
            catch (Exception ex) { return Erro(ex.Message, false); }
        }
        #endregion

        #region GetSocios
        [Route("GetSocios")]
        [HttpGet]
        public async Task<ActionResult<ResponseModel>> GetSocios(int empresaId)
        {
            try
            {
                var pessoas = await _empresaService.GetSocios(empresaId);

                foreach(var pessoa in pessoas) 
                {
                    foreach (var pf in pessoa.PessoaFisicas) { pessoa.Cpf = pf.Cpf.FormateCPF(); }
                }

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<List<PessoaModel>>(pessoas),
                    Errors = []
                });
            }
            catch (ServiceException ex) { return Erro(ex.Message, true); }
            catch (Exception ex) { return Erro(ex.Message, false); }
        }
        #endregion        

        #region PostSocios
        [Route("PostSocios")]
        [HttpPost]
        public async Task<ActionResult<ResponseModel>> PostSocios(int empresaId, List<PessoaModel> pessoasModel)
        {
            try
            {
                var pessoas = _mapper.Map<List<Pessoa>>(pessoasModel);

                foreach (var pessoa in pessoas) { pessoa.Cpf = pessoa.Cpf.RemoveMascara(); }

                await _empresaService.ManterSociosAsync(empresaId, pessoas);

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = pessoasModel,
                    Errors = []
                });
            }
            catch (ServiceException ex) { return Erro(ex.Message, true); }
            catch (Exception ex) { return Erro(ex.Message, false); }
        }
        #endregion        

        #region Erro
        private static ActionResult<ResponseModel> Erro(string mensagem, bool succeeded)
        {
            return (new ResponseModel()
            {
                Succeeded = succeeded,
                ObjectRetorno = null,
                Errors = (mensagem == null) ? [] : [mensagem]
            });
        }
        #endregion
    }
}
