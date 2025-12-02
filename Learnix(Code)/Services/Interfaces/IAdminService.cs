using Learnix.Dtos.AdminDtos;
using Learnix.Models;
using Learnix.ViewModels.AccountVMs;
using Learnix.ViewModels.AdminVMs;

namespace Learnix.Services.Interfaces
{
    public interface IAdminService : IGenericService<Admin, AdminDto, string>
    {
        Task<PagedResult<AdminUserVM>> GetAllUsersAsync(string role, int pageNumber, int pageSize);
        Task<AdminUserVM?> GetUserDetailsAsync(string id);
        Task<(bool Success, string Message)> UpdateUserAsync(AdminUserVM vm);
        Task<(int totalUsers, int admins, int instructors, int students)> GetUserStatisticsAsync();



        Task<PagedResult<AdminInstructorVM>> GetAllInstructorsAsync(string specialty, int pageNumber, int pageSize);
        Task<AdminInstructorVM?> GetInstructorDetailsAsync(string id);
        Task<(bool Success, string Message)> UpdateInstructorAsync(AdminInstructorVM vm);
        Task<(int totalInstructors, int totalCourses, int totalStudents, decimal totalEarnings)> GetInstructorStatisticsAsync();
        Task<List<string>> GetSpecialtiesAsync();
        Task<PagedResult<InstructorReviewVM>> GetPendingInstructorsAsync(int pageNumber, int pageSize);
        Task<(bool Success, string Message)> ApproveInstructorAsync(string instructorId);
        Task<(bool Success, string Message)> RejectInstructorAsync(string instructorId);


        Task<PagedResult<AdminCourseVM>> GetAllCoursesAsync(string status, string category, int pageNumber, int pageSize);
        Task<AdminCourseVM?> GetCourseDetailsAsync(int id);
        Task<(bool Success, string Message)> UpdateCourseAsync(AdminCourseVM vm);
        Task<(int totalCourses, int publishedCourses, int pendingCourses, int totalEnrollments)> GetCourseStatisticsAsync();
        Task<List<string>> GetCourseCategoriesAsync();
        Task<List<string>> GetCourseStatusesAsync();

    }
}
