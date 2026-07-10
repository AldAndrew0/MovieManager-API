using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MovieManager.BLL.Models;
using System.Text;

namespace MovieManager.BLL.Services.Interfaces
{
    /// <summary>
    /// Pattern: Generic Service Interface.
    /// Definisce il contratto per le operazioni di business (CRUD) basate sui modelli (TModel).
    /// Isola i controller dell'API dalla logica di accesso ai dati (Repository/UnitOfWork).
    /// </summary>
    /// <typeparam name="TModel">Il modello DTO su cui opera il servizio. Deve implementare IModelWithId.</typeparam>
    public interface IGenericService<TModel> where TModel : class, IModelWithId, new()
    {
        Task<TModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TModel>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<TModel> CreateAsync(TModel model, CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(TModel model, CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
