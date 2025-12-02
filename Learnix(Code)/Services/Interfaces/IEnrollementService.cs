using Learnix.Dtos.EnrollementDtos;
using Learnix.Models;

namespace Learnix.Services.Interfaces
{
    public interface IEnrollementService : IGenericService<Enrollment, EnrollementDto,int>
    {
        public IEnumerable<Student> GetAllCourseStudents(int courseId, string? search = null, int pageIndex = 1, int pageSize = 10);
       
    }
}
