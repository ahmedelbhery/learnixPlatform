namespace Learnix.ViewModels.CoursesVMs
{
    public class AllCourseStudentsVM
    {
        public string CourseName { get; set; }
        public int CourseID { get; set; }
        public List<CourseStudentsVM> AllCourseStudents { get; set; } = new List<CourseStudentsVM>();

        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public string? SearchTerm { get; set; }

        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
