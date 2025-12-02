using Learnix.Dtos.InstructorDtos;
using Learnix.Models;
using Learnix.Repoisatories.Interfaces;
using Learnix.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Learnix.Services.Implementations
{
    public class InstructorService : GenericService<Instructor,InstructorDto,string> , IInstructorService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICourseService CourseService;
        private readonly IStudentService StudentService;
        public InstructorService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager,ICourseService courseService,IStudentService studentService) : base(unitOfWork) 
        {
            this.userManager = userManager;
            this.CourseService = courseService;
            this.StudentService = studentService;
        }

        public double GetTotalRevenueForInstructor(string instructorId)
        {
            return _unitOfWork.Instructors.GetTotalRevenueForInstructor(instructorId);
        }

        public async Task<InstructorMainDashboardDto> MainDashboard(string id)
        {

            var CurrentUser = await userManager.FindByIdAsync(id);
            var CurrentInstructor = _unitOfWork.Instructors.GetByID(id);

            if (CurrentInstructor == null)
            {
                return null;
            }

            InstructorMainDashboardDto instructorMainDashboardDto = new InstructorMainDashboardDto();
            instructorMainDashboardDto.InstructorFirstName = CurrentUser.FirstName;
            instructorMainDashboardDto.InstructorLasttName = CurrentUser.LastName;
            instructorMainDashboardDto.InstructorImageUrl = CurrentUser.ImageUrl;
            instructorMainDashboardDto.TotalNumofInstructorCourses = CourseService.GetTotalNumberOfCoursebyInstructorID(id);
            instructorMainDashboardDto.TotalEarning = Convert.ToDouble(CurrentInstructor.Balance);//GetTotalRevenueForInstructor(id);
            instructorMainDashboardDto.TotalNumofInstructorStudents = StudentService.GetToltalNumberOfStudentsForSpeceifcInstructor(id);
            instructorMainDashboardDto.topPerformingCoursebyStudentCapacity = CourseService.GetTheTop3EnrolledCoursesForSpecificInstructor(id);
            instructorMainDashboardDto.topPerformingCoursebyEarning = CourseService.GetTheTop3ProfitableCoursesForSpecificInstructor(id);

            return instructorMainDashboardDto;
        }
    }
}
