using Learnix.Data;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;

namespace Learnix.Repoisatories.Implementations
{
    public class CategoryRepository : GenericRepository<Category,int>, ICategoryRepository
    {
        public CategoryRepository(LearnixContext context) : base(context) { }
    }
}
