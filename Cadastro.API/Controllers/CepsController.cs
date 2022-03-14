using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Enums;
using Cadastro.Domain.Models.Aplicacao;
using Cadastro.Domain.Models.Response;

namespace Cadastro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CepsController : ControllerBase
    {
        private readonly ICepService _cepService;
        private readonly IMapper _mapper;
        public CepsController(ICepService cepService, IMapper mapper)
        {
            _cepService = cepService;
            _mapper = mapper;
        }

        #region GetCep
        // GET: api/Ceps
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResultModel>> GetCep()
        {
            try
            {
                var ceps = await _cepService.ObterAsync();
                return (new ResultModel()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<List<CepModel>>(ceps),
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }

        // GET: api/Ceps/5
        [HttpGet("{cep}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultModel>> GetCep(string cep)
        {
            try
            {
                var Cep = await _cepService.ObterAsync(cep);
                return (new ResultModel()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<CepModel>(Cep),
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
