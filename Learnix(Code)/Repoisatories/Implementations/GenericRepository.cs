using Learnix.Data;
using Learnix.Repoisatories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Learnix.Repoisatories.Implementations
{
    public class GenericRepository<T,DT> : IGenericRepository<T,DT> where T : class
    {
        protected LearnixContext _context;
        protected DbSet<T> _dbSet;

        public GenericRepository(LearnixContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T? GetByID(DT id)
        {
            return _dbSet.Find(id);
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public void Delete(DT id)
        {
            var entity = GetByID(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
            else
            {
                return;
            }

        }
        public void Save()
        {
            _context.SaveChanges();
        }

    }
}



