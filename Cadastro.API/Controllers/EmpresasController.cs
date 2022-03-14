using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Enums;
using Cadastro.Domain.Models.Aplicacao;
using Cadastro.Domain.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cadastro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresasController : ControllerBase
    {
        private readonly IEmpresaService _empresaService;
        private readonly IMapper _mapper;
        public EmpresasController(IEmpresaService empresaService, IMapper mapper)
        {
            _empresaService = empresaService;
            _mapper = mapper;
        }

        #region GetEmpresa
        // GET: api/Empresas
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResultModel>> GetEmpresa()
        {
            try
            {
                var empresas = await _empresaService.ObterAsync();

                var empresasModel = _mapper.Map<List<EmpresaModel>>(empresas);

                foreach (var empresa in empresasModel) 
                {
                    empresa.TipoEmpresa = ((ETipoEmpresa)empresa.Tipo).ToString();
                }

                return (new ResultModel()
                {
                    Succeeded = true,
                    ObjectRetorno = empresasModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }

        // GET: api/Empresas/5
        [HttpGet("{empresaId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultModel>> GetEmpresa(int empresaId)
        {
            try
            {
                var empresa = await _empresaService.ObterAsync(empresaId);

                var empresaModel = _mapper.Map<EmpresaModel>(empresa);

                empresaModel.TipoEmpresa = ((ETipoEmpresa)empresaModel.Tipo).ToString();

                return (new ResultModel()
                {
                    Succeeded = true,
                    ObjectRetorno = empresaModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region PostEmpresa
        // POST: api/Empresas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ResultModel>> PostEmpresa(EmpresaModel empresaModel)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var empresa = _mapper.Map<Empresa>(empresaModel);
                await _empresaService.InsereAsync(empresa);

                empresaModel = _mapper.Map<EmpresaModel>(empresa);
                CreatedAtAction("GetEmpresa", new { empresaId = empresaModel.EmpresaId }, empresaModel);

                return (new ResultModel()
                {
                    Succeeded = true,
                    ObjectRetorno = empresaModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region PutEmpresa
        // PUT: api/Empresas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{empresaId}")]
        public async Task<ActionResult<ResultModel>> PutEmpresa(int empresaId, EmpresaModel empresaModel)
        {
            try
            {
                if (empresaId != empresaModel.EmpresaId) return BadRequest();

                var empresa = _mapper.Map<Empresa>(empresaModel);
                await _empresaService.UpdateAsync(empresaId, empresa);

                empresaModel = _mapper.Map<EmpresaModel>(empresa);
                CreatedAtAction("GetEmpresa", new { empresaId = empresaModel.EmpresaId }, empresaModel);

                return (new ResultModel()
                {
                    Succeeded = true,
                    ObjectRetorno = empresaModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion        

        #region DeleteEmpresa
        // DELETE: api/Empresas/5
        [HttpDelete("{empresaId}")]
        public async Task<ActionResult<ResultModel>> DeleteEmpresa(int empresaId)
        {
            try
            {
                await _empresaService.RemoveAsync(empresaId);
                NoContent();
                return (new ResultModel()
                {
                    Succeeded = true,
                    ObjectRetorno = null,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region GetFiliais
        [Route("GetFiliais")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResultModel>> GetFiliais(int empresaId)
        {
            try
            {
                var empresas = await _empresaService.GetFiliais(empresaId);

                var empresasModel = _mapper.Map<List<EmpresaModel>>(empresas);

                foreach (var empresa in empresasModel)
                {
                    empresa.TipoEmpresa = ((ETipoEmpresa)empresa.Tipo).ToString();
                }

                return (new ResultModel()
                {
                    Succeeded = true,
                    ObjectRetorno = empresasModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region GetSocios
        [Route("GetSocios")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResultModel>> GetSocios(int empresaId)
        {
            try
            {
                var pessoas = await _empresaService.GetSocios(empresaId);
                return (new ResultModel()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<List<PessoaModel>>(pessoas),
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region PostEndereco
        [Route("PostEndereco")]
        [HttpPost]
        public async Task<ActionResult<ResultModel>> PostEndereco(int empresaId, EnderecoModel enderecoModel)
        {
            try
            {
                var endereco = _mapper.Map<Endereco>(enderecoModel);

                await _empresaService.ManterEnderecoAsync(empresaId, endereco);
                return (new ResultModel()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<EnderecoModel>(endereco),
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region Erro
        private ActionResult<ResultModel> Erro(ETipoErro erro, string mensagem)
        {
            return (new ResultModel()
            {
                Succeeded = false,
                ObjectRetorno = null,
                ObjectResult = (erro == ETipoErro.Fatal)
                    ? (int)EObjectResult.ErroFatal : (int)EObjectResult.BadRequest,
                Errors = (mensagem == null)
                    ? new List<string>() : new List<string> { mensagem }
            });
        }
        #endregion
    }
}
