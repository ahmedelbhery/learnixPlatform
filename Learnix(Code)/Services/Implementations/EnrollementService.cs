using Learnix.Dtos.EnrollementDtos;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;
using Learnix.Services.Interfaces;

namespace Learnix.Services.Implementations
{
    public class EnrollementService : GenericService<Enrollment,EnrollementDto,int> , IEnrollementService
    {        
        public EnrollementService(IUnitOfWork unitOfWork) : base(unitOfWork) { }


        public IEnumerable<Student> GetAllCourseStudents(int courseId, string? search = null, int pageIndex = 1, int pageSize = 10)
        {
           return _unitOfWork.Enrollements.GetAllCourseStudents(courseId,search,pageIndex,pageSize);
        }

    }
}
