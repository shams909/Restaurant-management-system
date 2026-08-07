using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Interfaces
{
    // The <T> is the "Generic" magic. T can be a Tenant, an Order, a Burger, anything!
    public interface IGenericRepository<T> where T : class
    {
        // Notice we use 'object' for the id. 
        // Why? Because your Tenant uses a GUID, but your Branch uses an INT!
        Task<T> GetByIdAsync(object id);

        Task<IReadOnlyList<T>> GetAllAsync();

        Task AddAsync(T entity);

        void Update(T entity);

        void Delete(T entity);
    }
}
