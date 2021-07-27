using Cadastro.Domain.Contracts.Repositories;
using Cadastro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cadastro.Data.EFC.Repositories
{
    public class SocioRepositoryEFC : RepositoryEFC<Socio>, ISocioRepository
    {
        public SocioRepositoryEFC(CadastroContextEFC cadastroContext) : base(cadastroContext)
        {
        }        
    }
}
