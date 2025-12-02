namespace Learnix.ViewModels.CourseDetailsVMs
{
    public class LessonViewModel
    {
        public int LessonId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LearningObjectives { get; set; }
        public string? VideoUrl { get; set; }
        public string? Duration { get; set; }
        public int Order { get; set; }
        public List<LessonMaterialViewModel> Materials { get; set; }
    }
}
