using System.Threading.Tasks;

namespace Cadastro.Core.Domain.Repositories
{
    public interface IUnitOfWork
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
