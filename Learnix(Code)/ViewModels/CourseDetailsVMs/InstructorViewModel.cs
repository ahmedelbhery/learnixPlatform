namespace Learnix.ViewModels.CourseDetailsVMs
{
    public class InstructorViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Major { get; set; }
        public int TotalCourses { get; set; }
        public int TotalStudents { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }
}
