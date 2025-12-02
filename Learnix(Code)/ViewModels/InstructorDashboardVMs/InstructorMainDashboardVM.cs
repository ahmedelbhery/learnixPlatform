using Learnix.Dtos.CourseDtos;

namespace Learnix.ViewModels.InstructorDashboardVMs
{
    public class InstructorMainDashboardVM
    {
        public string InstructorFirstName { get; set; }
        public string InstructorLasttName { get; set; }
        public string InstructorImageUrl { get; set; }
        public int TotalNumofInstructorCourses { get; set; }
        public int TotalNumofInstructorStudents { get; set; }
        public double TotalEarning {  get; set; }
        public IEnumerable<TopPerformingCourseDto> topPerformingCoursebyStudentCapacity { get; set; } = Enumerable.Empty<TopPerformingCourseDto>();
        public IEnumerable<TopPerformingCourseDto> topPerformingCoursebyEarning { get; set; } = Enumerable.Empty<TopPerformingCourseDto>();
    }
}

