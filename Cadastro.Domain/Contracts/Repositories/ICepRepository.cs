using Cadastro.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Repositories
{
    public interface ICepRepository : IRepository<Cep>
    {
        Task<Cep> ObterAsync(string cep);
    }
}
