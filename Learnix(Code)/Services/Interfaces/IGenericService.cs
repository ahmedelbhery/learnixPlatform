namespace Learnix.Services.Interfaces
{
    public interface IGenericService<TEntity, TDto,DT> where TEntity : class where TDto : class
    {
        IEnumerable<TDto> GetAll();
        TDto GetById(DT id);
        void Add(TDto dto);
        void Update(TDto dto);
        void Delete(DT id);
        public void Save();
    }
}

