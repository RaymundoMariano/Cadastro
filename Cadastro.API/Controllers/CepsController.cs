using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Models.Aplicacao;
using Cadastro.Domain.Models.Response;
using Cadastro.Domain.Extensions;

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
        public async Task<ActionResult<ResponseModel>> GetCep()
        {
            try
            {
                var ceps = await _cepService.ObterAsync();

                foreach(var cep in ceps) { cep.CEP = cep.CEP.FormateCEP(); }

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<List<CepModel>>(ceps),
                    Errors = new List<string>()
                });
            }
            catch (Exception) { return Erro(null); }
        }

        // GET: api/Ceps/5
        [HttpGet("{cep}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseModel>> GetCep(string cep)
        {
            try
            {
                var Cep = await _cepService.ObterAsync(cep.RemoveMascara());

                Cep.CEP = Cep.CEP.FormateCEP();

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<CepModel>(Cep),
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { return Erro(ex.Message); }
            catch (Exception) { return Erro(null); }
        }
        #endregion

        #region Erro
        private ActionResult<ResponseModel> Erro(string mensagem)
        {
            return (new ResponseModel()
            {
                Succeeded = mensagem == null ? false : true,
                ObjectRetorno = null,
                Errors = (mensagem == null)
                    ? new List<string>() : new List<string> { mensagem }
            });
        }
        #endregion
    }
}
