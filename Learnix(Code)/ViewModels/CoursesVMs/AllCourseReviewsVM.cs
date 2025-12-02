namespace Learnix.ViewModels.CoursesVMs
{
    public class AllCourseReviewsVM
    {
        public string? AdminFirstName { get; set; }
        public string? AdminLasttName { get; set; }
        public string? AdminImageUrl { get; set; }
        public List<CourseReviewVM> courseReviewVMs { get; set; } = new List<CourseReviewVM>();
        public int PageIndex { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public string? SearchTerm { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
