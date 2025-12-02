namespace Learnix.ViewModels.CourseDetailsVMs
{
    public class CourseDetailsViewModel
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LearningOutCome { get; set; }
        public string Duration { get; set; }
        public string Requirement { get; set; }
        public decimal? Price { get; set; }
        public bool IsFree { get; set; }

        public InstructorViewModel Instructor { get; set; }
        public string Category { get; set; }
        public string Level { get; set; }
        public string Language { get; set; }

        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public int TotalStudents { get; set; }
        public int TotalLessons { get; set; }

        public List<SectionViewModel> Sections { get; set; }
        public List<ReviewViewModel> Reviews { get; set; }
    }
}
