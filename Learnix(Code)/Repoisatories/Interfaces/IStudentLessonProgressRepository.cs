using Learnix.Models;
using Microsoft.EntityFrameworkCore;

namespace Learnix.Repoisatories.Interfaces
{
    public interface IStudentLessonProgressRepository : IGenericRepository<StudentLessonProgress,int>
    {
        public Task<double> GetCourseProgressAsync(string studentId, int courseId);
    }
}
