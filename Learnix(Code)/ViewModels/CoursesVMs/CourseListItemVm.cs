namespace Learnix.ViewModels.CoursesVMs
{
    public class CourseListItemVm
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string? ImageUrl { get; set; }
        public string ShortDescription { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public string InstructorName { get; set; } = "";
        public double? Price { get; set; }
        public bool IsFree { get; set; }
        public string LevelName { get; set; } = "";
        public string Duration { get; set; } = "";
        public double Rating { get; set; } = 0; // optional
    }
}
