namespace Learnix.ViewModels.StudentCourseDetailsVMs
{
    public class LessonViewModel
    {
        public int LessonId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LearningObjectives { get; set; }
        public string Duration { get; set; }
        public string VideoUrl { get; set; }
        public int Order { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletionDate { get; set; }
        public List<LessonMaterialViewModel> Materials { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
    }
}
