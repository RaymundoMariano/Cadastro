using Cadastro.Core.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cadastro.Core.Domain.Repositories
{
    public interface IEnderecoRepository : IRepository
    {
        Task<IEnumerable<Endereco>> ObterAsync();
        Task<Endereco> ObterAsync(int enderecoId);
        void Insere(Endereco endereco);
        void Update(Endereco endereco);
        void Remove(Endereco endereco);
        bool Exists(int enderecoId);
    }
}
