using Learnix.Models;
using Learnix.ViewModels.AccountVMs;
using Learnix.ViewModels.AdminVMs;
using System.Threading.Tasks;

namespace Learnix.Repoisatories.Interfaces
{
    public interface IAdminRepository : IGenericRepository<Admin, string>
    {

        Task<List<string>> GetInstructorSpecialtiesAsync();
        Task<IQueryable<Instructor>> GetInstructorsWithUserAsync();
        Task<Instructor?> GetInstructorWithUserByIdAsync(string id);
        Task UpdateInstructorAsync(Instructor instructor);
        Task<int> GetCoursesCountByInstructorIdsAsync(List<string> instructorIds);
        Task<int> GetStudentsCountByInstructorIdsAsync(List<string> instructorIds);
        Task<decimal> GetEarningsByInstructorIdsAsync(List<string> instructorIds);
        Task<int> GetTotalCoursesCountAsync();
        Task<int> GetTotalStudentsCountAsync();
        Task<decimal> GetTotalEarningsAsync();
        Task<IQueryable<Instructor>> GetPendingInstructorsWithUserAsync();
        Task<InstructorStatus> GetInstructorStatus(string status);



        Task<IQueryable<Course>> GetCoursesWithIncludesAsync();
        Task<Course?> GetCourseWithIncludesByIdAsync(int id);
        Task UpdateCourseAsync(Course course);
        Task<int> GetCoursesCountAsync();
        Task<int> GetCoursesCountByStatusAsync(string status);
        Task<int> GetEnrollmentsCountAsync();
        Task<List<string>> GetCourseCategoriesAsync();
        Task<List<string>> GetCourseStatusesAsync();
        Task<int> GetEnrollmentsCountByCourseIdAsync(int courseId);
        Task<int> GetLessonsCountByCourseIdAsync(int courseId);
        Task<int> GetDurationSumByCourseIdAsync(int courseId);
    }
}
