namespace Learnix.ViewModels.StudentCourseDetailsVMs
{
    public class CourseDetailsViewModel
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LearningOutCome { get; set; }
        public string Requirement { get; set; }
        public string Duration { get; set; }
        public string ImageUrl { get; set; }
        public string InstructorName { get; set; }
        public string InstructorMajor { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public string Level { get; set; }
        public decimal ProgressPercentage { get; set; }
        public int CompletedLessons { get; set; }
        public int TotalLessons { get; set; }
        public List<SectionViewModel> Sections { get; set; } //= new List<SectionViewModel>();
        public List<ReviewViewModel> Reviews { get; set; } = new List<ReviewViewModel>();
        public List<AnnouncementViewModel> Announcements { get; set; } = new List<AnnouncementViewModel>();
        public LessonViewModel CurrentLesson { get; set; } 
    }
}
