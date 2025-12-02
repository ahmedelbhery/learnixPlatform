using Learnix.Dtos.CourseDtos;

namespace Learnix.ViewModels.CoursesVMs
{
    public class AllInstructorCoursesVM
    {
        public string InstructorFirstName { get; set; }
        public string InstructorLasttName { get; set; }
        public string InstructorImageUrl { get; set; }


        public int TotalNumofInstructorCourses { get; set; }
        public int TotalNumofInstructorStudents { get; set; }
        public int TotalNumofInstructorPublishedCourses { get; set; }
        public int TotalNumofInstructorDraftedCourses { get; set; }
        public int TotalNumofInstructorRejectedCourses { get; set; }


        public IEnumerable<CourseDetailsDto> AllCoursesDetails { get; set; } = Enumerable.Empty<CourseDetailsDto>();
        public IEnumerable<PublishedCourseDetailsDto> PublishedCoursesDetails { get; set; } = Enumerable.Empty<PublishedCourseDetailsDto>();
        public IEnumerable<DraftCourseDetailsDto> DraftedCoursesDetails { get; set; } = Enumerable.Empty<DraftCourseDetailsDto>();
        public IEnumerable<RejectedCourseDetailsDto> RejectedCoursesDetails { get; set; } = Enumerable.Empty<RejectedCourseDetailsDto>();
    }
}
