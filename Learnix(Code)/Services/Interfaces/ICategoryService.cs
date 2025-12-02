
using Learnix.Dtos.CategoryDtos;
using Learnix.Models;

namespace Learnix.Services.Interfaces
{
    public interface ICategoryService : IGenericService<Category, CategoryDto,int>
    {
    }
}
