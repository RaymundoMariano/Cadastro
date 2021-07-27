using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Entities;
using AutoMapper;
using Cadastro.Domain.Aplication.Responses;
using Cadastro.Domain.Enums;
using Cadastro.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Acessorio.Util;

namespace Cadastro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly IPessoaService _pessoaService;
        private readonly IEnderecoPessoaService _enderecoPessoaService;
        private readonly IMapper _mapper;
        public PessoasController(IPessoaService pessoaService
            , IEnderecoPessoaService enderecoPessoaService
            , IMapper mapper)
        {
            _pessoaService = pessoaService;
            _enderecoPessoaService = enderecoPessoaService;
            _mapper = mapper;
        }

        #region GetPessoa
        // GET: api/Pessoas
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetPessoa()
        {
            try
            {
                var pessoas = await _pessoaService.ObterAsync();

                var pessoasModel = _mapper.Map<List<PessoaModel>>(pessoas);

                foreach (var pessoa in pessoasModel)
                {
                    foreach (var ep in pessoa.EnderecoPessoas)
                    {
                        ep.Endereco.TipoEndereco = ((ETipoEndereco)ep.Endereco.Tipo).ToString();
                        ep.Endereco.CEP = Formate.CEP(ep.Endereco.CEP);
                    }
                }

                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = pessoasModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }

        // GET: api/Pessoas/5
        [HttpGet("{pessoaId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResultResponse>> GetPessoa(int pessoaId)
        {
            try
            {
                var pessoa = await _pessoaService.ObterAsync(pessoaId);

                var pessoaModel = _mapper.Map<PessoaModel>(pessoa);

                foreach (var ep in pessoaModel.EnderecoPessoas)
                {
                    ep.Endereco.TipoEndereco = ((ETipoEndereco)ep.Endereco.Tipo).ToString();
                    ep.Endereco.CEP = Formate.CEP(ep.Endereco.CEP);
                }

                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = pessoaModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region PostPessoa
        // POST: api/Pessoas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> PostPessoa(PessoaModel pessoaModel)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var pessoa = _mapper.Map<Pessoa>(pessoaModel);
                await _pessoaService.InsereAsync(pessoa);

                pessoaModel = _mapper.Map<PessoaModel>(pessoa);
                CreatedAtAction("GetPessoa", new { pessoaId = pessoaModel.PessoaId }, pessoaModel);

                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = pessoaModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region PutPessoa
        // PUT: api/Pessoas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{pessoaId}")]
        public async Task<ActionResult<ResultResponse>> PutPessoa(int pessoaId, PessoaModel pessoaModel)
        {
            try
            {
                if (pessoaId != pessoaModel.PessoaId) return BadRequest();

                var pessoa = _mapper.Map<Pessoa>(pessoaModel);
                await _pessoaService.UpdateAsync(pessoaId, pessoa);

                pessoaModel = _mapper.Map<PessoaModel>(pessoa);
                CreatedAtAction("GetPessoa", new { pessoaId = pessoaModel.PessoaId }, pessoaModel);

                return (new ResultResponse()
                {
                    Succeeded = true,
                    ObjectRetorno = pessoaModel,
                    ObjectResult = (int)EObjectResult.OK,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { return Erro(ETipoErro.Sistema, ex.Message); }
            catch (Exception) { return Erro(ETipoErro.Fatal, null); }
        }
        #endregion

        #region DeletePessoa
        // DELETE: api/Pessoas/5
        [HttpDelete("{pessoaId}")]
        public async Task<ActionResult<ResultResponse>> DeletePessoa(int pessoaId)
        {
            try
            {
                await _pessoaService.RemoveAsync(pessoaId);
                NoContent();
                return (new ResultResponse()
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

        #region PostEndereco
        [Route("PostEndereco")]
        [HttpPost]
        public async Task<ActionResult<ResultResponse>> PostEndereco(int pessoaId, EnderecoModel enderecoModel)
        {
            try
            {
                var endereco = _mapper.Map<Endereco>(enderecoModel);

                await _enderecoPessoaService.ManterAsync(pessoaId, endereco);
                return (new ResultResponse()
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
        private ActionResult<ResultResponse> Erro(ETipoErro erro, string mensagem)
        {
            return (new ResultResponse()
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
