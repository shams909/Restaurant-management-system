using System;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    // 'IDisposable' forces the server to instantly close the database connection when it's done to save RAM
    public interface IUnitOfWork : IDisposable
    {
        // This magical method will instantly generate a repository for ANY table you ask for!
        IGenericRepository<T> Repository<T>() where T : class;

        // This is the trigger. It fires all pending changes to SQL Server at the exact same time.
        Task<int> SaveAsync();
    }
}
