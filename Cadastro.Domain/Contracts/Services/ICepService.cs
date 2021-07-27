using Cadastro.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Services
{
    public interface ICepService
    {
        Task<IEnumerable<Cep>> ObterAsync();
        Task<Cep> ObterAsync(string cep);
        Task InsereAsync(Cep cep);
    }
}
