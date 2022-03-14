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
    public class EnderecosController : ControllerBase
    {
        private readonly IEnderecoService _enderecoService;
        private readonly IMapper _mapper;
        public EnderecosController(IEnderecoService enderecoService, IMapper mapper)
        {
            _enderecoService = enderecoService;
            _mapper = mapper;
        }

        #region GetEndereco
        // GET: api/Enderecos
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResultModel>> GetEndereco()
        {
            try
            {
                var enderecos = await _enderecoService.ObterAsync();

                var enderecosModel = _mapper.Map<List<EnderecoModel>>(enderecos);

                foreach (var endereco in enderecosModel)
                {
                    endereco.TipoEndereco = ((ETipoEndereco)endereco.Tipo).ToString();
                }

                return (new ResultModel()
                {
                    Succeeded = true,
                    ObjectRetorno = enderecosModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }

        // GET: api/Enderecos/5
        [HttpGet("{enderecoId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultModel>> GetEndereco(int enderecoId)
        {
            try
            {
                var endereco = await _enderecoService.ObterAsync(enderecoId);

                var enderecoModel = _mapper.Map<EnderecoModel>(endereco);

                enderecoModel.TipoEndereco = ((ETipoEndereco)enderecoModel.Tipo).ToString();

                return (new ResultModel()
                {
                    Succeeded = true,
                    ObjectRetorno = enderecoModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region PostEndereco
        // POST: api/Enderecos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ResultModel>> PostEndereco(EnderecoModel enderecoModel)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var endereco = _mapper.Map<Endereco>(enderecoModel);
                await _enderecoService.InsereAsync(endereco);

                enderecoModel = _mapper.Map<EnderecoModel>(endereco);
                CreatedAtAction("GetEndereco", new { enderecoId = enderecoModel.EnderecoId }, enderecoModel);

                return (new ResultModel()
                {
                    Succeeded = true,
                    ObjectRetorno = enderecoModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region PutEndereco
        // PUT: api/Enderecos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{enderecoId}")]
        public async Task<ActionResult<ResultModel>> PutEndereco(int enderecoId, EnderecoModel enderecoModel)
        {
            try
            {
                if (enderecoId != enderecoModel.EnderecoId) return BadRequest();

                var endereco = _mapper.Map<Endereco>(enderecoModel);
                await _enderecoService.UpdateAsync(enderecoId, endereco);

                enderecoModel = _mapper.Map<EnderecoModel>(endereco);
                CreatedAtAction("GetEndereco", new { enderecoId = enderecoModel.EnderecoId }, enderecoModel);

                return (new ResultModel()
                {
                    Succeeded = true,
                    ObjectRetorno = enderecoModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion        

        #region DeleteEndereco
        // DELETE: api/Enderecos/5
        [HttpDelete("{enderecoId}")]
        public async Task<ActionResult<ResultModel>> DeleteEndereco(int enderecoId)
        {
            try
            {
                await _enderecoService.RemoveAsync(enderecoId);
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
