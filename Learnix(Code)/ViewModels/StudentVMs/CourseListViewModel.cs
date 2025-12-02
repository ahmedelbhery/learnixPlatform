namespace Learnix.ViewModels.StudentVMs
{
    public class CourseListViewModel
    {
        public int TotalEnrolledCourses { get; set; }
        public int CompletedCourses { get; set; }
        public int InProgressCourses { get; set; }
        public int NotStartedCourses { get; set; }
        public int CertificatesCount { get; set; }
        public List<CourseCardViewModel> Courses { get; set; }
    }
}
