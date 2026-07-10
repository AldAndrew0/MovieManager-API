using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MovieManager.BLL.Models;

namespace MovieManager.BLL.Services.Interfaces
{
    public interface IMovieActorService
    {
        Task<MovieActorModel?> GetByIdsAsync(int movieId, int actorId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<MovieActorModel>> GetByMovieIdAsync(int movieId, CancellationToken cancellationToken = default);
        Task<MovieActorModel> CreateAsync(MovieActorModel model, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int movieId, int actorId, CancellationToken cancellationToken = default);
    }
}