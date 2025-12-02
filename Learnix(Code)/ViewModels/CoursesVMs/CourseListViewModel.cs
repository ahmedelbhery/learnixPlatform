namespace Learnix.ViewModels.CoursesVMs
{
    public class CourseListViewModel
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string InstructorName { get; set; }
        public decimal? Price { get; set; }
        public bool IsFree { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public int TotalStudents { get; set; }
        public string Duration { get; set; }
        public string Category { get; set; }
        public string Level { get; set; }
        public string Language { get; set; }
    }
}
