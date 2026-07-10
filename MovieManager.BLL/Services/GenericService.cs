using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MovieManager.BLL.Models;
using MovieManager.BLL.Services.Interfaces;
using MovieManager.DAL.Repositories.Interfaces;

namespace MovieManager.BLL.Services
{
    /// <summary>
    /// Pattern: Generic Service.
    /// Implementazione centralizzata della logica di business (CRUD).
    /// Fa da ponte tra l'API e il DAL, utilizzando AutoMapper per convertire Entità e Modelli.
    /// </summary>
    /// <typeparam name="TEntity">L'entità del database (DAL).</typeparam>
    /// <typeparam name="TModel">Il modello di trasferimento dati (BLL).</typeparam>
    public class GenericService<TEntity, TModel> : IGenericService<TModel>
        where TEntity : class, new()
        where TModel : class, IModelWithId, new()
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IGenericRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

        public GenericService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = _unitOfWork.Repository<TEntity>();
        }

        public async Task<TModel?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);

            return entity == null ? null : _mapper.Map<TModel>(entity);
        }

        public async Task<IReadOnlyList<TModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _repository.GetAllAsync(cancellationToken);

            return _mapper.Map<IReadOnlyList<TModel>>(entities);
        }

        public async Task<TModel> CreateAsync(TModel model, CancellationToken cancellationToken = default)
        {
            var entity = _mapper.Map<TEntity>(model);

            await _repository.AddAsync(entity, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TModel>(entity);
        }

        public async Task<bool> UpdateAsync(TModel model, CancellationToken cancellationToken = default)
        {
            var existingEntity = await _repository.GetByIdAsync(model.Id, cancellationToken);
            if (existingEntity == null)
            {
                return false;
            }

            _mapper.Map(model, existingEntity);

            _repository.Update(existingEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                return false;
            }

            _repository.Remove(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}