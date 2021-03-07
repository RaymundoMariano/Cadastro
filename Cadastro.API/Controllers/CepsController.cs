using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cadastro.Core.Domain.Models;
using Cadastro.Core.Domain.Services;
using System;

namespace Cadastro.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CepsController : ControllerBase
    {
        private readonly ICepService _cepService;

        public CepsController(ICepService cepService)
        {
            _cepService = cepService;
        }

        #region GetCep
        // GET: api/Ceps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cep>>> GetCep()
        {
            try
            {
                var ceps = await _cepService.ObterAsync();
                return ceps.ToList();
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }

        // GET: api/Ceps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cep>> GetCep(long? id)
        {
            try
            {
                var cep = await _cepService.ObterAsync(id);
                if (cep == null) return NotFound();
                return cep;
            }
            catch (ServiceException ex) { throw new ServiceException(ex.Message, ex.InnerException); }
            catch (Exception ex) { throw new Exception(ex.Message, ex.InnerException); }
        }
        #endregion
    }
}
