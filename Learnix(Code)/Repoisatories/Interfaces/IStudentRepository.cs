using Learnix.Models;

namespace Learnix.Repoisatories.Interfaces
{
    public interface IStudentRepository : IGenericRepository<Student,string>
    {
        int GetToltalNumberOfStudentsForSpeceifcInstructor(string InstructorID);

        #region mohammed Yasser & mohammed atef
        Task<List<Course>> GetEnrolledCoursesByStudentIdAsync(string studentId);
        Task<Course> GetCourseWithDetailsAsync(int courseId);
        Task<List<StudentLessonProgress>> GetStudentProgressAsync(string studentId, int courseId);
        Task<List<Section>> GetSectionsWithLessonsAsync(int courseId);
        Task<List<Review>> GetCourseReviewsAsync(int courseId);
        Task<List<Announcement>> GetCourseAnnouncementsAsync(int courseId);
        Task<int> GetTotalLessonsCountAsync(int courseId);
        Task<int> GetCompletedLessonsCountAsync(string studentId, int courseId);
        Task<StudentLessonProgress> GetLessonProgressAsync(string studentId, int lessonId);
        Task<bool> UpdateLessonProgressAsync(StudentLessonProgress progress);
        Task<Lesson> GetLessonWithDetailsAsync(int lessonId);
        Task<bool> LessonExistsAsync(int lessonId);
        Task<bool> ValidateLessonAndStudentAsync(string studentId, int lessonId);

        Task DebugCourseDataAsync(int courseId, string studentId);
        #endregion
    }
}
