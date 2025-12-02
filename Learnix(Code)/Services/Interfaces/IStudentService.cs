using Learnix.Dtos.StudentDtos;
using Learnix.Models;
using Learnix.ViewModels.StudentCourseDetailsVMs;
using Learnix.ViewModels.StudentVMs;


namespace Learnix.Services.Interfaces
{
    public interface IStudentService : IGenericService<Student, StudentDto,string>
    {
        int GetToltalNumberOfStudentsForSpeceifcInstructor(string InstructorID);

        #region mohammed Yasser & mohammed atef
        Task<CourseListViewModel> GetStudentCoursesAsync(string studentId);
        Task<CourseDetailsViewModel> GetCourseDetailsAsync(int courseId, string studentId);
        Task<bool> MarkLessonAsCompletedAsync(string studentId, int lessonId);
        Task<bool> IsStudentEnrolledInCourseAsync(string studentId, int courseId);
        Task<LessonViewModel> GetLessonDetailsAsync(int lessonId, string studentId);
        #endregion
    }
}
