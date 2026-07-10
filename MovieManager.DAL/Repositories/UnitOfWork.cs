using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieManager.DAL.Data;
using MovieManager.DAL.Repositories.Interfaces;

namespace MovieManager.DAL.Repositories
{
    /// <summary>
    /// Pattern: Unit of Work.
    /// Coordina i repository e funge da gestore transazionale centralizzato. 
    /// Assicura che le modifiche multiple sui dati vengano salvate nel database (SaveChangesAsync) 
    /// come un'unica operazione atomica (tutto o niente).
    /// </summary>
    public class UnitOfWork: IUnitOfWork
    {
        protected readonly MovieDbContext _context;
        protected readonly Dictionary<Type, object> _repositories = new();

        public UnitOfWork(MovieDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (_repositories.TryGetValue(type, out var repository))
            {
                return (IGenericRepository<T>)repository;
            }

            var newRepository = new GenericRepository<T>(_context);
            _repositories[type] = newRepository;

            return newRepository;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
