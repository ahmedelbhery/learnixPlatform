using Learnix.Dtos.CourseDtos;

namespace Learnix.Dtos.InstructorDtos
{
    public class InstructorMainDashboardDto
    {
        public string InstructorFirstName { get; set; }
        public string InstructorLasttName { get; set; }
        public string InstructorImageUrl { get; set; }
        public int TotalNumofInstructorCourses { get; set; }
        public int TotalNumofInstructorStudents { get; set; }
        public double TotalEarning { get; set; }
        public IEnumerable<TopPerformingCourseDto>? topPerformingCoursebyStudentCapacity { get; set; }
        public IEnumerable<TopPerformingCourseDto>? topPerformingCoursebyEarning { get; set; }
    }
}
