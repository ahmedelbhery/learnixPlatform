using Learnix.Data;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;

namespace Learnix.Repoisatories.Implementations
{
    public class LessonMaterialsRepository : GenericRepository<LessonMaterial,int>, ILessonMaterialsRepository
    {
        public LessonMaterialsRepository(LearnixContext context) : base(context) { }
    }
}
