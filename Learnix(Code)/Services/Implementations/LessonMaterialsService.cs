using Learnix.Dtos.LessonDtos;
using Learnix.Dtos.LessonMaterialsDtos;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;
using Learnix.Services.Interfaces;

namespace Learnix.Services.Implementations
{
    public class LessonMaterialsService : GenericService<LessonMaterial,LessonMaterialDto,int> , ILessonMaterialsService
    {
        public LessonMaterialsService(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
