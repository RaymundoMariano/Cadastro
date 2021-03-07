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
    public class PessoasController : ControllerBase
    {
        private readonly IPessoaService _pessoaService;

        public PessoasController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        #region GetPessoa
        // GET: api/Pessoas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoa()
        {
            var pessoas = await _pessoaService.ObterAsync();
            return pessoas.ToList();
        }

        // GET: api/Pessoas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pessoa>> GetPessoa(int id)
        {
            var tbPessoa = await _pessoaService.ObterAsync(id);

            if (tbPessoa == null)
            {
                return NotFound();
            }

            return tbPessoa;
        }
        #endregion

        #region PutPessoa
        // PUT: api/Pessoas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPessoa(int id, Pessoa pessoa)
        {
            await _pessoaService.UpdateAsync(pessoa);

            return NoContent();
        }
        #endregion

        #region PostPessoa
        // POST: api/Pessoas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pessoa>> PostPessoa(Pessoa pessoa)
        {
            await _pessoaService.InsereAsync(pessoa);

            return CreatedAtAction("GetPessoa", new { id = pessoa.PessoaId }, pessoa);
        }
        #endregion

        #region DeletePessoa
        // DELETE: api/Pessoas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa(int id)
        {
            var pessoa = await _pessoaService.ObterAsync(id);
            if (pessoa == null)
            {
                return NotFound();
            }

            await _pessoaService.RemoveAsync(id);

            return NoContent();
        }
        #endregion
    }
}
