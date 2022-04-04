using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Cadastro.Domain.Contracts.Services;
using Cadastro.Domain.Enums;
using Cadastro.Domain.Entities;
using Cadastro.Domain.Models.Aplicacao;
using Cadastro.Domain.Models.Response;
using Cadastro.Domain.Extensions;

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
        public async Task<ActionResult<ResponseModel>> GetPessoa()
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
                        ep.Endereco.CEP = ep.Endereco.CEP.FormateCEP();
                    }
                    foreach (var pf in pessoa.PessoaFisicas) { pessoa.Cpf = pf.Cpf.FormateCPF(); }
                }

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = pessoasModel,
                    Errors = new List<string>()
                });
            }
            catch (Exception) { return Erro(null); }
        }

        // GET: api/Pessoas/5
        [HttpGet("{pessoaId}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseModel>> GetPessoa(int pessoaId)
        {
            try
            {
                var pessoa = await _pessoaService.ObterAsync(pessoaId);

                var pessoaModel = _mapper.Map<PessoaModel>(pessoa);

                foreach(var pf in pessoaModel.PessoaFisicas) { pessoaModel.Cpf = pf.Cpf.FormateCPF(); }

                foreach (var ep in pessoaModel.EnderecoPessoas)
                {
                    ep.Endereco.TipoEndereco = ((ETipoEndereco)ep.Endereco.Tipo).ToString();
                    ep.Endereco.CEP = ep.Endereco.CEP.FormateCEP();
                }

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = pessoaModel,
                    Errors = new List<string>()
                });
            }
            catch (Exception) { return Erro(null); }
        }
        #endregion

        #region PostPessoa
        // POST: api/Pessoas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ResponseModel>> PostPessoa(PessoaModel pessoaModel)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var pessoa = _mapper.Map<Pessoa>(pessoaModel);

                pessoa.Cpf.RemoveMascara();

                await _pessoaService.InsereAsync(pessoa);

                pessoa.Cpf.FormateCPF();

                CreatedAtAction("GetPessoa", new { pessoaId = pessoaModel.PessoaId }, pessoaModel);

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<PessoaModel>(pessoa),
                    Errors = new List<string>()
                });
            }
            catch (Exception) { return Erro(null); }
        }
        #endregion

        #region PutPessoa
        // PUT: api/Pessoas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{pessoaId}")]
        public async Task<ActionResult<ResponseModel>> PutPessoa(int pessoaId, PessoaModel pessoaModel)
        {
            try
            {
                if (pessoaId != pessoaModel.PessoaId) return BadRequest();

                var pessoa = _mapper.Map<Pessoa>(pessoaModel);

                pessoa.Cpf = pessoa.Cpf.RemoveMascara();

                await _pessoaService.UpdateAsync(pessoaId, pessoa);

                CreatedAtAction("GetPessoa", new { pessoaId = pessoaModel.PessoaId }, pessoaModel);

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = pessoaModel,
                    Errors = new List<string>()
                });
            }
            catch (ServiceException ex) { return Erro(ex.Message); }
            catch (Exception) { return Erro(null); }
        }
        #endregion

        #region DeletePessoa
        // DELETE: api/Pessoas/5
        [HttpDelete("{pessoaId}")]
        public async Task<ActionResult<ResponseModel>> DeletePessoa(int pessoaId)
        {
            try
            {
                await _pessoaService.RemoveAsync(pessoaId);
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

        #region PostEndereco
        [Route("PostEndereco")]
        [HttpPost]
        public async Task<ActionResult<ResponseModel>> PostEndereco(int pessoaId, EnderecoModel enderecoModel)
        {
            try
            {
                var endereco = _mapper.Map<Endereco>(enderecoModel);

                endereco.CEP = endereco.CEP.RemoveMascara();

                await _enderecoPessoaService.ManterAsync(pessoaId, endereco);

                endereco.CEP = endereco.CEP.FormateCEP();

                return (new ResponseModel()
                {
                    Succeeded = true,
                    ObjectRetorno = _mapper.Map<EnderecoModel>(endereco),
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
