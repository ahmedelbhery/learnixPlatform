using Learnix.Dtos.CourseDtos;
using Learnix.Dtos.CourseLanguageDtos;
using Learnix.Dtos.CourseLevelDtos;
using Learnix.Dtos.CourseStatusDtos;
using Learnix.Models;
using Learnix.ViewModels.CourseDetailsVMs;
using Learnix.ViewModels.CoursesVMs;
using Learnix.ViewModels.LessonVMs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;

namespace Learnix.Services.Interfaces
{
    public interface ICourseService : IGenericService<Course, CourseDto,int>
    {
        int GetTotalNumberOfCoursebyInstructorID(string InstructorID);
        IEnumerable<TopPerformingCourseDto> GetTheTop3EnrolledCoursesForSpecificInstructor(string InstructorID);
        IEnumerable<TopPerformingCourseDto> GetTheTop3ProfitableCoursesForSpecificInstructor(string InstructorID);
        double? GetTotalEarningMonyPerCourse(int courseID);
        Task<CreateCourseVM> GetCreateCourseVMAsync(string UserName);
        EditCourseVM GetEditCourseVM(int CourseId);
        Task<AllInstructorCoursesVM> GetAllInstructorCoursesVMAsync(string UserName);
        public int GetTotalNumberofDraftedCoursesforSpecificInstructor(string InstructorID);
        public int GetTotalNumberofPublishedCoursesforSpecificInstructor(string InstructorID);
        public int GetTotalNumberofRejectedCoursesforSpecificInstructor(string InstructorID);
        public IEnumerable<CourseDetailsDto> GetAllCoursesForSpecificInstructor(string InstructorID);
        public IEnumerable<DraftCourseDetailsDto> GetAllDraftedCoursesForSpecificInstructor(string InstructorID);
        public IEnumerable<PublishedCourseDetailsDto> GetAllPublishedCoursesForSpecificInstructor(string InstructorID);
        public IEnumerable<RejectedCourseDetailsDto> GetAllRejectedCoursesForSpecificInstructor(string InstructorID);
        public string GetCategoryNameforCourse(int courseID);
        public string GetStatusNameforCourse(int courseID);
        public Task<CourseContentVM> GetCourseContentAsync(int courseId);
        public bool UpdateCourse(EditCourseVM vm);
        public CourseDetailsVM? GetCourseDetails(int courseID);
        public IEnumerable<CourseDetailsVM>? GetAllLearnixCourses();
        Task<bool> EnrollInCourse(int CourseID, string StudentID);
        Task<AllCourseStudentsVM> GetAllCourseStudents(int courseId,string? search = null, int pageIndex = 1, int pageSize = 10);
        public bool IsThisCourseBelongsToThisInstructor(int CourseID, string InstructorID);
        public IEnumerable<CourseDto> GetAllCoursesBelongstoInstructor(string InstructorID);
        public List<CourseReviewVM> GetAllDraftedandRejectedCourses(string? search = null, int pageIndex = 1, int pageSize = 10);
        public void ApproveCourseStatus(int courseID);
        public void RejectCourseStatus(int courseID);
        public IEnumerable<HomePageCoursesVM> GetTop3PaidCourses();

        /*         
         
        Task<PagedResult<CourseListItemVm>> GetCoursesAsync(CourseListFilterVm filter, CancellationToken ct = default);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<CourseLevel>> GetAllLevelsAsync();
        Task<IEnumerable<ApplicationUser>> GetAllInstructorsAsync();
         
         
         */




        #region ahmed & abdelaziz
        Task<IEnumerable<CourseListViewModel>> GetAllCoursesAsync(CourseFilterViewModel? filter = null);
        Task<CourseDetailsViewModel?> GetCourseDetailsAsync(int id);
        Task<IEnumerable<CourseListViewModel>> GetCoursesByInstructorAsync(string instructorId);
        #endregion



    }
}
