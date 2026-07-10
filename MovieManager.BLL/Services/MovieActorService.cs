using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces;
using MovieManager.DAL.Entities;
using MovieManager.DAL.Repositories.Interfaces;

namespace MovieManager.BLL.Services
{
    public class MovieActorService : IMovieActorService
    {
        private readonly IMovieActorRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieActorService(IMovieActorRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MovieActorModel?> GetByIdsAsync(int movieId, int actorId, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdsAsync(movieId, actorId, cancellationToken);
            return entity == null ? null : _mapper.Map<MovieActorModel>(entity);
        }

        public async Task<IReadOnlyList<MovieActorModel>> GetByMovieIdAsync(int movieId, CancellationToken cancellationToken = default)
        {
            var entities = await _repository.GetByMovieIdAsync(movieId, cancellationToken);
            return _mapper.Map<IReadOnlyList<MovieActorModel>>(entities);
        }

        public async Task<MovieActorModel> CreateAsync(MovieActorModel model, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<MovieActor>(model);
            await _repository.AddAsync(entity, cancellationToken);

        
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<MovieActorModel>(entity);
        }

        public async Task<bool> DeleteAsync(int movieId, int actorId, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdsAsync(movieId, actorId, cancellationToken);
            if (entity == null) return false;

            _repository.Remove(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}