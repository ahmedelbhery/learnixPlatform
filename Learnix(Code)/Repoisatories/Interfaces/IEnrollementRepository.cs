using Learnix.Models;

namespace Learnix.Repoisatories.Interfaces
{
    public interface IEnrollementRepository  : IGenericRepository<Enrollment,int>
    {
        bool IsStudentEnrolled(string studentId, int courseId);

        IEnumerable<Student> GetAllCourseStudents (int courseId,string? search = null, int pageIndex = 1, int pageSize = 10);
    }
}
