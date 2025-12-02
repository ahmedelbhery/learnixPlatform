namespace Learnix.ViewModels.CoursesVMs
{
    public class HomePageCoursesVM
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public string Title { get; set; }
        public double? Price { get; set; }
        public string? InstructorFullName { get; set; }
        public string? InstructorImageUrl { get; set; }
        public string? CategoryName { get; set; }
        public int NumberofStudents { get; set; }
    }
}
