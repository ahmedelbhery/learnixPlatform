using Learnix.Dtos.CategoryDtos;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;
using Learnix.Services.Interfaces;

namespace Learnix.Services.Implementations
{
    public class CategoryService : GenericService<Category, CategoryDto,int>, ICategoryService
    {

        public CategoryService(IUnitOfWork unitOfWork) : base(unitOfWork) { }


    }
}
