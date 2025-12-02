namespace Learnix.Repoisatories.Interfaces
{
    public interface IGenericRepository<T,DT> where T : class
    {
        IEnumerable<T> GetAll();
        T? GetByID(DT id);
        void Add(T entity);
        void Delete(DT id);
        void Update(T entity);
        void Save();
    }
}
