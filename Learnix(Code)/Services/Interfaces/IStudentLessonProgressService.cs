using Learnix.Dtos.StudentLessonProgressDtos;
using Learnix.Models;
using Microsoft.EntityFrameworkCore;

namespace Learnix.Services.Interfaces
{
    public interface IStudentLessonProgressService : IGenericService<StudentLessonProgress, StudenLessonProgressDto,int>
    {
        public Task<double> GetCourseProgressAsync(string studentId, int courseId);
        
    }
}
