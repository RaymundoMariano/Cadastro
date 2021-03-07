using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cadastro.Core.Domain.Models;
using Cadastro.Core.Domain.Services;

namespace Cadastro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaJuridicasController : ControllerBase
    {
        private readonly IPessoaJuridicaService _pessoaJuridicaService;

        public PessoaJuridicasController(IPessoaJuridicaService pessoaJuricaService )
        {
            _pessoaJuridicaService = pessoaJuricaService;
        }

        #region GetPessoaJuridica
        // GET: api/PessoaJuridicas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaJuridica>>> GetPessoaJuridica()
        {
            try
            {
                var pessoasJuridica = await _pessoaJuridicaService.ObterAsync();
                return pessoasJuridica.ToList();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        // GET: api/PessoaJuridicas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PessoaJuridica>> GetPessoaJuridica(long? id)
        {
            try
            {
                var pessoaJuridica = await _pessoaJuridicaService.ObterAsync(id);
                if (pessoaJuridica == null)
                {
                    return NotFound();
                }
                return pessoaJuridica;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PutPessoaJuridica
        // PUT: api/PessoaJuridicas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPessoaJuridica(long? id, PessoaJuridica pessoaJuridica)
        {
            try
            { 
                await _pessoaJuridicaService.UpdateAsync(id, pessoaJuridica);
                return NoContent();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PostPessoaJuridica
        // POST: api/PessoaJuridicas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PessoaJuridica>> PostPessoaJuridica(PessoaJuridica pessoaJuridica)
        {
            try
            {
                await _pessoaJuridicaService.InsereAsync(pessoaJuridica);
                return CreatedAtAction("GetPessoaJuridica", new { id = pessoaJuridica.Cgc }, pessoaJuridica);
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region DeletePessoaJuridica
        // DELETE: api/PessoaJuridicas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoaJuridica(long? id)
        {
            try
            {
                await _pessoaJuridicaService.RemoveAsync(id);
                return NoContent();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
