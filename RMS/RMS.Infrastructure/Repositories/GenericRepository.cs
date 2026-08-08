using Microsoft.EntityFrameworkCore;
using RMS.Application.Interfaces;
using RMS.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            // [FIX] We use FirstOrDefaultAsync instead of FindAsync because FindAsync 
            // completely ignores our Global Query Filters (The Invisible Wall)!
            return await _dbSet.FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id));
        }


        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity); // Prepares the update in memory
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity); // Prepares the delete in memory
        }
    }
}
