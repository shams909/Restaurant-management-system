using RMS.Application.Interfaces;
using RMS.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        // This dictionary acts as a cache so we don't recreate repositories over and over and waste memory
        private readonly Dictionary<Type, object> _repositories;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            // If we haven't created a repository for this specific table yet, create it and store it in the dictionary
            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new GenericRepository<T>(_context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T>)_repositories[type];
        }

        public async Task<int> SaveAsync()
        {
            // This is the trigger. It fires all the changes to SQL Server safely in one transaction!
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            // Instantly closes the connection to the remote database to save RAM
            _context.Dispose();
        }
    }
}
