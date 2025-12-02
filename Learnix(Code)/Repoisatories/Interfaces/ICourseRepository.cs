using Learnix.Models;
using Learnix.ViewModels.CourseDetailsVMs;
using System.Linq.Expressions;

namespace Learnix.Repoisatories.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Course,int>
    {
        int GetTotalNumberOfCoursebyInstructorID (string InstructorID);
        IEnumerable<Course> GetTheTop3EnrolledCoursesForSpecificInstructor (string InstructorID);
        IEnumerable<Course> GetTheTop3ProfitableCoursesForSpecificInstructor(string InstructorID);
        int GetTotalNumberOfStudentsPerCourse(int  courseID);
        double? GetTotalEarningMonyPerCourse(int courseID);
        int GetTotalNumberofPublishedCoursesforSpecificInstructor(string InstructorID);
        int GetTotalNumberofDraftedCoursesforSpecificInstructor(string InstructorID);
        int GetTotalNumberofRejectedCoursesforSpecificInstructor(string InstructorID);
        IEnumerable<Course> GetAllCoursesForSpecificInstructor(string InstructorID);
        IEnumerable<Course> GetAllPublishedCoursesForSpecificInstructor(string InstructorID);
        IEnumerable<Course> GetAllDraftedCoursesForSpecificInstructor(string InstructorID);
        IEnumerable<Course> GetAllRejectedCoursesForSpecificInstructor(string InstructorID);
        string GetCategoryNameforCourse(int courseID);
        string GetStatusNameforCourse(int courseID);
        Task<Course> GetAllCourseContent(int courseID);
        Course? GetCourseDetails(int  courseID);
        public IEnumerable<Course>? GetAllLearnixCourses();
        bool IsThisCourseBelongsToThisInstructor(int CourseID, string InstructorID);
        IEnumerable<Course> GetAllDraftedandRejectedCourses(string? search = null, int pageIndex = 1, int pageSize = 10);
        IEnumerable<Course> GetTop3PaidCourses();


        #region ahmed & abdelaziz
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int id);
        Task<IEnumerable<Course>> GetByFilterAsync(Expression<Func<Course, bool>> filter);
        Task<IEnumerable<Course>> GetCoursesByInstructorAsync(string instructorId);
        Task<InstructorStatistics> GetInstructorStatisticsAsync(string instructorId);
        #endregion




        //Task<PagedResult<Course>> GetPagedAsync(CourseListFilterVm filter,CancellationToken ct = default);

    }
}
