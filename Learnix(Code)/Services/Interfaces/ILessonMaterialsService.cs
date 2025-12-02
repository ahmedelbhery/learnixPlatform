using Learnix.Dtos.LessonMaterialsDtos;
using Learnix.Models;

namespace Learnix.Services.Interfaces
{
    public interface ILessonMaterialsService : IGenericService<LessonMaterial, LessonMaterialDto,int>
    {
    }
}
