namespace Learnix.ViewModels.StudentVMs
{
    public class CourseCardViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string InstructorName { get; set; }
        public string Status { get; set; } // "In Progress", "Completed", "Not Started"
        public decimal ProgressPercentage { get; set; }
        public int CompletedLessons { get; set; }
        public int TotalLessons { get; set; }
        public string Duration { get; set; }
        public string Category { get; set; }
        public string Level { get; set; }
    }
}
