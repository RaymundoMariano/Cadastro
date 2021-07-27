using System.Threading.Tasks;

namespace Cadastro.Domain.Contracts.Repositories.Seedwork
{
    public interface IUnitOfWork
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
