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
    public class FiliaisController : ControllerBase
    {
        private readonly IFilialService _filialService;
        private readonly IMapper _mapper;
        public FiliaisController(IFilialService filialService, IMapper mapper)
        {
            _filialService = filialService ?? throw new ArgumentNullException(nameof(filialService));
            _mapper = mapper;
        }

        #region GetFilial
        // GET: api/Filiais
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResultModel>> GetFilial()
        {
            try
            {
                var filiais = await _filialService.ObterAsync();
                return (new ResultModel()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<List<FilialModel>>(filiais),
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }

        // GET: api/Filiais/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultModel>> GetFilial(int id)
        {
            try
            {
                var filial = await _filialService.ObterAsync(id);
                return (new ResultModel()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<FilialModel>(filial),
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region PostFilial
        // POST: api/Filiais
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ResultModel>> PostFilial(FilialModel filialModel)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var filial = _mapper.Map<Filial>(filialModel);
                await _filialService.InsereAsync(filial);

                filialModel = _mapper.Map<FilialModel>(filial);
                CreatedAtAction("GetFilial", new { filialId = filialModel.FilialId }, filialModel);

                return (new ResultModel()
                {
                    Succeeded = true,
                    ObjectRetorno = filialModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region DeleteFilial
        // DELETE: api/Filiais/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResultModel>> DeleteFilial(int id)
        {
            try
            {
                await _filialService.RemoveAsync(id);
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
