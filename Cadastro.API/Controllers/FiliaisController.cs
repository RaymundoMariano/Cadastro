using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Extensions;
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
        public async Task<ActionResult<ResponseModel>> GetFilial()
        {
            try
            {
                var filiais = _mapper.Map<List<FilialModel>>(await _filialService.ObterAsync());

                foreach(var filial in filiais) { filial.Cgc = filial.Cgc.FormateCGC(); }

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = filiais,
                    Errors = new List<string>()
                });
            }
            catch (Exception) { return Erro(null); }
        }

        // GET: api/Filiais/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseModel>> GetFilial(int id)
        {
            try
            {
                var filial = await _filialService.ObterAsync(id);

                filial.Cgc = filial.Cgc.FormateCGC();

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<FilialModel>(filial),
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { return Erro(ex.Message); }
            catch (Exception) { return Erro(null); }
        }
        #endregion

        #region PostFilial
        // POST: api/Filiais
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ResponseModel>> PostFilial(FilialModel filialModel)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var filial = _mapper.Map<Filial>(filialModel);

                filial.Cgc = filial.Cgc.RemoveMascara();

                await _filialService.InsereAsync(filial);

                filial.Cgc = filial.Cgc.FormateCGC();

                CreatedAtAction("GetFilial", new { filialId = filialModel.FilialId }, filialModel);

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<FilialModel>(filial),
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { return Erro(ex.Message); }
            catch (Exception) { return Erro(null); }
        }
        #endregion

        #region DeleteFilial
        // DELETE: api/Filiais/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseModel>> DeleteFilial(int id)
        {
            try
            {
                await _filialService.RemoveAsync(id);
                NoContent();
                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = null,
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
