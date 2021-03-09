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
    public class FiliaisController : ControllerBase
    {
        private readonly IFilialService _filialService;

        public FiliaisController(IFilialService filialService)
        {
            _filialService = filialService;
        }

        #region GetFilial
        // GET: api/Filiais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Filial>>> GetFilial()
        {
            try
            {
                var filial = await _filialService.ObterAsync();
                return filial.ToList();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        // GET: api/Filiais/5
        [HttpGet("{cgc}")]
        public async Task<ActionResult<Filial>> GetFilial(long? cgc)
        {
            try
            {
                var filial = await _filialService.ObterAsync(cgc);
                if (filial == null)
                {
                    return NotFound();
                }
                return filial;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region PostFilial
        // POST: api/Filiais
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Filial>> PostPessoaJuridica(Filial filial)
        {
            try
            {
                await _filialService.InsereAsync(filial);
                return CreatedAtAction("GetFilial", new { cgc = filial.Cgc }, filial);
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion

        #region DeleteFilial
        // DELETE: api/Filiais/5
        [HttpDelete("{cgc}")]
        public async Task<IActionResult> DeleteFilial(long? cgc)
        {
            try
            {
                await _filialService.RemoveAsync(cgc);
                return NoContent();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
