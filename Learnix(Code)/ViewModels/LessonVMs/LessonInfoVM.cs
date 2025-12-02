namespace Learnix.ViewModels.LessonVMs
{
    public class LessonInfoVM
    {
        public int LessonId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LearningObjectives { get; set; }
        public string? VideoUrl { get; set; }
        public string? Duration { get; set; }
        public int Order { get; set; }

        public int SectionId { get; set; }
        public int CourseID { get; set; }
    }
}
