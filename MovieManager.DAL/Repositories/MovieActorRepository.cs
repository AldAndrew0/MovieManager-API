using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieManager.DAL.Data;
using MovieManager.DAL.Entities;
using MovieManager.DAL.Repositories.Interfaces;

namespace MovieManager.DAL.Repositories
{
    public class MovieActorRepository : IMovieActorRepository
    {
        private readonly MovieDbContext _context;

        public MovieActorRepository(MovieDbContext context)
        {
            _context = context;
        }

        public async Task<MovieActor?> GetByIdsAsync(int movieId, int actorId, CancellationToken cancellationToken = default)
        {
            return await _context.MovieActors
                .FirstOrDefaultAsync(ma => ma.MovieId == movieId && ma.ActorId == actorId, cancellationToken);
        }

        public async Task<IReadOnlyList<MovieActor>> GetByMovieIdAsync(int movieId, CancellationToken cancellationToken = default)
        {
            return await _context.MovieActors
                .Where(ma => ma.MovieId == movieId)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int movieId, int actorId, CancellationToken cancellationToken = default)
        {
            return await _context.MovieActors.AnyAsync(ma => ma.MovieId == movieId && ma.ActorId == actorId, cancellationToken);
        }

        public async Task AddAsync(MovieActor entity, CancellationToken cancellationToken = default)
        {
            await _context.MovieActors.AddAsync(entity, cancellationToken);
        }

        public void Remove(MovieActor entity)
        {
            _context.MovieActors.Remove(entity);
        }
    }
}