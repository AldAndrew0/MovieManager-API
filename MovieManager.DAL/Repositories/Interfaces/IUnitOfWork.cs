using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieManager.DAL.Repositories.Interfaces
{
    /// <summary>
    /// Pattern: Unit of Work.
    /// Coordina i repository e funge da gestore transazionale centralizzato. 
    /// Assicura che le modifiche multiple sui dati vengano salvate nel database (SaveChangesAsync) 
    /// come un'unica operazione atomica (tutto o niente).
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {

        IGenericRepository<T> Repository<T>() where T : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
