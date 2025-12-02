using Learnix.Dtos.StudentLessonProgressDtos;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;
using Learnix.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Learnix.Services.Implementations
{
    public class StudentLessonProgressService : GenericService<StudentLessonProgress,StudenLessonProgressDto,int> , IStudentLessonProgressService
    {
        public StudentLessonProgressService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public async Task<double> GetCourseProgressAsync(string studentId, int courseId)
        {
           return await _unitOfWork.StudentLessonsProgress.GetCourseProgressAsync(studentId, courseId);
        }
    }
}
