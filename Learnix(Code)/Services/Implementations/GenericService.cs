using Learnix.Repoisatories.Interfaces;
using Learnix.Services.Interfaces;

namespace Learnix.Services.Implementations
{
    public class GenericService<TEntity, TDto,DT> : IGenericService<TEntity, TDto, DT>
         where TEntity : class, new()
         where TDto : class, new()
    {

        protected readonly IUnitOfWork _unitOfWork;


        public GenericService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<TDto> GetAll()
        {
            var entities = _unitOfWork.Repository<TEntity,DT>().GetAll();
            return entities.Select(MapToDto);
        }

        public TDto GetById(DT id)
        {
            var entity = _unitOfWork.Repository<TEntity,DT>().GetByID(id);
            return MapToDto(entity);
        }

        public void Add(TDto dto)
        {
            var entity = MapToEntity(dto);
            _unitOfWork.Repository<TEntity,DT>().Add(entity);
            _unitOfWork.Complete();
        }

        public void Update(TDto dto)
        {
            var entity = MapToEntity(dto);
            _unitOfWork.Repository<TEntity,DT>().Update(entity);
            _unitOfWork.Complete();
        }

        public void Delete(DT id)
        {
            _unitOfWork.Repository<TEntity,DT>().Delete(id);
            _unitOfWork.Complete();
        }

        protected virtual TDto MapToDto(TEntity entity)
        {
            if (entity == null)
                return null;
            var dto = new TDto();
            foreach (var prop in typeof(TEntity).GetProperties())
            {
                var dtoProp = typeof(TDto).GetProperty(prop.Name);
                if (dtoProp != null)
                    dtoProp.SetValue(dto, prop.GetValue(entity));
            }
            return dto;
        }

        protected virtual TEntity MapToEntity(TDto dto)
        {
            var entity = new TEntity();
            foreach (var prop in typeof(TDto).GetProperties())
            {
                var entProp = typeof(TEntity).GetProperty(prop.Name);
                if (entProp != null)
                    entProp.SetValue(entity, prop.GetValue(dto));
            }
            return entity;
        }


        public void Save()
        {
            _unitOfWork.Complete();
        }

    }
}


